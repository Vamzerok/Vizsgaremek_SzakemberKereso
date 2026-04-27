"use client";
import { useEffect } from "react";
import { useRouter } from "next/navigation";
import useUser from "@/hooks/useUser";
import { isUserAnExpert } from "@/utils/user";
import Box from "@mui/material/Box";
import Paper from "@mui/material/Paper";
import Skeleton from "@mui/material/Skeleton";
import Sidebar from "@/components/dashboard/Sidebar";

export default function DashboardLayout({ children }) {
  const { user, isLoading, error } = useUser();
  const router = useRouter();

  useEffect(() => {
    if (!isLoading && error) {
      router.replace("/auth/login?from=/dashboard");
    }
  }, [isLoading, error, router]);

  if (isLoading) {
    return (
      <Box sx={{ p: 4 }}>
        <Skeleton variant="rectangular" height={200} />
      </Box>
    );
  }
  if (error) return null;

  const isExpert = isUserAnExpert(user);

  const muiMobileBreakpoint = "md"; 

  return (
    <Box sx={{ display: "flex" }}>
      <Sidebar isExpert={isExpert} user={user} />
      <Paper
        elevation={0}
        sx={(theme) => {
          const toolbar = theme.mixins.toolbar;
          return {
            width: "100%",
            overflow: "auto",
            p: "10px",
            paddingTop: `calc(${toolbar.minHeight}px + 10px)`,
            [theme.breakpoints.up(muiMobileBreakpoint)]: {
              paddingTop: `calc(${toolbar[theme.breakpoints.up(muiMobileBreakpoint)]?.minHeight ?? 64}px + 10px)`,
            },
            borderRadius: { md: "25px 0px 0px 25px" },
            backgroundColor: "background.default",
          };
        }}
      >
        {children}
      </Paper>
    </Box>
  );
}
