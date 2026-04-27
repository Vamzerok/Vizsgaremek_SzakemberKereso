"use client";

import { useParams } from "next/navigation";
import useSWR from "swr";

import Skeleton from "@mui/material/Skeleton";
import Avatar from "@mui/material/Avatar";
import Stack from "@mui/material/Stack";
import Typography from "@mui/material/Typography";
import Paper from "@mui/material/Paper";
import Box from "@mui/material/Box";
import Divider from "@mui/material/Divider";

import EmailIcon from "@mui/icons-material/Email";
import LocationOnIcon from "@mui/icons-material/LocationOn";
import PhoneIcon from "@mui/icons-material/Phone";

import { genericFetcher } from "@/utils/fetcher";

export default function ExpertProfileInfo() {
  const { slug } = useParams();

  const { data: expert, isLoading } = useSWR(
    slug ? `/api/Experts/${slug}` : null,
    genericFetcher
  );

  if (isLoading) {
    return (
      <Paper sx={{ p: 3 }}>
        <Stack spacing={1.5} alignItems="center">
          <Skeleton variant="circular" width={100} height={100} />
          <Skeleton variant="text" width={140} height={24} />
          <Skeleton variant="text" width={180} height={18} />
          <Divider flexItem />
          <Skeleton variant="text" width="90%" height={18} />
          <Skeleton variant="text" width="80%" height={18} />
        </Stack>
      </Paper>
    );
  }

  if (!expert) return null;

  const initials = `${expert.firstName?.[0] ?? ""}${expert.lastName?.[0] ?? ""}`.toUpperCase() || "?";

  return (
    <Paper sx={{ p: 3 }}>
      <Stack spacing={2} alignItems="center" sx={{ textAlign: "center" }}>
        <Avatar sx={{ width: 100, height: 100, fontSize: 32, bgcolor: "primary.main" }}>
          {initials}
        </Avatar>
        <Box>
          <Typography variant="h6" fontWeight={600} lineHeight={1.3}>
            {expert.lastName} {expert.firstName}
          </Typography>
          <Typography variant="body2" color="text.secondary" sx={{ wordBreak: "break-all", mt: 0.5 }}>
            {expert.email}
          </Typography>
        </Box>
      </Stack>

      {expert.biography && (
        <>
          <Divider sx={{ my: 2 }} />
          <Typography variant="body2" color="text.secondary" sx={{ whiteSpace: "pre-line" }}>
            {expert.biography}
          </Typography>
        </>
      )}

      <Divider sx={{ my: 2 }} />

      <Stack spacing={1.5}>
        {expert.companyEmail && (
          <Stack direction="row" spacing={1.5} alignItems="center">
            <EmailIcon fontSize="small" color="action" sx={{ flexShrink: 0 }} />
            <Typography variant="body2" sx={{ wordBreak: "break-all" }}>
              {expert.companyEmail}
            </Typography>
          </Stack>
        )}
        {expert.companyPhoneNumber && (
          <Stack direction="row" spacing={1.5} alignItems="center">
            <PhoneIcon fontSize="small" color="action" sx={{ flexShrink: 0 }} />
            <Typography variant="body2">{expert.companyPhoneNumber}</Typography>
          </Stack>
        )}
        {expert.workLocation && (
          <Stack direction="row" spacing={1.5} alignItems="center">
            <LocationOnIcon fontSize="small" color="action" sx={{ flexShrink: 0 }} />
            <Typography variant="body2">
              {expert.workLocation.name}, {expert.workLocation.countyName}
            </Typography>
          </Stack>
        )}
      </Stack>
    </Paper>
  );
}
