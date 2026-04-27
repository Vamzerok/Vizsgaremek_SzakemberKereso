"use client";
import { Suspense, useState, useEffect } from "react";
import { useRouter, useSearchParams } from "next/navigation";

import { mutate } from "swr";

import TextField from "@mui/material/TextField";
import Alert from "@mui/material/Alert";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import Stack from "@mui/material/Stack";
import Typography from "@mui/material/Typography";
import CircularProgress from "@mui/material/CircularProgress";
import Link from "@mui/material/Link";

import NextLink from "next/link";

import useUser from "@/hooks/useUser";

function LoginPageContent() {
  const router = useRouter();
  const searchParams = useSearchParams();
  const from = searchParams.get("from") || "/";

  const { user, isLoading: userLoading, isValidating: userValidating, error: userError } = useUser();

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    if (!userLoading && !userValidating && user && !userError) {
      router.replace(from);
    }
  }, [user, userLoading, userValidating, userError, from, router]);

  async function handleSubmit(e) {
    e.preventDefault();
    setError("");
    setLoading(true);

    try {
      const res = await fetch('/api/login', {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
        body: JSON.stringify({ email, password }),
      });

      if (res.ok) {
        await mutate('/api/Users/me');
        router.replace(from);
      } else if (res.status === 401 || res.status === 404) {
        setError("Érvénytelen email cím vagy jelszó.");
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
        Bejelentkezés
      </Typography>

      <Stack component="form" spacing={2} onSubmit={handleSubmit} sx={{ mt: 3 }}>
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
          autoComplete="current-password"
          fullWidth
          required
        />

        {error && <Alert severity="error">{error}</Alert>}

        <Button
          type="submit"
          variant="contained"
          size="large"
          fullWidth
          disabled={loading}
          startIcon={loading ? <CircularProgress size={18} color="inherit" /> : null}
        >
          {loading ? "" : "Bejelentkezés"}
        </Button>

        <Typography variant="body2" textAlign="center">
          {"Még nincs fiókod? "}
          <Link component={NextLink} href="/auth/register">
            Regisztrálj
          </Link>
        </Typography>
      </Stack>
    </Box>
  );
}

export default function LoginPage() {
  return (
    <Suspense>
      <LoginPageContent />
    </Suspense>
  );
}
