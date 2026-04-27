import Box from "@mui/material/Box";
import Container from "@mui/material/Container";
import Typography from "@mui/material/Typography";
import Button from "@mui/material/Button";
import Link from "next/link";

export default function UserPublicNotFound() {
  return (
    <Container maxWidth="sm">
      <Box sx={{ textAlign: "center", py: 10 }}>
        <Typography variant="h3" fontWeight={600} gutterBottom>
          404
        </Typography>
        <Typography variant="h6" color="text.secondary" gutterBottom>
          Az oldal nem található.
        </Typography>
        <Button component={Link} href="/" variant="contained" sx={{ mt: 3 }}>
          Főoldalra
        </Button>
      </Box>
    </Container>
  );
}
