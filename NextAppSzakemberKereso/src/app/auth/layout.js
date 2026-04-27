import Box from "@mui/material/Box";

export default function AuthLayout({ children }) {
  return (
    <Box
      sx={{
        minHeight: "100vh",
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
        bgcolor: "background.default",
      }}
    >
      <Box
        component="main"
        sx={{
          width: "100%",
          maxWidth: 480,
          px: 4,
          py: 8,
        }}
      >
        {children}
      </Box>
    </Box>
  );
}
