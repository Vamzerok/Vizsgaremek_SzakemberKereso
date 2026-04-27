import Box from "@mui/material/Box";
import Stack from "@mui/material/Stack";
import Typography from "@mui/material/Typography";

//not used but looks kinda nice
export default function PasswordStrengthDisplay({ evaluatedRules }) {
  return (
    <Stack spacing={0.5} sx={{ mt: 1 }}>
      {Object.values(evaluatedRules).map((rule, i) => (
        <Box
          key={i}
          sx={{
            px: 1,
            py: 0.25,
            borderRadius: 1,
            bgcolor: rule.isValid ? "success.light" : "warning.light",
            display: "inline-block",
            width: "fit-content",
          }}
        >
          <Typography
            variant="caption"
            color={rule.isValid ? "success.dark" : "warning.dark"}
          >
            {rule.text}
          </Typography>
        </Box>
      ))}
    </Stack>
  );
}
