'use client';
import { useState } from 'react';

import OccupationFilter from './filters/OccupationFilter';
import PricingFilter from './filters/PricingFilter';
import LocationFilter from './filters/LocationFilter';
import SearchIcon from '@mui/icons-material/Search';
import { Button, Paper, Stack, Typography } from '@mui/material';

import { buildFilterQuery } from '@/utils/expertSearch';

const EMPTY_FILTERS = {
  occupationId: null,
  minFixedPrice: null,
  maxFixedPrice: null,
  allowUnitBased: true,
  unitName: null,
  countyName: null,
  settlementName: null,
};

export default function FilterComponent({ formState, onSearch }) {
  const [formData, setFormData] = useState({...formState});

  function set(field, value) {
    if (field === 'occupationId') {
      setFormData((prev) => ({ ...prev, occupationId: value, minFixedPrice: null, maxFixedPrice: null, unitName: null, allowUnitBased: true }));
    } else {
      setFormData((prev) => ({ ...prev, [field]: value }));
    }
  }

  const formDataEqualsParams = buildFilterQuery(formData) == buildFilterQuery(formState);
  const hasActiveFilters = buildFilterQuery(formState) !== '';

  function handleClear() {
    setFormData({...EMPTY_FILTERS});
    onSearch(EMPTY_FILTERS);
  }

  return (
    <Paper sx={{ p: 3 }}>
      <Typography variant="h6" mb={2}>
        Szűrők
      </Typography>

      <Stack spacing={2}>
        <OccupationFilter
          selectedOccupationId={formData.occupationId}
          onChange={set}
        />

        <PricingFilter
          formState={formState}
          occupationId={formState.occupationId}
          onChange={set}
        />

        <LocationFilter
          countyName={formData.countyName}
          settlementName={formData.settlementName}
          onChange={set}
        />

        <Button
          variant="contained"
          fullWidth
          startIcon={<SearchIcon />}
          disabled={formDataEqualsParams}
          onClick={() => onSearch(formData)}
        >
          Keresés
        </Button>

        <Button
          variant="outlined"
          fullWidth
          disabled={!hasActiveFilters}
          onClick={handleClear}
        >
          Filterek törlése
        </Button>
      </Stack>
    </Paper>
  );
}
