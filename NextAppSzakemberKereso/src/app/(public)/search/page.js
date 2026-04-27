'use client';

import { Suspense, useState } from 'react';
import { useRouter, useSearchParams } from 'next/navigation';

import { Box, Button, Container, Drawer, useMediaQuery, useTheme } from '@mui/material';
import FilterListIcon from '@mui/icons-material/FilterList';
import FilterComponent from '@/components/search/FilterComponent';
import ExpertResults from '@/components/search/ExpertResults';

import { parseFilterParams, buildFilterQuery } from '@/utils/expertSearch';

function SearchPageContent() {
  const router = useRouter()
  const searchParams = useSearchParams()

  const isSmallScreen = useMediaQuery(useTheme().breakpoints.down('md'));
  const [mobileFilterOpen, setMobileFilterOpen] = useState(false)

  const parsedFilterParams = parseFilterParams(searchParams)

  function handleSearch(updatedFormState) {
    router.push(`/search?${buildFilterQuery(updatedFormState)}`);
    setMobileFilterOpen(false);
  }

  return (
    <Container sx={(theme) => ({ minHeight: '100vh', py: 3, mt: theme.mixins.toolbar.height })}>
      {isSmallScreen && (
        <Button
          variant="outlined"
          startIcon={<FilterListIcon />}
          onClick={() => setMobileFilterOpen(true)}
          sx={{ mb: 2, width: '100%' }}
        >
          Szűrők
        </Button>
      )}

      <Box sx={{
        display: 'grid',
        gridTemplateColumns: { xs: '1fr', md: '350px 5fr' },
        gap: 4,
        alignItems: 'start',
      }}>
        {isSmallScreen ? (
          <Drawer anchor="top" open={mobileFilterOpen} onClose={() => setMobileFilterOpen(false)}>
            <FilterComponent
              key={buildFilterQuery(parsedFilterParams)}
              formState={parsedFilterParams}
              onSearch={handleSearch}
            />
          </Drawer>
        ) : (
          <Box sx={{ position: 'sticky', top: (theme) => theme.mixins.toolbar.minHeight + 16, minWidth: 0 }}>
            <FilterComponent
              key={buildFilterQuery(parsedFilterParams)}
              formState={parsedFilterParams}
              onSearch={handleSearch}
            />
          </Box>
        )}

        <ExpertResults filterParams={parsedFilterParams} />
      </Box>
    </Container>
  );
}

export default function SearchPage() {
  return (
    <Suspense>
      <SearchPageContent />
    </Suspense>
  );
}
