'use client';

import { useState, useEffect } from 'react';
import useSWR from 'swr';

import Autocomplete from '@mui/material/Autocomplete';
import CircularProgress from '@mui/material/CircularProgress';
import FormControl from '@mui/material/FormControl';
import InputLabel from '@mui/material/InputLabel';
import MenuItem from '@mui/material/MenuItem';
import Select from '@mui/material/Select';
import Stack from '@mui/material/Stack';
import TextField from '@mui/material/TextField';

import { genericFetcher } from '@/utils/fetcher';

export default function ExpertServiceSelector({ initialExpertId, initialServiceId, onServiceChange }) {
  const [selectedExpertId, setSelectedExpertId] = useState(null); 
  const [selectedServiceId, setSelectedServiceId] = useState(null);

  const { data: experts, isLoading: expertsLoading } = useSWR('/api/Experts/truncated', genericFetcher); 
  const { data: services, isLoading: servicesLoading } = useSWR(
    selectedExpertId ? `/api/Services/truncated?expertId=${selectedExpertId}` : null,
    genericFetcher
  );

  useEffect(() => {
    if (!initialExpertId || !experts) return;
    handleSetSelectedExpertId(initialExpertId);
  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [experts]);

  useEffect(() => {
    if (!initialServiceId || !services) return;
    handleSetSelectedServiceId(initialServiceId); 
  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [services]);

  function handleSetSelectedExpertId(id) {
    if (id !== null && !experts?.find((e) => e.id === id)) return; 
    setSelectedExpertId(id); 
    handleSetSelectedServiceId(null);
  }

  function handleSetSelectedServiceId(id) {
    const service = id ? (services?.find((s) => s.id === id) ?? null) : null;
    if (id !== null && !service) return;
    setSelectedServiceId(id);
    onServiceChange(service);
  }

  const selectedExpert = experts?.find((e) => e.id === selectedExpertId) ?? null;
  const selectedService = services?.find((s) => s.id === selectedServiceId) ?? null;

  return (
    <Stack spacing={2}>
      <Autocomplete
        options={experts ?? []}
        getOptionLabel={(e) => e.name}
        value={selectedExpert}
        onChange={(_, expert) => handleSetSelectedExpertId(expert?.id ?? null)}
        isOptionEqualToValue={(option, value) => option.id === value.id}
        loading={expertsLoading}
        renderInput={(params) => (
          <TextField
            {...params} 
            label="Szakember" 
            size="small" 
            required
            slotProps={{ 
              input: { 
                ...params.InputProps,
                endAdornment: (
                  <>
                    {expertsLoading && <CircularProgress size={16} />}
                    {params.InputProps.endAdornment}
                  </>
                ), 
              },
            }}
          />
        )}
      />

      <FormControl size="small" required disabled={!selectedExpertId || servicesLoading}>
        <InputLabel>
          {servicesLoading ? 'Betöltés…' : 'Szolgáltatás'} 
        </InputLabel>
        <Select
          value={selectedService?.id ?? ''}
          label={servicesLoading ? 'Betöltés…' : 'Szolgáltatás'}
          onChange={(e) => handleSetSelectedServiceId(e.target.value)}
        >
          {(services ?? []).map((s) => ( 
            <MenuItem key={s.id} value={s.id}>{s.name}</MenuItem>
          ))}
        </Select>
      </FormControl>
    </Stack>
  );
}