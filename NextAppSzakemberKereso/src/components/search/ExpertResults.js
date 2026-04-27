'use client';

import useSWR from 'swr';
import {useTheme,useMediaQuery, Divider} from '@mui/material';

import { Alert, Box, CircularProgress, IconButton, Stack, Tooltip, Typography,} from '@mui/material';
import SortIcon from '@mui/icons-material/Sort';

import ExpertCard from './ExpertCard';
import { buildFilterQuery } from '@/utils/expertSearch';
import { genericFetcher } from '@/utils/fetcher';

export default function ExpertResults({ filterParams }) {
  const isSmallScreen = useMediaQuery(useTheme().breakpoints.down('md'));

  const { data: experts, isLoading, error } = useSWR(
    `/api/Experts?${buildFilterQuery(filterParams)}`,
    genericFetcher
  );

  if (isLoading) {
    return ( 
      <Box sx={{ display: 'flex', justifyContent: 'center', pt: 10 }}>
        <CircularProgress />
      </Box>
    );
  }

  if (error) {
    return <Alert severity="error">Valami hiba történt</Alert>;
  }

  if (!experts?.length) {
    return (
      <Typography color="text.secondary" sx={{ pt: 4, textAlign: 'center' }}>
        Nem található szakember a megadott feltételekkel. 
      </Typography>
    );
  }

  return (
    <Box sx={{ minHeight: '50vh' }}>
      <Divider sx={{ mb: 2}}>
        <Typography variant="body2" color="text.secondary">
          {experts.length} találat 
        </Typography>
      </Divider>

      <Stack spacing={2}>
        {experts.map((expert) => (
          <ExpertCard key={expert.userId} expert={expert} filterParams={filterParams} isMobile={isSmallScreen}/>
        ))}
      </Stack>
    </Box>
  );
}
