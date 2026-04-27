'use client';

import useSWR from 'swr';

import Stack from '@mui/material/Stack';
import DialogTitle from '@mui/material/DialogTitle';
import CircularProgress from '@mui/material/CircularProgress';
import Divider from '@mui/material/Divider';
import StepLabel from '@mui/material/StepLabel';
import Step from '@mui/material/Step';
import Box from '@mui/material/Box';
import IconButton from '@mui/material/IconButton';
import StepContent from '@mui/material/StepContent';
import Alert from '@mui/material/Alert';
import Stepper from '@mui/material/Stepper';
import Typography from '@mui/material/Typography';
import Dialog from '@mui/material/Dialog';
import DialogContent from '@mui/material/DialogContent';
import CloseIcon from '@mui/icons-material/Close';

import Link from 'next/link';
import DateTimeSpecifier from './DateTimeSpecifier';
import { CANCELLED_STEP, JOB_STATES } from '@/utils/jobState';
import { formatLocation } from '@/utils/location';
import { genericFetcher } from '@/utils/fetcher';

function modifyJob(job) {
  if (job.status !== JOB_STATES.Cancelled) return { ...job, isCancelled: false };
  return { ...job, isCancelled: true, status: job.cancelledFromStatus };
}

function buildSteps(job, steps) {
  if (!job.isCancelled) return steps;
  return [...steps.slice(0, job.status + 1), CANCELLED_STEP];
}

function getActiveStep(job, steps) {
  if (job.isCancelled) return steps.length - 1;
  return job.status;
}

const detailLinkSx = { color: 'inherit', textDecoration: 'none', '&:hover': { textDecoration: 'underline' } };

function JobDetails({ job }) {
  const hasDescription = !!job.description;

  return (
    <Box
      sx={{
        display: 'grid',
        gridTemplateColumns: hasDescription ? { xs: '1fr', md: '1fr 1fr' } : '1fr',
        gap: 2,
        mb: 3,
      }}
    >
      <Stack spacing={1.5}>
        <Stack spacing={0.5}>
          <Typography variant="caption" color="text.secondary">Szakember</Typography>
          <Typography variant="body2">
            <Link href={`/experts/${job.service?.expertUserId}`} style={detailLinkSx}>
              {job.service?.expertName}
            </Link>
          </Typography>
        </Stack>
        <Stack spacing={0.5}>
          <Typography variant="caption" color="text.secondary">Szolgáltatás</Typography>
          <Typography variant="body2">{job.service?.name}</Typography>
        </Stack>
        <Stack spacing={0.5}>
          <Typography variant="caption" color="text.secondary">Megrendelő</Typography>
          <Typography variant="body2">
            {job.initiatingUser?.lastName} {job.initiatingUser?.firstName}
          </Typography>
        </Stack>
        <Stack spacing={0.5}>
          <Typography variant="caption" color="text.secondary">Helyszín</Typography>
          <Typography variant="body2">{formatLocation(job.location)}</Typography>
        </Stack>
        {job.userAvailability?.length > 0 && (
          <Stack spacing={0.5}>
            <Typography variant="caption" color="text.secondary">Megadott időpontok</Typography>
            <DateTimeSpecifier value={job.userAvailability} disabled />
          </Stack>
        )}
      </Stack>

      {hasDescription && (
        <Box>
          <Typography variant="caption" color="text.secondary">Leírás</Typography>
          <Typography variant="body2" color="text.secondary" sx={{ mt: 0.5, whiteSpace: 'pre-line' }}>
            {job.description}
          </Typography>
        </Box>
      )}
    </Box>
  );
}

export default function JobDialog({ jobId, onClose, steps }) {
  const { data, isLoading, error, mutate } = useSWR(
    jobId ? `/api/Jobs/${jobId}` : null,
    genericFetcher,
    { refreshInterval: 3000 }
  );

  if (!jobId) return null;

  if (isLoading) {
    return (
      <Dialog open onClose={onClose} fullWidth maxWidth="md" scroll="paper">
        <DialogTitle>Munka részletei</DialogTitle>
        <DialogContent dividers>
          <Box sx={{ display: 'flex', justifyContent: 'center', py: 6 }}>
            <CircularProgress />
          </Box>
        </DialogContent>
      </Dialog>
    );
  }

  if (error || !data) {
    return (
      <Dialog open onClose={onClose} fullWidth maxWidth="md" scroll="paper">
        <DialogTitle>Munka részletei</DialogTitle>
        <DialogContent dividers>
          <Alert severity="error">Hiba történt a munka adatainak lekérésekor.</Alert>
        </DialogContent>
      </Dialog>
    );
  }

  const job = modifyJob(data);
  const builtSteps = buildSteps(job, steps);
  const activeStep = getActiveStep(job, builtSteps);

  return (
    <Dialog open onClose={onClose} fullWidth maxWidth="md" scroll="paper">
      <DialogTitle sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
        {job.title}
        <IconButton size="small" onClick={onClose}>
          <CloseIcon fontSize="small" />
        </IconButton>
      </DialogTitle>

      <DialogContent dividers>
        <JobDetails job={job} />
        <Divider sx={{ mb: 2 }} />

        <Stepper activeStep={activeStep} orientation="vertical">
          {builtSteps.map((step, index) => (
            <Step
              key={step.state}
              expanded={step.state <= job.status && step.expandWhenPast}
            >
              <StepLabel slotProps={{ stepIcon: { error: step.state === JOB_STATES.Cancelled } }}>
                {step.label}
              </StepLabel>
              <StepContent>
                {step.component && (
                  <step.component
                    job={job}
                    refreshJob={mutate}
                    disabled={index !== activeStep}
                    {...step.extraProps}
                  />
                )}
              </StepContent>
            </Step>
          ))}
        </Stepper>
      </DialogContent>
    </Dialog>
  );
}
