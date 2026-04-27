import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import Avatar from '@mui/material/Avatar';
import Chip from '@mui/material/Chip';
import Box from '@mui/material/Box';
import Accordion from '@mui/material/Accordion';
import Divider from '@mui/material/Divider';
import Stack from '@mui/material/Stack';
import Typography from '@mui/material/Typography';
import Card from '@mui/material/Card';
import AccordionDetails from '@mui/material/AccordionDetails';
import Button from '@mui/material/Button';
import AccordionSummary from '@mui/material/AccordionSummary';
import CardContent from '@mui/material/CardContent';
import LocationOnIcon from '@mui/icons-material/LocationOn';
import Link from 'next/link';

import { PricingType } from '@/utils/pricing';
import ServiceCard from './ServiceCard';

function serviceMatchesFilter(s, filterParams) {
  if (filterParams.minFixedPrice != null && s.pricing.fixedPrice < filterParams.minFixedPrice) return false;
  if (filterParams.maxFixedPrice != null && s.pricing.fixedPrice > filterParams.maxFixedPrice) return false;
  if (!filterParams.allowUnitBased && s.pricing.pricingType === PricingType.FixedAndUnitBased) return false;
  if (filterParams.unitName && s.pricing.unitName !== filterParams.unitName) return false;
  return true;
}

export default function ExpertCard({ expert, filterParams, isMobile }) {
  const initials =
    `${expert.firstName?.[0] ?? ''}${expert.lastName?.[0] ?? ''}`.toUpperCase() || '?';

  const relevantSpecialties = filterParams.occupationId
    ? expert.expertSpecialties?.filter((es) => es.occupationId === filterParams.occupationId)
    : expert.expertSpecialties ?? [];

  const specialtiesWithServices = relevantSpecialties
    .map((es) => ({
      ...es,
      matchingServices: (es.services ?? []).filter((s) => serviceMatchesFilter(s, filterParams)),
    }))
    .filter((es) => es.matchingServices.length > 0);


  return (
    <Card variant="outlined">
      <CardContent>
        <Stack direction="row" spacing={2} alignItems="flex-start">
          <Avatar sx={{ width: 52, height: 52, flexShrink: 0, mt: 0.5 }}>{initials}</Avatar>

          <Box sx={{ flexGrow: 1, minWidth: 0 }}>
            <Stack direction="row" alignItems="center" justifyContent="space-between" flexWrap="wrap" gap={1}>
              <Typography variant="subtitle1" fontWeight="bold">
                {expert.firstName} {expert.lastName}
              </Typography>
              {!isMobile && (
                <Button
                  component={Link}
                  href={`/experts/${expert.userId}`}
                  variant="outlined"
                  size="small"
                  sx={{ flexShrink: 0 }}
                >
                  Profil megtekintése
                </Button>
              )}
            </Stack>

            {expert.workLocation && (
              <Stack direction="row" alignItems="center" spacing={0.5} sx={{ mt: 0.5 }}>
                <LocationOnIcon sx={{ fontSize: 14, color: 'text.secondary' }} />
                <Typography variant="body2" color="text.secondary">
                  {expert.workLocation.name}, {expert.workLocation.countyName}
                </Typography>
              </Stack>
            )}

            {!isMobile && expert.expertSpecialties.length > 0 && (
              <Stack direction="row" flexWrap="wrap" gap={0.5} sx={{ mt: 1 }}>
                {expert.expertSpecialties.map((es) => (
                  <Chip key={es.id} label={es.name} size="small" variant="outlined" />
                ))}
              </Stack>
            )}
          </Box>
        </Stack>
      </CardContent>

      <Accordion defaultExpanded disableGutters elevation={0} sx={{ borderTop: '1px solid', borderColor: 'divider' }}>
        <AccordionSummary expandIcon={<ExpandMoreIcon />}>
          <Typography variant="body2" color="text.secondary">
            Szolgáltatások
          </Typography>
        </AccordionSummary>
        <AccordionDetails>
          {specialtiesWithServices.length === 0 ? (
            <Typography variant="body2" color="text.secondary">
              Nincsenek elérhető szolgáltatások
            </Typography>
          ) : (
            <Stack spacing={2}>
              {specialtiesWithServices.map((es, index) => (
                <Box key={es.id}>
                  <Divider sx={{ mb: 1, mt: index > 0 ? 3 : 0 }}>
                    <Typography variant="caption" color="text.secondary">
                      {es.name}
                    </Typography>
                  </Divider>
                  <Stack spacing={1.5}>
                    {es.matchingServices.map((s) => (
                      <ServiceCard key={s.id} service={s} expertId={expert.userId} />
                    ))}
                  </Stack>
                </Box>
              ))}
            </Stack>
          )}
        </AccordionDetails>
      </Accordion>
    </Card>
  );
}
