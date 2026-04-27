"use client";

import { useParams } from "next/navigation";
import useSWR from "swr";
import Link from "next/link";

import { useTheme } from "@mui/material/styles";
import useMediaQuery from "@mui/material/useMediaQuery";

import Divider from "@mui/material/Divider";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import CircularProgress from "@mui/material/CircularProgress";
import Grid from "@mui/material/Grid";
import Stack from "@mui/material/Stack";

import ArrowForwardIcon from "@mui/icons-material/ArrowForward";

import { genericFetcher } from "@/utils/fetcher";
import PublicJobCard from "@/components/public/PublicJobCard";
import ServiceCard from "@/components/search/ServiceCard";

function SectionTitle({ label, href }) {
  return (
    <Box
      component={Link}
      href={href}
      sx={{
        display: "inline-flex",
        alignItems: "center",
        gap: 0.75,
        textDecoration: "none",
        color: "inherit",
        "&:hover .arrow": { transform: "translateX(4px)" },
      }}
    >
      <Typography variant="h6" fontWeight={600}>{label}</Typography>
      <ArrowForwardIcon
        className="arrow"
        fontSize="small"
        color="action"
        sx={{ transition: "transform 0.15s" }}
      />
    </Box>
  );
}

function ReferenceSection({ href, isLoading, jobs, maxNumberOfJobs = 4 }) {
  const theme = useTheme();
  const isSm = useMediaQuery(theme.breakpoints.up("sm"));
  const isMd = useMediaQuery(theme.breakpoints.up("md"));
  const isLg = useMediaQuery(theme.breakpoints.up("lg"));
  const isXl = useMediaQuery(theme.breakpoints.up("xl"));

  const desktopCols = isXl ? maxNumberOfJobs : isLg ? maxNumberOfJobs - 1 : maxNumberOfJobs - 2;

  const count = isSm ? desktopCols : maxNumberOfJobs;
  const previewJobs = (jobs ?? []).slice(0, count);

  return (
    <Box>
      <SectionTitle label="Referenciák" href={href} />
      <Divider sx={{ my: 1.5 }} />
      {isLoading ? (
        <CircularProgress size={24} />
      ) : previewJobs.length === 0 ? (
        <Typography variant="body2" color="text.secondary">Nincs befejezett munka.</Typography>
      ) : (
        <Grid container spacing={2}>
          {previewJobs.map((job) => (
            <Grid size={{ xs: 12, sm: 6, md: 6, lg: 4, xl: 3 }} key={job.id}>
              <PublicJobCard job={job} />
            </Grid>
          ))}
        </Grid>
      )}
    </Box>
  );
}

function ServiceSection({href, expert, maxNumberOfServices = 3}){
  const allServices = expert.expertSpecialties?.flatMap((s) => s.services ?? []).slice(0, maxNumberOfServices) ?? [];

  return (
    <Box>
      <SectionTitle label="Szolgáltatások" href={href} />

      <Divider sx={{ my: 1.5 }} />

      {allServices.length === 0 ? (
        <Typography variant="body2" color="text.secondary">Nincs meghirdetett szolgáltatás.</Typography>
      ) : (
        <Stack spacing={1.5}>
          {allServices.map((service) => (
            <ServiceCard key={service.id} service={service} expertId={expert.userId} />
          ))}
        </Stack>
      )}
    </Box>
  )
}

export default function ExpertProfilePage() {
  const { slug } = useParams();

  const { data: expert, isLoading: expertLoading, error: expertError } = useSWR(
    slug ? `/api/Experts/${slug}` : null,
    genericFetcher
  );
  const { data: jobs, isLoading: jobsLoading } = useSWR(
    slug ? `/api/Experts/${slug}/jobs` : null,
    genericFetcher
  );

  if (expertLoading) {
    return (
      <Box sx={{ display: "flex", justifyContent: "center", mt: 6 }}>
        <CircularProgress />
      </Box>
    );
  }

  if (expertError || !expert) {
    return <Typography color="error">A szakember adata nem található.</Typography>;
  }


  return (
    <Stack spacing={5}>
      <ReferenceSection
        href={`/experts/${slug}/jobs`}
        isLoading={jobsLoading}
        jobs={jobs}
        maxNumberOfJobs={4}/>
      <ServiceSection
        href={`/experts/${slug}/services`}
        expert={expert}
      />
    </Stack>
  );
}
