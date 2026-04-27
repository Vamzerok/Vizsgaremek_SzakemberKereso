'use client';

import { useState } from 'react';
import { useRouter, useSearchParams } from 'next/navigation';
import { useMediaQuery } from '@mui/material';
import useSWR from 'swr';

import Container from '@mui/material/Container';
import Stack from '@mui/material/Stack';
import CircularProgress from '@mui/material/CircularProgress';
import SpeedDial from '@mui/material/SpeedDial';
import SpeedDialIcon from '@mui/material/SpeedDialIcon';
import Typography from '@mui/material/Typography';
import Alert from '@mui/material/Alert';

import ExpertJobCard from '@/components/dashboard/jobs/ExpertJobCard';
import UserJobCard from '@/components/dashboard/jobs/UserJobCard';
import JobDialog from '@/components/dashboard/jobs/JobDialog';
import CreateJobDialog from '@/components/dashboard/jobs/CreateJobDialog';
import { EXPERT_STEPS, USER_STEPS } from '@/utils/jobSteps';
import { genericFetcher } from '@/utils/fetcher';
import { isUserAnExpert } from '@/utils/user';
import useUser from '@/hooks/useUser';

export default function JobsPage() {
  const router = useRouter();
  const searchParams = useSearchParams();
  const isSmallScreen = useMediaQuery((theme) => theme.breakpoints.down('sm'));
  const [createDialogOpen, setCreateDialogOpen] = useState(false);

  const { user, isLoading: userLoading } = useUser();
  const isExpert = isUserAnExpert(user);

  const expertId = searchParams.get('expertId') ? Number(searchParams.get('expertId')) : null;
  const serviceId = expertId && searchParams.get('serviceId') ? Number(searchParams.get('serviceId')) : null;

  const showCreate = !isExpert && (createDialogOpen || !!expertId);
  const jobId = showCreate ? null : searchParams.get('jobId') ? Number(searchParams.get('jobId')) : null;

  const { data: jobs, isLoading: jobsLoading, error, mutate } = useSWR(
    user ? `/api/${isExpert ? 'Experts' : 'Users'}/me/jobs` : null,
    genericFetcher
  );

  function handleDialogClose() {
    router.push('/dashboard/jobs');
    mutate();
  }

  function handleCreateClose() {
    setCreateDialogOpen(false);
    router.push('/dashboard/jobs');
    mutate();
  }

  if (userLoading || jobsLoading) {
    return (
      <Container sx={{ display: 'flex', justifyContent: 'center', pt: 8 }}>
        <CircularProgress />
      </Container>
    );
  }

  if (error) {
    return (
      <Container sx={{ p: 3 }}>
        <Alert severity="error">Hiba történt a munkák betöltése közben</Alert>
      </Container>
    );
  }

  const steps = isExpert ? EXPERT_STEPS : USER_STEPS;
  const JobCard = isExpert ? ExpertJobCard : UserJobCard;

  return (
    <Container sx={isSmallScreen ? { px: 0, py: 2 } : { px: 3, py: 3 }}>
      {!jobs?.length ? (
        <Typography color="text.secondary" textAlign="center" sx={{ pt: 6 }}>
          Még nincsenek munkák
        </Typography>
      ) : (
        <Stack spacing={2}>
          {jobs.map((job) => (
            <JobCard key={job.id} job={job} onClick={() => router.push(`/dashboard/jobs?jobId=${job.id}`)} />
          ))}
        </Stack>
      )}

      <JobDialog key={jobId} jobId={jobId} onClose={handleDialogClose} steps={steps} />

      {showCreate && (
        <CreateJobDialog
          onClose={handleCreateClose}
          initialExpertId={expertId}
          initialServiceId={serviceId}
        />
      )}

      {!isExpert && (
        <SpeedDial
          ariaLabel="Új munkakérés"
          sx={{ position: 'fixed', bottom: 24, right: 24 }}
          icon={<SpeedDialIcon />}
          onClick={() => setCreateDialogOpen(true)}
        />
      )}
    </Container>
  );
}
