import CardContent from "@mui/material/CardContent";
import Chip from "@mui/material/Chip";
import Box from "@mui/material/Box";
import Card from "@mui/material/Card";
import Typography from "@mui/material/Typography";

export default function OccupationCard({ specialty, onClick }) {
  const serviceCount = specialty.services?.length ?? 0;

  return (
    <Card
      variant="outlined"
      onClick={onClick}
      sx={{ height: "100%", cursor: "pointer", "&:hover": { borderColor: "primary.main" }, transition: "border-color 0.15s" }}
    >
      <CardContent>
        <Typography variant="subtitle1" fontWeight={600} noWrap gutterBottom>
          {specialty.name}
        </Typography>
        <Chip label={`FEOR ${specialty.occupationId}`} size="small" variant="outlined" sx={{ mb: 2 }} />
        <Box sx={{ textAlign: "center", pt: 1 }}>
          <Typography variant="h3" fontWeight={700} color="primary" lineHeight={1}>
            {serviceCount}
          </Typography>
          <Typography variant="body2" color="text.secondary" sx={{ mt: 0.5 }}>
            szolgáltatás
          </Typography>
        </Box>
      </CardContent>
    </Card>
  );
}