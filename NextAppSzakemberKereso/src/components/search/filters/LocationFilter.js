'use client';

import useSWR from 'swr';
import { Accordion, AccordionDetails, AccordionSummary, Alert, Autocomplete, CircularProgress, Stack, TextField, Typography } from '@mui/material';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import { genericFetcher } from '@/utils/fetcher';

export default function LocationFilter({ countyName, settlementName, onChange }) {
  const { data: counties = [], isLoading: countiesLoading, error: countiesError } = useSWR(
    '/api/Settlements/counties',
    genericFetcher
  );

  const { data: settlements = [], isLoading: settlementsLoading } = useSWR(
    countyName ? `/api/Settlements/byCounty?countyName=${encodeURIComponent(countyName)}` : null,
    genericFetcher
  );

  return (
    <Accordion defaultExpanded disableGutters elevation={0} sx={{ border: '1px solid', borderColor: 'divider', borderRadius: 1 }}>
      <AccordionSummary expandIcon={<ExpandMoreIcon />}>
        <Typography variant="body2">Helyszín</Typography>
      </AccordionSummary>
      <AccordionDetails>
        <Stack spacing={2}>
          {countiesError && <Alert severity="error">A helységek betöltése sikertelen.</Alert>}
          <Autocomplete
            options={counties}
            value={countyName}
            loading={countiesLoading}
            onChange={(_, val) => {
              onChange('countyName', val);
              onChange('settlementName', null);
            }}
            size="small"
            renderInput={(params) => (
              <TextField
                {...params}
                label="Vármegye"
                slotProps={{
                  input: {
                    ...params.InputProps,
                    endAdornment: (
                      <>
                        {countiesLoading && <CircularProgress size={16} />}
                        {params.InputProps.endAdornment}
                      </>
                    ),
                  },
                }}
              />
            )}
          />
          <Autocomplete
            options={settlements}
            value={settlementName}
            loading={settlementsLoading}
            disabled={!countyName}
            onChange={(_, val) => onChange('settlementName', val)}
            size="small"
            renderInput={(params) => (
              <TextField
                {...params}
                label="Település"
                slotProps={{
                  input: {
                    ...params.InputProps,
                    endAdornment: (
                      <>
                        {settlementsLoading && <CircularProgress size={16} />}
                        {params.InputProps.endAdornment}
                      </>
                    ),
                  },
                }}
              />
            )}
          />
        </Stack>
      </AccordionDetails>
    </Accordion>
  );
}
