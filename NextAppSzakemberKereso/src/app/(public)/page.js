"use client";

import { useState, useEffect } from 'react';
import { useRouter } from 'next/navigation';

import Autocomplete from '@mui/material/Autocomplete';
import Box from '@mui/material/Box';
import Button from '@mui/material/Button';
import TextField from '@mui/material/TextField';
import Typography from '@mui/material/Typography';
import SearchIcon from '@mui/icons-material/Search';

export default function Home() {
  const [occupations, setOccupations] = useState([]);
  const [selected, setSelected] = useState(null);
  const router = useRouter();

  useEffect(() => {
    fetch('/api/Occupations')
      .then((res) => res.json())
      .then(setOccupations)
      .catch(() => {});
  }, []);

  function handleSearch() {
    const url = selected ? `/search?occupationId=${selected.id}` : '/search';
    router.push(url);
  }

  return (
    <Box
      sx={{
        display: 'flex',
        flexDirection: 'column',
        justifyContent: 'center',
        alignItems: 'center',
        height: (theme) => `calc(100vh - ${theme.mixins.toolbar.minHeight + 1}px)`,
        background: 'linear-gradient(150deg, #0d47a1 0%, #1565c0 40%, #1976d2 70%, #42a5f5 100%)',
        textAlign: 'center',
        px: 2,
      }}
    >
      <Typography
        variant="h1"
        fontWeight={700}
        color="white"
        sx={{ fontSize: { xs: '2rem', sm: '3rem', md: '3.5rem' }, mb: 2 }}
      >
        Találd meg a megfelelő szakembert
      </Typography>

      <Typography
        variant="h6"
        fontWeight={400}
        sx={{ color: 'rgba(255,255,255,0.8)', maxWidth: 520, mb: 6 }}
      >
        Böngéssz a szakemberek között, majd kérd fel őket egyszerűen, egy helyen.
      </Typography>

      <Box
        sx={{
          display: 'flex',
          flexDirection: { xs: 'column', sm: 'row' },
          alignItems: 'stretch',
          gap: 1.5,
          width: '100%',
          maxWidth: 540,
          bgcolor: 'white',
          borderRadius: 3,
          boxShadow: '0 12px 40px rgba(0,0,0,0.22)',
          p: { xs: 2, sm: 3 },
        }}
      >
        <Autocomplete
          options={occupations}
          getOptionLabel={(o) => o.name}
          value={selected}
          onChange={(_, val) => setSelected(val)}
          sx={{ flexGrow: 1 }}
          renderInput={(params) => (
            <TextField {...params} label="Foglalkozás kiválasztása" size="small" />
          )}
        />
        <Button
          variant="contained"
          size="large"
          startIcon={<SearchIcon />}
          onClick={handleSearch}
          sx={{ flexShrink: 0, px: 3, borderRadius: 2 }}
        >
          Keresés indítása
        </Button>
      </Box>
    </Box>
  );
}
