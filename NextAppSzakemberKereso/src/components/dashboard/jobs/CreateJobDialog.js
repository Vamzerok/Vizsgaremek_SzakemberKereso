'use client';

import { useRef, useState } from 'react';

import Alert from '@mui/material/Alert';
import Button from '@mui/material/Button';
import CircularProgress from '@mui/material/CircularProgress';
import Dialog from '@mui/material/Dialog';
import DialogContent from '@mui/material/DialogContent';
import DialogTitle from '@mui/material/DialogTitle';
import Divider from '@mui/material/Divider';
import IconButton from '@mui/material/IconButton';
import Stack from '@mui/material/Stack';
import TextField from '@mui/material/TextField';
import Typography from '@mui/material/Typography';
import CloseIcon from '@mui/icons-material/Close';

import InputResidentialAddress from '@/components/InputResidentialAddress';
import DateTimeSpecifier from './DateTimeSpecifier';
import ExpertServiceSelector from './ExpertServiceSelector';

export default function CreateJobDialog({ onClose, initialExpertId, initialServiceId }) {
  const selectedServiceRef = useRef(null);
  const titleRef = useRef(null);
  const descriptionRef = useRef(null);
  const locationRef = useRef(null);
  const intervalsRef = useRef(null);

  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState(null);

  async function handleSubmit() {
    const service = selectedServiceRef.current;
    const title = titleRef.current?.value?.trim() ?? '';
    const description = descriptionRef.current?.value ?? '';
    const location = locationRef.current?.getValue();
    const availableTimeIntervals = intervalsRef.current?.getValue() ?? [];

    if (
      !service ||
      !title ||
      !location?.settlement?.name ||
      !location?.settlement?.postalCode ||
      !location?.streetAddress ||
      !availableTimeIntervals.length ||
      !availableTimeIntervals.every((t) => t.date && t.startTime && t.endTime)
    ) {
      setError('Kérjük töltsd ki az összes kötelező mezőt.');
      return;
    }

    setSubmitting(true);
    setError(null);
    try {
      const res = await fetch('/api/Jobs', {
        method: 'POST',
        credentials: 'include',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          serviceId: service.id,
          title,
          description,
          location,
          availableTimeIntervals,
        }),
      });
      if (!res.ok) {
        const body = await res.json().catch(() => {});
        const message = body?.error ?? `Hiba történt (${res.status})`;
        throw new Error(message);
      }
      onClose();
    } catch (e) {
      setError(e.message);
    } finally {
      setSubmitting(false);
    }
  }

  return (
    <Dialog open onClose={onClose} fullWidth maxWidth="md" scroll="paper">
      <DialogTitle sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
        Új munkakérés
        <IconButton size="small" onClick={onClose}>
          <CloseIcon fontSize="small" />
        </IconButton>
      </DialogTitle>

      <DialogContent dividers>
        <Stack spacing={2} sx={{ pb: 1 }}>
          {error && <Alert severity="error">{error}</Alert>}

          <ExpertServiceSelector
            initialExpertId={initialExpertId}
            initialServiceId={initialServiceId}
            onServiceChange={(service) => { selectedServiceRef.current = service; }}
          />

          <Divider />

          <TextField
            label="Cím"
            size="small"
            required
            inputRef={titleRef}
            slotProps={{
              htmlInput: {
                maxLength: 100
              }
            }}
          />

          <TextField
            label="Leírás"
            size="small"
            multiline
            minRows={3}
            inputRef={descriptionRef}
          />

          <InputResidentialAddress ref={locationRef} />

          <Stack spacing={0.5}>
            <Typography variant="caption" color="text.secondary">Javasolt időpontok</Typography>
            <DateTimeSpecifier ref={intervalsRef} />
          </Stack>

          <Button
            variant="contained"
            disabled={submitting}
            startIcon={submitting ? <CircularProgress size={16} color="inherit" /> : null}
            onClick={handleSubmit}
            sx={{ alignSelf: 'flex-start' }}
          >
            Munkakérés beküldése
          </Button>
        </Stack>
      </DialogContent>
    </Dialog>
  );
}
