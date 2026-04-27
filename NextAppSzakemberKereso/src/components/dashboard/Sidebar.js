"use client";

import { useState } from "react";
import { mutate } from "swr";
import { usePathname, useRouter } from "next/navigation";

import { styled, useTheme } from "@mui/material/styles";
import useMediaQuery from "@mui/material/useMediaQuery";
import Box from "@mui/material/Box";
import MuiDrawer from "@mui/material/Drawer";
import MuiAppBar from "@mui/material/AppBar";
import Toolbar from "@mui/material/Toolbar";
import List from "@mui/material/List";
import Divider from "@mui/material/Divider";
import ListItem from "@mui/material/ListItem";
import ListItemButton from "@mui/material/ListItemButton";
import ListItemText from "@mui/material/ListItemText";
import Typography from "@mui/material/Typography";
import Link from "next/link";
import Avatar from "@mui/material/Avatar";
import Menu from "@mui/material/Menu";
import MenuItem from "@mui/material/MenuItem";

import SearchIcon from '@mui/icons-material/Search';
import IconButton from "@mui/material/IconButton";
import ListItemIcon from "@mui/material/ListItemIcon";
import MenuIcon from "@mui/icons-material/Menu";
import ChevronLeftIcon from "@mui/icons-material/ChevronLeft";
import ChevronRightIcon from "@mui/icons-material/ChevronRight";
import HomeIcon from "@mui/icons-material/Home";
import LogoutIcon from "@mui/icons-material/Logout";

import { GENERIC_NAV_OPTIONS, EXPERT_NAV_OPTIONS } from "@/utils/nav";

const drawerWidth = 240;

const openedMixin = (theme) => ({
  width: drawerWidth,
  transition: theme.transitions.create('width', {
    easing: theme.transitions.easing.sharp,
    duration: theme.transitions.duration.enteringScreen,
  }),
  overflowX: 'hidden',
});

const closedMixin = (theme) => ({
  transition: theme.transitions.create('width', {
    easing: theme.transitions.easing.sharp,
    duration: theme.transitions.duration.leavingScreen,
  }),
  overflowX: 'hidden',
  width: `calc(${theme.spacing(7)} + 1px)`,
  [theme.breakpoints.up('sm')]: {
    width: `calc(${theme.spacing(8)} + 1px)`,
  },
});

const DrawerHeader = styled('div')(({ theme }) => ({
  display: 'flex',
  alignItems: 'center',
  justifyContent: 'flex-end',
  padding: theme.spacing(0, 1),//so that stuff is placed under the appbar
  ...theme.mixins.toolbar,
}));

const AppBar = styled(MuiAppBar, {
  shouldForwardProp: (prop) => prop !== 'open',
})(({ theme }) => ({
  zIndex: theme.zIndex.drawer + 1,
  transition: theme.transitions.create(['width', 'margin'], {
    easing: theme.transitions.easing.sharp,
    duration: theme.transitions.duration.leavingScreen,
  }),
  variants: [
    {
      props: ({ open }) => open,
      style: {
        [theme.breakpoints.up('sm')]: {
          marginLeft: drawerWidth,
          width: `calc(100% - ${drawerWidth}px)`,
          transition: theme.transitions.create(['width', 'margin'], {
            easing: theme.transitions.easing.sharp,
            duration: theme.transitions.duration.enteringScreen,
          }),
        },
      },
    },
  ],
}));

const Drawer = styled(MuiDrawer, {
  shouldForwardProp: (prop) => prop !== 'open',
})(({ theme }) => ({
  width: drawerWidth,
  flexShrink: 0,
  whiteSpace: "nowrap",
  boxSizing: "border-box",
  variants: [
    {
      props: ({ open }) => open,
      style: {
        ...openedMixin(theme),
        "& .MuiDrawer-paper": openedMixin(theme),
      },
    },
    {
      props: ({ open }) => !open,
      style: {
        ...closedMixin(theme),
        "& .MuiDrawer-paper": closedMixin(theme),
      },
    },
  ],
}));


export default function Sidebar({ isExpert, user }) {
  const theme = useTheme();
  const router = useRouter();
  const [open, setOpen] = useState(false);
  const [accountAnchor, setAccountAnchor] = useState(null);

  const initials = `${user?.firstName?.[0] ?? ""}${user?.lastName?.[0] ?? ""}`.toUpperCase() || "?";

  async function handleLogout() {
    await fetch("/api/logout", { method: "POST", credentials: "include" });
    await mutate('/api/Users/me', null, false);
    router.replace("/auth/login");
  }

  const isSmallScreen = useMediaQuery(theme.breakpoints.down("md"));

  const navItems = isExpert ? EXPERT_NAV_OPTIONS : GENERIC_NAV_OPTIONS;
  const pathname = usePathname();
  const currentPage = navItems.find((item) => item.href === pathname)?.text ?? "";

  const DrawerContent = (
    <>
      <DrawerHeader>
        <IconButton onClick={() => setOpen(false)}>
          {theme.direction === "rtl" ? <ChevronRightIcon /> : <ChevronLeftIcon />}
        </IconButton>
      </DrawerHeader>
      <Divider />
      <List>
        {navItems.map((option) => (
          <Link
            href={option.href}
            key={option.text}
            style={{ 
              textDecoration: "none", 
              color: "inherit" }}
            onClick={isSmallScreen ? () => setOpen(false) : undefined}
          >
            <ListItem disablePadding sx={{ display: "block" }}>
              <ListItemButton
                selected={option.href === pathname}
                sx={[
                  { minHeight: 48, px: 2.5 },
                  open ? { justifyContent: "initial" } : { justifyContent: "center" },
                ]}
              >
                <ListItemIcon
                  sx={[
                    { 
                      minWidth: 0, 
                      justifyContent: "center" 
                    },
                    open ? { mr: 3 } : { mr: "auto" },
                  ]}
                >
                  {option.icon}
                </ListItemIcon>
                <ListItemText
                  primary={option.text}
                  sx={[open ? { opacity: 1 } : { opacity: 0 }]}
                />
              </ListItemButton>
            </ListItem>
          </Link>
        ))}
      </List>
    </>
  );

  return (
    <Box sx={{ display: "flex" }}>
      <AppBar position="fixed" open={open}>
        <Toolbar>
          <IconButton
            color="inherit"
            aria-label="open drawer"
            onClick={() => setOpen(!open)}
            edge="start"
            sx={[{ marginRight: 5 }, !isSmallScreen && open && { display: "none" }]}
          >
            <MenuIcon />
          </IconButton>
          {currentPage && (
            <Typography variant="h6" color="inherit" noWrap>
              {currentPage}
            </Typography>
          )}

          <Box
            onClick={(e) => setAccountAnchor(e.currentTarget)}
            sx={{
              display: "flex",
              alignItems: "center",
              gap: 1,
              ml: "auto",
              px: 1,
              py: 0.5,
              borderRadius: 1,
              cursor: "pointer",
              "&:hover": { bgcolor: "rgba(255,255,255,0.1)" },
            }}
          >
            {!isSmallScreen && (
              <Typography variant="body2" color="inherit" noWrap>
                {user?.lastName} {user?.firstName} 
              </Typography>
            )}
            <Avatar sx={{ width: 32, height: 32, fontSize: 14  }}>
              {initials} 
            </Avatar>
          </Box>

          <Menu
            anchorEl={accountAnchor} 
            open={!!accountAnchor} 
            onClose={() => setAccountAnchor(null)}
            anchorOrigin={{ vertical: "bottom", horizontal: "right"}} 
            transformOrigin={{ vertical: "top", horizontal: "right" }}
          >
            <MenuItem component={Link} href="/" onClick={() => setAccountAnchor(null)}>
              <ListItemIcon><HomeIcon fontSize="small" /></ListItemIcon>
              Főoldal
            </MenuItem>
            <MenuItem component={Link} href="/search" onClick={() => setAccountAnchor(null)}>
              <ListItemIcon><SearchIcon fontSize="small" /></ListItemIcon>
              Kereső 
            </MenuItem>
            <Divider />
            <MenuItem onClick={() => { setAccountAnchor(null); handleLogout(); }}>
              <ListItemIcon><LogoutIcon fontSize="small" /></ListItemIcon>
              Kijelentkezés
            </MenuItem>
          </Menu>
        </Toolbar>
      </AppBar>
      {isSmallScreen ? (
        <MuiDrawer variant="temporary" open={open} slotProps={{ paper: { sx: { width: drawerWidth } } }}>
          {DrawerContent}
        </MuiDrawer>
      ) : (
        <Drawer variant={"permanent"} open={open}>
          {DrawerContent}
        </Drawer>
      )}
    </Box>
  );
}
