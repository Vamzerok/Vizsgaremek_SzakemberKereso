'use client';

import useSWR from 'swr';
import {
  Accordion, AccordionDetails, AccordionSummary, Alert, Autocomplete,
  FormControlLabel, Skeleton, Slider, Stack, Switch, TextField, Typography,
} from '@mui/material';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import { genericFetcher } from '@/utils/fetcher';

export default function PricingFilter({ formState, occupationId, onChange }) {
  const swrKey = occupationId
    ? `/api/Services/pricingOptions?occupationId=${occupationId}`
    : '/api/Services/pricingOptions';

  const { data: pricingOptions, isLoading, error } = useSWR(swrKey, genericFetcher);

  function handleSliderCommitted(_, [min, max]) {
    const isFullRange = min === pricingOptions.minFixedPrice && max === pricingOptions.maxFixedPrice;
    onChange('minFixedPrice', isFullRange ? null : min);
    onChange('maxFixedPrice', isFullRange ? null : max);
  }

  return (
    <Accordion defaultExpanded disableGutters elevation={0} sx={{ border: '1px solid', borderColor: 'divider', borderRadius: 1 }}>
      <AccordionSummary expandIcon={<ExpandMoreIcon />}>
        <Typography variant="body2">Árazás</Typography>
      </AccordionSummary>
      <AccordionDetails>
        <Stack spacing={3}>
          {error && <Alert severity="error">Az árazási adatok betöltése sikertelen.</Alert>}

          <Stack spacing={1}>
            <Typography variant="body2" color="text.secondary">Alapár (Ft)</Typography>
            {isLoading || !pricingOptions ? (
              <Skeleton variant="rounded" height={28} />
            ) : (
              <>
                <Slider
                  key={swrKey}
                  defaultValue={[
                    formState.minFixedPrice ?? pricingOptions.minFixedPrice,
                    formState.maxFixedPrice ?? pricingOptions.maxFixedPrice,
                  ]}
                  min={pricingOptions.minFixedPrice}
                  max={pricingOptions.maxFixedPrice}
                  onChangeCommitted={handleSliderCommitted}
                  valueLabelDisplay="auto"
                  valueLabelFormat={(v) => `${v.toLocaleString('hu-HU')} Ft`}
                  disableSwap
                />
                <Stack direction="row" justifyContent="space-between">
                  <Typography variant="caption" color="text.secondary">
                    {pricingOptions.minFixedPrice.toLocaleString('hu-HU')} Ft
                  </Typography>
                  <Typography variant="caption" color="text.secondary">
                    {pricingOptions.maxFixedPrice.toLocaleString('hu-HU')} Ft
                  </Typography>
                </Stack>
              </>
            )}
          </Stack>

          <Stack spacing={1}>
            <FormControlLabel
              label={<Typography variant="body2">Egységalapú árak engedélyezése</Typography>}
              control={
                <Switch
                  defaultChecked={formState.allowUnitBased}
                  onChange={(e) => {
                    onChange('allowUnitBased', e.target.checked);
                    if (!e.target.checked) onChange('unitName', null);
                  }}
                />
              }
            />
            {isLoading || !pricingOptions ? (
              <Skeleton variant="rounded" height={40} />
            ) : (
              <Autocomplete
                key={swrKey}
                options={pricingOptions.unitNames}
                defaultValue={formState.unitName}
                onChange={(_, val) => onChange('unitName', val)}
                disabled={!formState.allowUnitBased}
                size="small"
                renderInput={(params) => (
                  <TextField {...params} label="Egység" placeholder="pl. óra, nap, m²" />
                )}
              />
            )}
          </Stack>
        </Stack>
      </AccordionDetails>
    </Accordion>
  );
}
