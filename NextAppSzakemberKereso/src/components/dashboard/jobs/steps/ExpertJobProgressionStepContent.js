'use client';

import { useState } from 'react';
import Button from '@mui/material/Button';
import Divider from '@mui/material/Divider';
import Stack from '@mui/material/Stack';
import CircularProgress from '@mui/material/CircularProgress';
import Typography from '@mui/material/Typography';
import Alert from '@mui/material/Alert';


import ConfirmCancelDialog from '@/components/dashboard/jobs/ConfirmCancelDialog';
import AppointmentList from '@/components/dashboard/jobs/AppointmentList';

export default function JobProgressionStepContent({ job, refreshJob, disabled = false }) {
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState(null);
  const [confirmOpen, setConfirmOpen] = useState(false);

  async function handleComplete() {
    setSubmitting(true);
    setError(null);
    try {
      const res = await fetch(`/api/Jobs/${job.id}/complete`, {
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

  async function handleCancel() {
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

      <Stack spacing={0.5}>
        <Typography variant="caption" color="text.secondary">Időpontok</Typography>
        <AppointmentList appointments={job.offeredAppointments} />
      </Stack>

      {!disabled ? (
        <>
          <Divider />
          <Stack direction="row" spacing={1}>
            <Button
              variant="contained"
              size="small"
              disabled={submitting}
              startIcon={submitting ? <CircularProgress size={16} color="inherit" /> : null}
              onClick={handleComplete}
            >
              Munka befejezése
            </Button>
            <Button
              variant="outlined"
              color="error"
              size="small"
              onClick={() => setConfirmOpen(true)}
              disabled={submitting}
            >
              Munkamegrendelés lemondása
            </Button>
          </Stack>

          <ConfirmCancelDialog
            open={confirmOpen}
            onClose={() => setConfirmOpen(false)}
            onConfirm={handleCancel}
          />
        </>
      ) : (
        null
      )}
    </Stack>
  );
}
