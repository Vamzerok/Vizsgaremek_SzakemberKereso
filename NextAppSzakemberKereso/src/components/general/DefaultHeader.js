import AppBar from "@mui/material/AppBar";
import Box from "@mui/material/Box";
import Container from "@mui/material/Container";
import Stack from "@mui/material/Stack";
import Toolbar from "@mui/material/Toolbar";
import Typography from "@mui/material/Typography";
import Link from "next/link";
import AccountMenu from "@/components/general/AccountMenu";

export default function DefaultHeader() {
  return (
    <AppBar sx={{ position: "sticky" }}>
      <Container maxWidth="xl">
        <Toolbar variant="regular" sx={{ gap: 2 }}>
          <Link href="/">
            <Typography variant="h6" sx={{ fontWeight: 600, color: 'inherit', textDecoration: 'none', mr: 2, whiteSpace: 'nowrap', flexShrink: 0 }}>
              Szakember Kereső
            </Typography>
          </Link>

          <Stack direction="row" spacing={3} sx={{ flexGrow: 1, display: { xs: 'none', sm: 'flex' } }}>
            <Link href="/" style={{ textDecoration: 'none', fontSize: '0.9rem', fontWeight: 400 }}>Főoldal</Link>
            <Link href="/search" style={{ textDecoration: 'none', fontSize: '0.9rem', fontWeight: 400 }}>Keresés</Link>
          </Stack>

          <Box sx={{ flexGrow: { xs: 1, sm: 0 } }} />

          <AccountMenu />
        </Toolbar>
      </Container>
    </AppBar>
  );
}
