import Card from "@mui/material/Card";
import Chip from "@mui/material/Chip";
import Typography from "@mui/material/Typography";
import CardContent from "@mui/material/CardContent";

import { formatPricing } from "@/utils/pricing";

export default function ServiceCard({ service, onClick }) {
  return (
    <Card
      variant="outlined"
      onClick={onClick}
      sx={{
        display: "flex",
        flexDirection: "column",
        height: "100%",
        cursor: "pointer",
        "&:hover": { borderColor: "primary.main" },
      }}
    >
      <CardContent sx={{ display: "flex", flexDirection: "column", height: "100%", pb: "12px !important" }}>
        <Typography variant="subtitle1" fontWeight={600} gutterBottom>
          {service.name}
        </Typography>

        {service.pricing && (
          <Chip
            label={formatPricing(service.pricing)}
            size="small"
            variant="outlined"
            sx={{ alignSelf: "flex-start", mb: 1 }}
          />
        )}

        {service.description && (
          <Typography
            variant="body2"
            color="text.secondary"
            sx={{
              display: "-webkit-box",
              WebkitLineClamp: 3,
              WebkitBoxOrient: "vertical",
              overflow: "hidden",
              overflowWrap: "break-word",
            }}
          >
            {service.description}
          </Typography>
        )}

        <Typography variant="caption" color="text.disabled" sx={{ mt: "auto" }}>
          Kattints a szerkesztéshez
        </Typography>
      </CardContent>
    </Card>
  );
}
