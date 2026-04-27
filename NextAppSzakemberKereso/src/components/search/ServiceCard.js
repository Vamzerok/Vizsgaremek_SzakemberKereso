'use client';

import { useState } from 'react';
import { useMediaQuery, useTheme } from '@mui/material';

import CardContent from '@mui/material/CardContent';
import Chip from '@mui/material/Chip';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import Stack from '@mui/material/Stack';
import Tooltip from '@mui/material/Tooltip';
import AddShoppingCartIcon from '@mui/icons-material/AddShoppingCart';
import Box from '@mui/material/Box';
import Typography from '@mui/material/Typography';
import Card from '@mui/material/Card';
import Link from 'next/link';

import { formatPricing } from '@/utils/pricing';

const BREAKPOINTS = { small: 'xs', big: 'sm' };

export default function ServiceCard({ service, expertId }) {
  const isSmallScreen = useMediaQuery(useTheme().breakpoints.down(BREAKPOINTS.big));
  const [expanded, setExpanded] = useState(false);

  return (
    <Card variant="outlined" sx={{ display: 'flex', flexDirection: { [BREAKPOINTS.small]: 'column', [BREAKPOINTS.big]: 'row' }, overflow: 'hidden' }}>
      <Box sx={{ flexGrow: 1, minWidth: 0 }}>
        <CardContent sx={{ pb: 1 }}>
          <Typography variant="subtitle2" fontWeight={600}>
            {service.name}
          </Typography>

          {service.description && (
            <Box>
              <Typography
                variant="body2"
                color="text.secondary"
                sx={{
                  mt: 0.5,
                  overflowWrap: 'break-word',
                  ...(!expanded && {
                    display: '-webkit-box',
                    WebkitLineClamp: 3,
                    WebkitBoxOrient: 'vertical',
                    overflow: 'hidden',
                  }),
                }}
              >
                {service.description}
              </Typography>
              <Box
                onClick={() => setExpanded((v) => !v)}
                sx={{ display: 'flex', justifyContent: 'center', cursor: 'pointer', mt: 0.25 }}
              >
                <ExpandMoreIcon
                  fontSize="small"
                  color="action"
                  sx={{ transition: 'transform 0.2s', transform: expanded ? 'rotate(180deg)' : 'rotate(0deg)' }}
                />
              </Box>
            </Box>
          )}

          <Chip
            label={formatPricing(service.pricing)}
            size="small"
            color="primary"
            variant="outlined"
            sx={{ mt: 1 }}
          />
        </CardContent>
      </Box>

      <Tooltip title={isSmallScreen ? "" : "Kérvényzés"} placement="left">
        <Box
          component={Link}
          href={`/dashboard/jobs?expertId=${expertId}&serviceId=${service.id}`}
          sx={{
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            bgcolor: 'primary.main',
            color: 'white',
            width: { [BREAKPOINTS.small]: '100%', [BREAKPOINTS.big]: 48 },
            height: { [BREAKPOINTS.small]: 40, [BREAKPOINTS.big]: 'auto' },
            flexShrink: 0,
            transition: 'filter 0.15s',
            '&:hover': { filter: 'brightness(1.15)' },
          }}
        >
          <Stack direction="row" gap={1}>
            <AddShoppingCartIcon fontSize="small" />
            {isSmallScreen && <Typography fontSize="small">KÉRVÉNYEZÉS</Typography>}
          </Stack>
        </Box>
      </Tooltip>
    </Card>
  );
}
