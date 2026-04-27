'use client';

import { useState, useEffect } from 'react';
import { Autocomplete, TextField } from '@mui/material';

export default function OccupationFilter({ selectedOccupationId, onChange }) {
  const [occupations, setOccupations] = useState([]);

  useEffect(() => {
    fetch('/api/Occupations')
      .then((res) => res.json())
      .then(setOccupations)
      .catch(() => {});
  }, []);

  const selected = occupations.find((o) => o.id === selectedOccupationId) ?? null;

  return (
    <Autocomplete
      options={occupations}
      getOptionLabel={(o) => o.name} 
      value={selected} 
      onChange={(_, val) => onChange('occupationId', val?.id ?? null)} 
      size="small"
      renderInput={(params) => ( 
        <TextField {...params} label="Foglalkozás" />
      )}
    />
  );
}
