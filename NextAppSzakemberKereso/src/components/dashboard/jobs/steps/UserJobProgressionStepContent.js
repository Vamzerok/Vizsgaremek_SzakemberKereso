'use client';

import { useState } from 'react';
import Divider from '@mui/material/Divider';
import Stack from '@mui/material/Stack';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import Alert from '@mui/material/Alert';


import ConfirmCancelDialog from '../ConfirmCancelDialog';
import AppointmentList from '../AppointmentList';

export default function UserJobProgressionStepContent({ job, refreshJob, disabled = false }) {
  const [error, setError] = useState(null);
  const [confirmOpen, setConfirmOpen] = useState(false);

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

      {!disabled && (
        <>
          <Divider />
          <Stack direction="row" spacing={1}>
            <Button
              variant="outlined"
              color="error"
              size="small"
              onClick={() => setConfirmOpen(true)}
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
      )}
    </Stack>
  );
}
