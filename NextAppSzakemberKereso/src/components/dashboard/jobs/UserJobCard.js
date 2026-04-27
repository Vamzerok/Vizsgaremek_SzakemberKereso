'use client';

import Box from '@mui/material/Box';
import Card from '@mui/material/Card';
import CardActionArea from '@mui/material/CardActionArea';
import CardContent from '@mui/material/CardContent';
import Divider from '@mui/material/Divider';
import Stack from '@mui/material/Stack';
import Typography from '@mui/material/Typography';
import { useMediaQuery } from '@mui/material';

import JobStateIndicator, { USER_STATE_CONFIG } from './JobStateIndicator';
import { formatLocation } from '@/utils/location';

function LabeledRow({ label, value }) {
  return (
    <Stack direction="row" spacing={1}>
      <Typography variant="caption" color="text.secondary" sx={{ minWidth: 70 }}>
        {label}
      </Typography>
      <Typography variant="caption" noWrap>
        {value}
      </Typography>
    </Stack>
  );
}

export default function UserJobCard({ job, onClick }) {
  const isSmallScreen = useMediaQuery((theme) => theme.breakpoints.down('sm'));

  return (
    <Card variant="outlined">
      <CardActionArea onClick={onClick}>
        <CardContent>
          <Stack direction="row" flexWrap="wrap" alignItems="center" gap={1} mb={1.5}>
            <Typography variant="subtitle1" fontWeight="bold" noWrap sx={{ flexGrow: 1, minWidth: 0 }}>
              {job.title}
            </Typography>
            {/*lil hack to force a wrap on smaller screens */}
            <Box sx={{ flexBasis: '100%', height: 0, display: { xs: 'block', sm: 'none' } }} />
            <JobStateIndicator status={job.status} config={USER_STATE_CONFIG} sx={{ flexShrink: 0 }} />
          </Stack>

          <Divider sx={{ mb: 1.5 }} />

          <Stack direction={isSmallScreen ? "column" : "row"} spacing={2}>
            <Stack spacing={0.5} sx={{ minWidth: 180, flexShrink: 0 }}>
              <LabeledRow label="Szolgáltatás" value={job.service?.name} />
              <LabeledRow label="Szakember" value={job.service?.expertName} />
              <LabeledRow label="Helyszín" value={formatLocation(job.location)} />
            </Stack>

            <Box sx={{ flexGrow: 1, minWidth: 0 }}>
              <Typography
                variant="body2"
                color="text.secondary"
                sx={{
                  display: '-webkit-box',
                  WebkitLineClamp: 4,
                  WebkitBoxOrient: 'vertical',
                  overflow: 'hidden',
                }}
              >
                {job.description}
              </Typography>
            </Box>
          </Stack>
        </CardContent>
      </CardActionArea>
    </Card>
  );
}
