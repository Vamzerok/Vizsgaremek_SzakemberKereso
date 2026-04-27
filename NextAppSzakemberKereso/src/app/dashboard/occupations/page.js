"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";


import Box from "@mui/material/Box";
import Grid from "@mui/material/Grid";
import Skeleton from "@mui/material/Skeleton";
import Typography from "@mui/material/Typography";

import OccupationCard from "@/components/dashboard/occupations/OccupationCard";
import AddCard from "@/components/dashboard/occupations/AddCard";
import AddOccupationDialog from "@/components/dashboard/occupations/AddOccupationDialog";
import useUser from "@/hooks/useUser";
import useExpert from "@/hooks/useExpert";
import { isUserAnExpert } from "@/utils/user";


const GRID_SIZE = { xs: 12, sm: 6, md: 4, lg: 3 };
const CARD_HEIGHT = 200;

export default function OccupationsPage() {
  const router = useRouter();
  const { user } = useUser();
  const { expert, isLoading, mutate: mutateExpert } = useExpert();
  const [dialogOpen, setDialogOpen] = useState(false);

  const isExpert = isUserAnExpert(user);

  if (!isExpert) {
    return (
      <Box sx={{ p: 2 }}>
        <Typography variant="h6" color="text.secondary">
          Ez az oldal csak szakemberek számára elérhető.
        </Typography>
      </Box>
    );
  }

  return (
    <Box sx={{ p: 2 }}>
      <Grid container spacing={2}>
        {isLoading ? (
          [0, 1, 2, 3].map((i) => (
            <Grid size={GRID_SIZE} key={i} sx={{ height: CARD_HEIGHT }}>
              <Skeleton variant="rectangular" height="100%" sx={{ borderRadius: 1 }} />
            </Grid>
          ))
        ) : (
          <>
            {expert?.expertSpecialties?.map((specialty) => (
              <Grid size={GRID_SIZE} key={specialty.occupationId} sx={{ height: CARD_HEIGHT }}>
                <OccupationCard
                  specialty={specialty}
                  onClick={() => router.push("/dashboard/services")}
                />
              </Grid>
            ))}
            <Grid size={GRID_SIZE} sx={{ height: CARD_HEIGHT }}>
              <AddCard onClick={() => setDialogOpen(true)} />
            </Grid>
          </>
        )}
      </Grid>

      <AddOccupationDialog
        open={dialogOpen}
        onClose={() => setDialogOpen(false)}
        existingOccupationIds={expert?.expertSpecialties?.map((s) => s.occupationId)}
        onSuccess={mutateExpert}
      />
    </Box>
  );
}
