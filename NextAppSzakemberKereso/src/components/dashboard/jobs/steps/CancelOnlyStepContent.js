'use client';

import { useState } from 'react';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import Stack from '@mui/material/Stack';
import Alert from '@mui/material/Alert';

import ConfirmCancelDialog from '../ConfirmCancelDialog';

export default function CancelOnlyStepContent({
  job,
  refreshJob,
  disabled = false,
  buttonLabel,
  confirmMessage,
  info,
}) {
  const [confirmOpen, setConfirmOpen] = useState(false);
  const [error, setError] = useState(null);

  if (disabled) return null;

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
      {info && <Typography variant="body2" color="text.secondary">{info}</Typography>}
      <Button
        variant="outlined"
        color="error"
        size="small"
        sx={{ alignSelf: 'flex-start' }}
        onClick={() => setConfirmOpen(true)}
      >
        {buttonLabel}
      </Button>
      <ConfirmCancelDialog
        open={confirmOpen}
        onClose={() => setConfirmOpen(false)}
        onConfirm={handleCancel}
        message={confirmMessage}
      />
    </Stack>
  );
}
