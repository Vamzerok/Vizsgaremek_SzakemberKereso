'use client';

import { useState } from 'react';
import Alert from '@mui/material/Alert';
import Button from '@mui/material/Button';
import CircularProgress from '@mui/material/CircularProgress';
import Rating from '@mui/material/Rating';
import Stack from '@mui/material/Stack';
import Typography from '@mui/material/Typography';

export default function UserCompletedStepContent({ job, refreshJob }) {
  const [rating, setRating] = useState(null);
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState(null);

  if (job.rating != null) {
    return (
      <Stack spacing={0.5} sx={{ pb: 1 }}>
        <Typography variant="caption" color="text.secondary">Értékelés</Typography>
        <Rating value={job.rating} precision={0.5} readOnly />
      </Stack>
    );
  }

  async function handleSubmit() {
    if (!rating) return;
    setSubmitting(true);
    setError(null);
    try {
      const res = await fetch(`/api/Jobs/${job.id}/rate?rating=${rating}`, {
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

  return (
    <Stack spacing={2} sx={{ pb: 1 }}>
      {error && <Alert severity="error">{error}</Alert>}
      <Stack spacing={0.5}>
        <Typography variant="caption" color="text.secondary">Értékeld a munkát</Typography>
        <Rating value={rating} onChange={(_, v) => setRating(v)} />
      </Stack>
      <Button
        variant="contained"
        size="small"
        sx={{ alignSelf: 'flex-start' }}
        disabled={!rating || submitting}
        startIcon={submitting ? <CircularProgress size={16} color="inherit" /> : null}
        onClick={handleSubmit}
      >
        Értékelés küldése
      </Button>
    </Stack>
  );
}
