"use client";

import { useState, Suspense } from "react";

import useUser from "@/hooks/useUser";
import useExpert from "@/hooks/useExpert";
import { isUserAnExpert } from "@/utils/user";
import CUServiceDialog from "@/components/dashboard/services/CUServiceDialog";
import ServiceCard from "@/components/dashboard/services/ServiceCard";
import AddCard from "@/components/dashboard/occupations/AddCard";

import Box from "@mui/material/Box";
import Chip from "@mui/material/Chip";
import Divider from "@mui/material/Divider";
import Grid from "@mui/material/Grid";
import Skeleton from "@mui/material/Skeleton";
import Stack from "@mui/material/Stack";
import Typography from "@mui/material/Typography";

const GRID_SIZE = { xs: 12, sm: 6, md: 4, lg: 3 };
const CARD_HEIGHT = 200;

function ServicesPageContent() {
  const { user } = useUser();
  const { expert, isLoading, mutate: mutateExpert } = useExpert();

  const [dialogOpen, setDialogOpen] = useState(false);
  const [selectedService, setSelectedService] = useState(null);
  const [initialSpecialty, setInitialSpecialty] = useState(null);

  if (!isUserAnExpert(user)) {
    return (
      <Box sx={{ p: 2 }}>
        <Typography variant="h6" color="text.secondary">
          Ez az oldal csak szakemberek számára elérhető.
        </Typography>
      </Box>
    );
  }

  const specialties = expert?.expertSpecialties ?? [];

  function openAddDialog(specialty) {
    setSelectedService(null);
    setInitialSpecialty(specialty ?? null);
    setDialogOpen(true);
  }

  function openEditDialog(service) {
    setSelectedService(service);
    setInitialSpecialty(null);
    setDialogOpen(true);
  }

  function handleClose() {
    setDialogOpen(false);
    setSelectedService(null);
    setInitialSpecialty(null);
  }

  return (
    <Box sx={{ p: 2 }}>
      {isLoading ? (
        <Grid container spacing={2}>
          {[0, 1, 2, 3].map((i) => (
            <Grid size={GRID_SIZE} key={i} sx={{ height: CARD_HEIGHT }}>
              <Skeleton variant="rectangular" height="100%" sx={{ borderRadius: 1 }} />
            </Grid>
          ))}
        </Grid>
      ) : specialties.length === 0 ? (
        <Typography color="text.secondary" textAlign="center" sx={{ pt: 6 }}>
          Még nincsenek hozzáadott foglalkozások. Előbb adj hozzá foglalkozást.
        </Typography>
      ) : (
        <Stack spacing={4}>
          {specialties.map((specialty) => (
            <Box key={specialty.occupationId}>
              <Stack direction="row" spacing={1.5} alignItems="center" sx={{ mb: 1 }}>
                <Typography variant="subtitle1" fontWeight={600}>
                  {specialty.name}
                </Typography>
                <Chip
                  label={`FEOR ${specialty.occupationId}`}
                  size="small"
                  color="primary"
                  variant="outlined"
                />
              </Stack>
              <Divider sx={{ mb: 2 }} />
              <Grid container spacing={2}>
                {specialty.services?.map((service) => (
                  <Grid size={GRID_SIZE} key={service.id} sx={{ height: CARD_HEIGHT }}>
                    <ServiceCard service={service} onClick={() => openEditDialog(service)} />
                  </Grid>
                ))}
                <Grid size={GRID_SIZE} sx={{ height: CARD_HEIGHT }}>
                  <AddCard onClick={() => openAddDialog(specialty)} />
                </Grid>
              </Grid>
            </Box>
          ))}
        </Stack>
      )}

      <CUServiceDialog
        open={dialogOpen}
        onClose={handleClose}
        specialties={specialties}
        onSuccess={mutateExpert}
        service={selectedService}
        initialSpecialty={initialSpecialty}
      />
    </Box>
  );
}

export default function ServicesPage() {
  return (
    <Suspense>
      <ServicesPageContent />
    </Suspense>
  );
}
