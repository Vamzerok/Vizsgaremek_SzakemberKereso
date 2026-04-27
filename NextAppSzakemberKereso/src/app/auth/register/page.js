"use client";

import { Suspense, useState, useEffect } from "react";
import { useRouter, useSearchParams } from "next/navigation";

import { mutate } from "swr";

import Collapse from "@mui/material/Collapse";
import Box from "@mui/material/Box";
import ToggleButtonGroup from "@mui/material/ToggleButtonGroup";
import Typography from "@mui/material/Typography";
import Link from "@mui/material/Link";
import CircularProgress from "@mui/material/CircularProgress";
import Stack from "@mui/material/Stack";
import TextField from "@mui/material/TextField";
import ToggleButton from "@mui/material/ToggleButton";
import NextLink from "next/link";
import Button from "@mui/material/Button";
import Alert from "@mui/material/Alert";

import WorkLocationSelector from "@/components/WorkLocationSelector";
import useUser from "@/hooks/useUser";

const PASSWORD_RULES = {
  numbers: {
    requiredValue: 1,
    actualValue: (value) => value.matchAll(/\d/g).toArray().length,
    isValid: (required, actual) => required <= actual,
    text: (required, actual) => `Legalább ${required} szám (${actual}/${required})`,
  },
  specialChars: {
    requiredValue: 1,
    actualValue: (value) => value.matchAll(/\W/g).toArray().length,
    isValid: (required, actual) => required <= actual,
    text: (required, actual) => `Legalább ${required} speciális karakter (${actual}/${required})`,
  },
  upperOrLower: {
    requiredValue: 1,
    actualValue: (value) => value.matchAll(/(?=.*[a-z])(?=.*[A-Z])/g).toArray().length,
    isValid: (required, actual) => required <= actual,
    text: () => "Kis- és nagybetűt is kell tartalmaznia",
  },
  length: {
    requiredValue: 6,
    actualValue: (value) => value.length,
    isValid: (required, actual) => required <= actual,
    text: (required, actual) => `Legalább ${required} karakter (${actual}/${required})`,
  },
};

function evaluatePasswordRules(rules, pwd) {
  const result = {};
  for (const rn in rules) {
    const rule = rules[rn];
    const actualValue = rule.actualValue(pwd);
    result[rn] = {
      requiredValue: rule.requiredValue,
      actualValue,
      isValid: rule.isValid(rule.requiredValue, actualValue),
      text: rule.text(rule.requiredValue, actualValue),
    };
  }
  return result;
}

function RegisterPageContent() {
  const router = useRouter();
  const searchParams = useSearchParams();
  const from = searchParams.get("from") || "/";

  const { user, isLoading: userLoading, isValidating: userValidating, error: userError } = useUser();

  const [accountType, setAccountType] = useState("generic");
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPwd, setConfirmPwd] = useState("");
  const [companyEmail, setCompanyEmail] = useState("");
  const [companyPhone, setCompanyPhone] = useState("");
  const [workLocation, setWorkLocation] = useState(null);
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);
  const [submitWasPressed, setSubmitWasPressed] = useState(false);

  const evaluatedRules = evaluatePasswordRules(PASSWORD_RULES, password);
  const passwordIsStrong = Object.values(evaluatedRules).every((r) => r.isValid);
  const passwordsMatch = password === confirmPwd;

  useEffect(() => {
    if (!userLoading && !userValidating && user && !userError) {
      router.replace(from);
    }
  }, [user, userLoading, userValidating, userError, from, router]);

  async function handleSubmit(e) {
    e.preventDefault();
    setSubmitWasPressed(true);
    setError("");

    if (!passwordIsStrong) {
      setError("A jelszó nem felel meg a követelményeknek.");
      return;
    }
    if (!passwordsMatch) {
      setError("A jelszavak nem egyeznek.");
      return;
    }
    if (accountType === "expert" && (!companyEmail || !companyPhone || !workLocation)) {
      setError("Kérjük töltsd ki az összes kötelező mezőt.");
      return;
    }

    setLoading(true);
    try {
      const endpoint = accountType === "expert" ? "/api/register/expert" : "/api/register";
      const body =
        accountType === "expert"
          ? { email, password, firstName, lastName, companyEmail, companyPhoneNumber: companyPhone, workLocationId: workLocation.id }
          : { email, password, firstName, lastName };

      const res = await fetch(endpoint, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
        body: JSON.stringify(body),
      });

      if (res.ok) {
        await mutate("/api/Users/me");
        router.replace(from);
      } else if (res.status === 400) {
        const data = await res.json();
        const messages = Array.isArray(data)
          ? data.map((e) => e.description).join(" ")
          : data?.title ?? "Hiba történt.";
        setError(messages);
      } else {
        setError("Hiba történt.");
      }
    } catch {
      setError("Hiba történt.");
    } finally {
      setLoading(false);
    }
  }

  return (
    <Box>
      <Typography variant="h5" fontWeight={600} textAlign="center" gutterBottom>
        Fiók létrehozása
      </Typography>

      <Stack component="form" spacing={2} onSubmit={handleSubmit} sx={{ mt: 3 }}>
        <Stack direction="row" spacing={2}>
          <TextField
            label="Vezetéknév"
            value={lastName}
            onChange={(e) => setLastName(e.target.value)}
            autoComplete="family-name"
            fullWidth
            required
            slotProps={{ htmlInput: { maxLength: 32 } }}
          />
          <TextField
            label="Keresztnév"
            value={firstName}
            onChange={(e) => setFirstName(e.target.value)}
            autoComplete="given-name"
            fullWidth
            required
            slotProps={{ htmlInput: { maxLength: 32 } }}
          />
        </Stack>

        <ToggleButtonGroup
          value={accountType}
          exclusive
          onChange={(_, val) => { if (val) setAccountType(val); }}
          fullWidth
        >
          <ToggleButton value="generic">Felhasználó</ToggleButton>
          <ToggleButton value="expert">Szakember</ToggleButton>
        </ToggleButtonGroup>

        <TextField
          type="email"
          label="Email cím"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          autoComplete="email"
          fullWidth
          required
          slotProps={{ htmlInput: { maxLength: 256 } }}
        />

        <TextField
          type="password"
          label="Jelszó"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          autoComplete="new-password"
          fullWidth
          required
        />

        <TextField
          type="password"
          label="Jelszó megerősítése"
          value={confirmPwd}
          onChange={(e) => setConfirmPwd(e.target.value)}
          autoComplete="new-password"
          fullWidth
          required
          error={submitWasPressed && !passwordsMatch}
          helperText={submitWasPressed && !passwordsMatch ? "A jelszavak nem egyeznek." : ""}
        />

        <Collapse in={accountType === "expert"}>
          <Stack spacing={2}>
            <Stack direction="row" spacing={2}>
              <TextField
                label="Vállalkozás telefonszáma"
                value={companyPhone}
                onChange={(e) => setCompanyPhone(e.target.value)}
                fullWidth
                required={accountType === "expert"}
                slotProps={{ htmlInput: { maxLength: 32 } }}
              />
              <TextField
                type="email"
                label="Vállalkozás emailje"
                value={companyEmail}
                onChange={(e) => setCompanyEmail(e.target.value)}
                fullWidth
                required={accountType === "expert"}
                slotProps={{ htmlInput: { maxLength: 256 } }}
              />
            </Stack>
            <WorkLocationSelector
              onSettlementChange={setWorkLocation}
              required={accountType === "expert"}
            />
          </Stack>
        </Collapse>

        {error && <Alert severity="error">{error}</Alert>}
        {submitWasPressed && Object.values(evaluatedRules).some((r) => !r.isValid) && Object.values(evaluatedRules).map((rule, i) => (
          <Alert variant="outlined" severity={rule.isValid ? "success" : "warning"} key={i}>
            {rule.text}
          </Alert>
        ))}

        <Button
          type="submit"
          variant="contained"
          size="large"
          fullWidth
          disabled={loading}
          startIcon={loading ? <CircularProgress size={18} color="inherit" /> : null}
        >
          {loading ? "" : "Regisztráció"}
        </Button>

        <Typography variant="body2" textAlign="center">
          {"Már van fiókod? Jelentkezz be: "}
          <Link component={NextLink} href="/auth/login">
            Bejelentkezés
          </Link>
        </Typography>
      </Stack>
    </Box>
  );
}

export default function RegisterPage() {
  return (
    <Suspense>
      <RegisterPageContent />
    </Suspense>
  );
}
