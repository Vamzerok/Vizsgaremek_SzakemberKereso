import Box from "@mui/material/Box";
import Container from "@mui/material/Container";

import ExpertProfileInfo from "@/components/public/ExpertProfileInfo";

export default function ExpertPublicLayout({ children }) {
  return (
    <Container maxWidth="xl" sx={{ py: 4 }}>
      <Box sx={{
        display: "grid",
        gridTemplateColumns: { xs: "1fr", md: "280px 1fr" },
        alignItems: "start",
        gap: 3,
      }}>
        <Box sx={{ position: "sticky", top: { md: 80 } }}>
          <ExpertProfileInfo />
        </Box>
        <Box>{children}</Box>
      </Box>
    </Container>
  );
}
