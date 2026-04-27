'use client';

import { useState } from 'react';
import FormControlLabel from '@mui/material/FormControlLabel';
import Switch from '@mui/material/Switch';
import CircularProgress from '@mui/material/CircularProgress';
import Alert from '@mui/material/Alert';
import Button from '@mui/material/Button';
import TextField from '@mui/material/TextField';
import Divider from '@mui/material/Divider';
import Typography from '@mui/material/Typography';
import InputAdornment from '@mui/material/InputAdornment';
import Stack from '@mui/material/Stack';
import Chip from '@mui/material/Chip'; 

import { PricingType, formatPricing } from '@/utils/pricing';
import ConfirmCancelDialog from '../ConfirmCancelDialog';
import DateTimeSpecifier from '../DateTimeSpecifier';

export default function OfferStepContent({ job, refreshJob, disabled = false }) {
  const [fixedPrice, setFixedPrice] = useState(''); //could be ref
  const [isUnitBased, setIsUnitBased] = useState(false); //needed for conditional rendering 
  const [unitPrice, setUnitPrice] = useState(''); //could be ref
  const [unitName, setUnitName] = useState(''); //could be ref

  const [intervals, setIntervals] = useState([]);

  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState(null);
  const [confirmOpen, setConfirmOpen] = useState(false);

  if (disabled) {
    if (!job.pricing) { //if the cancelled without a set price
      return (
        <Typography variant="body2" color="text.secondary" sx={{ pb: 1 }}>
          Ajánlat nem lett elküldve.
        </Typography>
      );
    }
    return (
      <Stack spacing={2} sx={{ pb: 1 }}>
        <Stack spacing={0.25}>
          <Typography variant="caption" color="text.secondary">Ajánlott ár</Typography>
          <Typography variant="body2">{formatPricing(job.pricing)}</Typography>
        </Stack>
        <Stack spacing={0.5}>
          <Typography variant="caption" color="text.secondary">Javasolt időpontok</Typography>
          <DateTimeSpecifier value={job.offeredAppointments} disabled />
        </Stack>
      </Stack>
    );
  }

  function isFormValid() {
    if(intervals.length === 0) return false;
    if(!intervals.every((t) => t.date && t.startTime && t.endTime)) return false;
    if (!fixedPrice) return false;
    if (isUnitBased && (!unitPrice || !unitName.trim())) return false;
    return true;
  }

  async function handleSendOffer() {
    setSubmitting(true);
    setError(null);
    try {
      const pricing = isUnitBased
        ? { pricingType: PricingType.FixedAndUnitBased, fixedPrice: parseFloat(fixedPrice), unitPrice: parseFloat(unitPrice), unitName: unitName.trim() }
        : { pricingType: PricingType.Fixed, fixedPrice: parseFloat(fixedPrice) };

      const res = await fetch(`/api/Jobs/${job.id}/offer`, {
        method: 'POST',
        credentials: 'include',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ pricing: pricing, offeredTimeIntervals: intervals }),
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

      <TextField
        label="Alapár"
        type="number"
        value={fixedPrice}
        onChange={(e) => setFixedPrice(e.target.value)}
        size="small"
        required
        slotProps={{ input: { endAdornment: <InputAdornment position="end">Ft</InputAdornment> } }}
      />

      <FormControlLabel
        label={<Typography variant="body2">Egységalapú árazás</Typography>}
        control={
          <Switch
            checked={isUnitBased}
            onChange={(e) => setIsUnitBased(e.target.checked)}
          />
        }
      />

      {isUnitBased && (
        <Stack direction={{ xs: 'column', sm: 'row' }} spacing={2}>
          <TextField
            label="Egységár"
            type="number"
            value={unitPrice}
            onChange={(e) => setUnitPrice(e.target.value)}
            size="small"
            required
            fullWidth
            slotProps={{ input: { endAdornment: <InputAdornment position="end">Ft</InputAdornment> } }}
          />
          <TextField
            label="Egység neve"
            placeholder="pl. óra, nap"
            value={unitName}
            onChange={(e) => setUnitName(e.target.value)}
            size="small"
            required
            fullWidth
            slotProps={{ htmlInput: { maxLength: 64 } }}
          />
        </Stack>
      )}

      <Stack spacing={0.5}>
        <Typography variant="caption" color="text.secondary">Javasolt időpontok</Typography>
        <DateTimeSpecifier value={intervals} onChange={setIntervals} />
      </Stack>

      <Divider />

      <Stack direction="row" spacing={1}>
        <Button
          variant="contained"
          size="small"
          disabled={!isFormValid() || submitting}
          startIcon={submitting ? <CircularProgress size={16} color="inherit" /> : null}
          onClick={handleSendOffer}
        >
          Ajánlat küldése
        </Button>
        <Button
          variant="outlined"
          color="error"
          size="small"
          onClick={() => setConfirmOpen(true)}
          disabled={submitting}
        >
          Kérés elutasítása
        </Button>
      </Stack>

      <ConfirmCancelDialog
        open={confirmOpen}
        onClose={() => setConfirmOpen(false)}
        onConfirm={handleReject}
        message="Biztosan el szeretnéd utasítani ezt a munkakérést?"
      />
    </Stack>
  );
}
