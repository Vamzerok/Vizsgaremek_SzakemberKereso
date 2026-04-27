"use client";

import { useParams } from "next/navigation";
import useSWR from "swr";

import { genericFetcher } from "@/utils/fetcher";
import PublicJobCard from "@/components/public/PublicJobCard";

import Grid from "@mui/material/Grid";
import Skeleton from "@mui/material/Skeleton";
import Typography from "@mui/material/Typography";

function JobCardSkeleton() {
  return <Skeleton variant="rectangular" height={110} sx={{ borderRadius: 1 }} />;
}

export default function ExpertJobsPage() {
  const { slug } = useParams();
  const { data: jobs, isLoading, error } = useSWR(
    slug ? `/api/Experts/${slug}/jobs` : null,
    genericFetcher
  );

  if (isLoading) {
    return (
      <Grid container spacing={2}>
        {[1, 2, 3, 4, 5, 6].map((i) => (
          <Grid size={{ xs: 12, sm: 6, md: 4, lg: 3 }} key={i}>
            <JobCardSkeleton />
          </Grid>
        ))}
      </Grid>
    );
  }

  if (error) {
    return <Typography color="error">Hiba a munkák betöltésekor.</Typography>;
  }

  if (!jobs?.length) {
    return <Typography color="text.secondary">Még nincsenek befejezett munkák.</Typography>;
  }

  return (
    <Grid container spacing={2}>
      {jobs.map((job) => (
        <Grid size={{ xs: 12, sm: 6, md: 4, lg: 3 }} key={job.id}>
          <PublicJobCard job={job} />
        </Grid>
      ))}
    </Grid>
  );
}
