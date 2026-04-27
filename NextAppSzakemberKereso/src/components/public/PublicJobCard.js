import Card from "@mui/material/Card";
import CardContent from "@mui/material/CardContent";
import Chip from "@mui/material/Chip";
import Stack from "@mui/material/Stack";
import Typography from "@mui/material/Typography";

import StarIcon from "@mui/icons-material/Star";
import AccessTimeIcon from "@mui/icons-material/AccessTime";
import PaidIcon from "@mui/icons-material/Paid";

import dayjs from "dayjs";
import { formatPricing } from "@/utils/pricing";

function calcTotalMinutes(appointments) {
  return (appointments ?? []).reduce((sum, a) => {
    const start = dayjs(`2000-01-01T${a.startTime}`);
    const end = dayjs(`2000-01-01T${a.endTime}`);
    return sum + end.diff(start, "minute");
  }, 0);
}

function formatDuration(minutes) {
  if (!minutes) return null;
  const h = Math.floor(minutes / 60);
  const m = minutes % 60;
  if (h === 0) return `${m} perc`;
  if (m === 0) return `${h} ó`;
  return `${h} ó ${m} perc`;
}

export default function PublicJobCard({ job }) {
  const totalMinutes = calcTotalMinutes(job.offeredAppointments);
  const formattedDuration = formatDuration(totalMinutes);
  const pricing = formatPricing(job.pricing);

  return (
    <Card variant="outlined" sx={{ height: "100%" }}>
      <CardContent>
        <Stack direction="row" justifyContent="space-between" alignItems="flex-start" spacing={1} sx={{ mb: 0.5 }}>
          <Typography variant="body2" fontWeight={600} sx={{ flexShrink: 1 }}>
            {job.title}
          </Typography>
          <Stack direction="row" spacing={0.5} alignItems="center" sx={{ flexShrink: 0 }}>
            <StarIcon sx={{ fontSize: 14, color: "warning.main" }} />
            <Typography variant="caption" fontWeight={600}>
              {job.rating != null ? job.rating.toFixed(1) : "—"}
            </Typography>
          </Stack>
        </Stack>

        <Typography variant="caption" color="text.secondary" display="block" sx={{ mb: 1.5 }}>
          {job.service?.name}
        </Typography>

        <Stack direction="row" spacing={1} flexWrap="wrap" useFlexGap>
          {pricing && (
            <Chip
              icon={<PaidIcon />}
              label={pricing}
              size="small"
              variant="outlined"
            />
          )}
          {formattedDuration && (
            <Chip
              icon={<AccessTimeIcon />}
              label={formattedDuration}
              size="small"
              variant="outlined"
            />
          )}
        </Stack>
      </CardContent>
    </Card>
  );
}
