'use client';

import { useState } from 'react';

import Button from '@mui/material/Button';
import Divider from '@mui/material/Divider';
import Stack from '@mui/material/Stack';
import CircularProgress from '@mui/material/CircularProgress';
import Typography from '@mui/material/Typography';
import Alert from '@mui/material/Alert';

import { formatPricing } from '@/utils/pricing';
import ConfirmCancelDialog from '../ConfirmCancelDialog';
import DateTimeSpecifier from '../DateTimeSpecifier';

export default function UserOfferedStepContent({ job, refreshJob, disabled = false }) {
  const [submitting, setSubmitting] = useState(false);
  const [confirmOpen, setConfirmOpen] = useState(false);
  const [error, setError] = useState(null);

  const content = (
    <Stack spacing={2} sx={{ pb: 1 }}>
      {error && <Alert severity="error">{error}</Alert>}
      <Stack spacing={0.25}>
        <Typography variant="caption" color="text.secondary">Ajánlott ár</Typography>
        <Typography variant="body2">{formatPricing(job.pricing)}</Typography>
      </Stack>
      <Stack spacing={0.5}>
        <Typography variant="caption" color="text.secondary">Javasolt időpontok</Typography>
        <DateTimeSpecifier value={job.offeredAppointments ?? []} disabled />
      </Stack>
    </Stack>
  );

  if (disabled) return content;

  async function handleAccept() {
    setSubmitting(true);
    setError(null);
    try {
      const res = await fetch(`/api/Jobs/${job.id}/accept`, {
        method: 'POST',
        credentials: 'include',
      });
      if (!res.ok) {
        const body = await res.json().catch(() => null);
        throw new Error(body?.title ?? `Hiba történt (${res.status})`);
      }
      await refreshJob();
    } catch (e) {
      setError(e.message);
    } finally {
      setSubmitting(false);
    }
  }

  async function handleReject() {
    setError(null);
    try {
      const res = await fetch(`/api/Jobs/${job.id}/cancel`, {
        method: 'POST',
        credentials: 'include',
      });
      if (!res.ok) throw new Error(`Hiba történt (${res.status})`);
      await refreshJob();
    } catch (e) {
      setError(e.message);
    }
  }

  return (
    <Stack spacing={2} sx={{ pb: 1 }}>
      {error && <Alert severity="error">{error}</Alert>}
      <Stack spacing={0.25}>
        <Typography variant="caption" color="text.secondary">Ajánlott ár</Typography>
        <Typography variant="body2">{formatPricing(job.pricing)}</Typography>
      </Stack>
      <Stack spacing={0.5}>
        <Typography variant="caption" color="text.secondary">Javasolt időpontok</Typography>
        <DateTimeSpecifier value={job.offeredAppointments ?? []} disabled />
      </Stack>

      <Divider />

      <Stack direction="row" spacing={1}>
        <Button
          variant="contained"
          size="small"
          disabled={submitting}
          startIcon={submitting ? <CircularProgress size={16} color="inherit" /> : null}
          onClick={handleAccept}
        >
          Ajánlat elfogadása
        </Button>
        <Button
          variant="outlined"
          color="error"
          size="small"
          disabled={submitting}
          onClick={() => setConfirmOpen(true)}
        >
          Ajánlat visszautasítása
        </Button>
      </Stack>

      <ConfirmCancelDialog
        open={confirmOpen}
        onClose={() => setConfirmOpen(false)}
        onConfirm={handleReject}
        message="Biztosan vissza szeretnéd utasítani az ajánlatot?"
      />
    </Stack>
  );
}
