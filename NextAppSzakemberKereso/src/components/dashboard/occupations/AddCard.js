import { alpha } from "@mui/material/styles";
import Card from "@mui/material/Card";
import IconButton from "@mui/material/IconButton";
import AddIcon from "@mui/icons-material/Add";

export default function AddCard({ onClick }) {
  {/* had way too much fun with this ngl */}
  return (
    <Card
      onClick={onClick}
      sx={{
        height: "100%",
        cursor: "pointer",
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
        bgcolor: (theme) => alpha(theme.palette.primary.main, 0.08),
        border: "1px solid",
        borderColor: (theme) => alpha(theme.palette.primary.main, 0.08),
        "&:hover": { bgcolor: (theme) => alpha(theme.palette.primary.main, 0.13) },
      }}
    >
      <IconButton
        disableRipple
        sx={{
          width: 56,
          height: 56,
          color: "primary.main",
          bgcolor: (theme) => alpha(theme.palette.primary.main, 0.12),
        }}
      >
        <AddIcon fontSize="large" />
      </IconButton>
    </Card>
  );
}