"use client";

import { useState, useRef } from "react";

import Avatar from "@mui/material/Avatar";
import Divider from "@mui/material/Divider";
import Paper from "@mui/material/Paper";
import Button from "@mui/material/Button";
import TextField from "@mui/material/TextField";
import Box from "@mui/material/Box";
import Container from "@mui/material/Container";
import Skeleton from "@mui/material/Skeleton";
import CircularProgress from "@mui/material/CircularProgress";
import Alert from "@mui/material/Alert";
import Stack from "@mui/material/Stack";
import Typography from "@mui/material/Typography";

import useUser from "@/hooks/useUser";
import useExpert from "@/hooks/useExpert";
import { isUserAnExpert } from "@/utils/user";
import WorkLocationSelector from "@/components/WorkLocationSelector";

export default function ProfilePage() {
  const { user, isLoading: userLoading, mutate: mutateUser } = useUser();
  const { expert, isLoading: expertLoading, mutate: mutateExpert } = useExpert();

  const [saving, setSaving] = useState(false);
  const [status, setStatus] = useState(null); // null | "success" | Error

  const workLocationRef = useRef(undefined); // undefined = untouched, null = explicitly cleared, Settlement = selected

  const firstNameRef = useRef(null);
  const lastNameRef = useRef(null);
  const phoneRef = useRef(null);
  const biographyRef = useRef(null);
  const companyPhoneRef = useRef(null);
  const companyEmailRef = useRef(null);

  const isExpert = isUserAnExpert(user);
  const isLoading = userLoading || (isExpert && expertLoading);
  const initials = `${user?.firstName?.[0] ?? ""}${user?.lastName?.[0] ?? ""}`.toUpperCase() || "?";

  async function handleSave() {
    const effectiveWorkLocation = workLocationRef.current === undefined
      ? expert?.workLocation
      : workLocationRef.current;

    if (isExpert && !effectiveWorkLocation?.id) {
      setStatus(new Error("A munkavégzés helyszíne kötelező."));
      return;
    }

    setSaving(true);
    setStatus(null);

    try {
      const userRes = await fetch("/api/Users/me", {
        method: "PUT",
        credentials: "include",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          firstName: firstNameRef.current?.value?.trim(),
          lastName: lastNameRef.current?.value?.trim(),
          phoneNumber: phoneRef.current?.value?.trim(),
        }),
      });
      if (!userRes.ok) throw new Error("A profil frissítése sikertelen.");
      mutateUser();

      if (isExpert) {
        const expertRes = await fetch("/api/Experts/me", {
          method: "PUT",
          credentials: "include",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({
            biography: biographyRef.current?.value?.trim() || null,
            companyPhoneNumber: companyPhoneRef.current?.value?.trim(),
            companyEmail: companyEmailRef.current?.value?.trim(),
            workLocationId: effectiveWorkLocation.id,
          }),
        });
        if (!expertRes.ok) throw new Error("A szakember adatok frissítése sikertelen.");
        mutateExpert();
      }

      setStatus("success");
    } catch (e) {
      setStatus(e);
    } finally {
      setSaving(false);
    }
  }

  if (isLoading) {
    return (
      <Container maxWidth="md" sx={{ py: 3 }}>
        <Stack direction={{ xs: "column", md: "row" }} spacing={3} alignItems={{ md: "flex-start" }}>
          <Paper variant="outlined" sx={{ p: 3, width: { xs: "100%", md: 200 }, flexShrink: 0 }}>
            <Stack spacing={1.5} alignItems="center">
              <Skeleton variant="circular" width={80} height={80} />
              <Skeleton variant="text" width={120} height={22} />
              <Skeleton variant="text" width={150} height={18} />
            </Stack>
          </Paper>
          <Stack spacing={2} sx={{ flex: 1 }}>
            <Skeleton variant="rectangular" height={130} sx={{ borderRadius: 2 }} />
            <Skeleton variant="rectangular" height={260} sx={{ borderRadius: 2 }} />
          </Stack>
        </Stack>
      </Container>
    );
  }

  return (
    <Container maxWidth="lg" sx={{ py: 3 }}>
      {status === "success" && (
        <Alert severity="success" sx={{ mb: 2 }} onClose={() => setStatus(null)}>
          A profil sikeresen frissítve.
        </Alert>
      )}
      {status instanceof Error && (
        <Alert severity="error" sx={{ mb: 2 }} onClose={() => setStatus(null)}>
          {status.message}
        </Alert>
      )}

      <Stack direction={{ xs: "column", md: "row" }} spacing={3} alignItems={{ md: "flex-start" }}>

        <Paper sx={{ width: { xs: "100%", md: 300 }, p: 3,  flexShrink: 0 }} variant="outlined">
          <Stack spacing={1.5} alignItems="center">
            <Avatar sx={{ width: 80, height: 80, fontSize: 28 }}>{initials}</Avatar>
            <Box sx={{ textAlign: "center" }}>
              <Typography variant="subtitle1" fontWeight={600} lineHeight={1.3}>
                {user?.firstName} {user?.lastName}
              </Typography>
              <Typography variant="body2" color="text.secondary" sx={{ wordBreak: "break-all" }}>
                {user?.email}
              </Typography>
            </Box>
            {isExpert && (
              <Typography variant="caption" color="primary" fontWeight={500}>
                Szakember
              </Typography>
            )}
          </Stack>
        </Paper>

        <Stack spacing={2} sx={{ flex: 1 }}>
          <Paper sx={{ p: 3 }} variant="outlined">
            <Typography variant="subtitle1" fontWeight={600} gutterBottom>
              Személyes adatok
            </Typography>
            <Divider sx={{ mb: 2 }} />
            <Stack spacing={2}>
              <Stack direction={{ xs: "column", sm: "row" }} spacing={2}>
                <TextField
                  label="Vezetéknév"
                  defaultValue={user?.lastName ?? ""}
                  inputRef={lastNameRef}
                  required
                  fullWidth
                  slotProps={{ htmlInput: { maxLength: 32 } }}
                />
                <TextField
                  label="Keresztnév"
                  defaultValue={user?.firstName ?? ""}
                  fullWidth
                  required
                  inputRef={firstNameRef}
                  slotProps={{ htmlInput: { maxLength: 32 } }}
                />
              </Stack>
              <TextField
                label="Telefonszám"
                inputRef={phoneRef} 
                defaultValue={user?.phoneNumber ?? ""} 
                fullWidth
              />
            </Stack>
          </Paper>

          {isExpert && (
            <Paper sx={{ p: 3 }} variant="outlined">
              <Typography variant="subtitle1" fontWeight={600} gutterBottom>
                Szakember adatok
              </Typography>
              <Divider sx={{ mb: 2 }} />
              <Stack spacing={2}>
                <TextField
                  label="Bemutatkozás"
                  defaultValue={expert?.biography ?? ""}
                  inputRef={biographyRef}
                  multiline
                  minRows={3}
                  fullWidth
                  slotProps={{ htmlInput: { maxLength: 512 } }}
                />
                <Stack direction={{ xs: "column", sm: "row" }} spacing={2}>
                  <TextField
                    label="Vállalkozás telefonszáma"
                    defaultValue={expert?.companyPhoneNumber ?? ""}
                    inputRef={companyPhoneRef}
                    required
                    fullWidth
                    slotProps={{ htmlInput: { maxLength: 32 } }}
                  />
                  <TextField
                    label="Vállalkozás emailje"
                    type="email"
                    defaultValue={expert?.companyEmail ?? ""}
                    inputRef={companyEmailRef}
                    required
                    fullWidth
                    slotProps={{ htmlInput: { maxLength: 256 } }}
                  />
                </Stack>
                <WorkLocationSelector
                  key={expert?.workLocation?.id ?? "none"} 
                  initialSettlement={expert?.workLocation} 
                  onSettlementChange={(s) => { workLocationRef.current = s; }}
                  required
                />
              </Stack>
            </Paper>
          )}

          <Box sx={{ display: "flex", justifyContent: "flex-end" }}>
            <Button
              variant="contained"
              onClick={handleSave} 
              disabled={saving} 
              startIcon={saving ? <CircularProgress size={16} color="inherit" /> : null}
            >
              {saving ? "Mentés…" : "Mentés"} 
            </Button>
          </Box>

        </Stack>
      </Stack>
    </Container>
  );
}
