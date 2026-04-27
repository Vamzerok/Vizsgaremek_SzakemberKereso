"use client";

import { useParams } from "next/navigation";
import { useState } from "react";
import useSWR from "swr";

import Divider from "@mui/material/Divider";
import Typography from "@mui/material/Typography";
import Box from "@mui/material/Box";
import CircularProgress from "@mui/material/CircularProgress";
import Stack from "@mui/material/Stack";
import Collapse from "@mui/material/Collapse";

import ExpandMoreIcon from "@mui/icons-material/ExpandMore";

import { genericFetcher } from "@/utils/fetcher";
import ServiceCard from "@/components/search/ServiceCard";

function SpecialtySection({ specialty, expertId }) {
  const [open, setOpen] = useState(true);

  return (
    <Box>
      <Box
        onClick={() => setOpen((v) => !v)}
        sx={{ display: "flex", alignItems: "center", justifyContent: "space-between", cursor: "pointer", pb: 0.5, userSelect: "none" }}
      >
        <Typography variant="body1" fontWeight={600}>{specialty.name}</Typography>
        <ExpandMoreIcon
          fontSize="small"
          color="action"
          sx={{ transition: "transform 0.2s", transform: open ? "rotate(180deg)" : "rotate(0deg)" }}
        />
      </Box>
      <Divider />
      <Collapse in={open}>
        <Box sx={{ pt: 2 }}>
          {specialty.services?.length > 0 ? (
            <Stack spacing={1.5}>
              {specialty.services.map((service) => (
                <ServiceCard key={service.id} service={service} expertId={expertId} />
              ))}
            </Stack>
          ) : (
            <Typography variant="body2" color="text.secondary">
              Még nincsenek meghirdetett szolgáltatások.
            </Typography>
          )}
        </Box>
      </Collapse>
    </Box>
  );
}

export default function ExpertServicesPage() {
  const { slug } = useParams();
  const { data: expert, isLoading, error } = useSWR(
    slug ? `/api/Experts/${slug}` : null,
    genericFetcher
  );

  if (isLoading) {
    return (
      <Box sx={{ display: "flex", justifyContent: "center", mt: 6 }}>
        <CircularProgress />
      </Box>
    );
  }

  if (error || !expert) {
    return <Typography color="error">Hiba történt a szolgáltatások lekérése közben</Typography>;
  }

  const specialties = expert.expertSpecialties ?? [];

  return (
    <Stack spacing={4}>
      {specialties.length === 0 ? (
        <Typography color="text.secondary">Még nincsenek meghírdetett szolgáltatások</Typography>
      ) : (
        specialties.map((specialty) => (
          <SpecialtySection key={specialty.id} specialty={specialty} expertId={expert.userId} />
        ))
      )}
    </Stack>
  );
}
