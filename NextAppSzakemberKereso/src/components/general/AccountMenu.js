'use client';

import { useState } from 'react';
import { useRouter, usePathname } from 'next/navigation';

import {useMediaQuery, useTheme} from '@mui/material'

import { Divider, ListItemIcon, Box, List, ListItem, Avatar, 
  Skeleton, ListItemText, Stack, Typography, Menu, MenuItem, Drawer, } from '@mui/material';

import KeyboardArrowDownIcon from '@mui/icons-material/KeyboardArrowDown';
import PersonIcon from '@mui/icons-material/Person';
import LogoutIcon from '@mui/icons-material/Logout';

import { GENERIC_NAV_OPTIONS, EXPERT_NAV_OPTIONS } from '@/utils/nav';
import useUser from '@/hooks/useUser';
import { isUserAnExpert } from '@/utils/user';

export default function AccountMenu() {
  const [anchorEl, setAnchorEl] = useState(null);
  const { user, isLoading, error, mutate } = useUser();
  const isMobile = useMediaQuery(useTheme().breakpoints.down('sm'));

  const router = useRouter();
  const pathname = usePathname();

  if (isLoading) {
    return <Skeleton variant="circular" width={32} height={32} />;
  }

  if (error || !user) {
    return (
      <Stack
        sx={{ cursor: 'pointer', userSelect: 'none' }}
        direction="row"
        alignItems="center"
        onClick={() => router.push(`/auth/login?from=${encodeURIComponent(pathname)}`)}
        spacing={1}
      >
        { !isMobile && (
          <Typography>
            Bejelentkezés
          </Typography>
        )}
        <Avatar
          sx={{ width: 32, height: 32, cursor: 'pointer', bgcolor: 'grey.400' }}
        >
          <PersonIcon sx={{ fontSize: 20 }} />
        </Avatar>
      </Stack>
    );
  }

  const navOptions = isUserAnExpert(user) ? EXPERT_NAV_OPTIONS : GENERIC_NAV_OPTIONS;

  async function handleLogout() {
    setAnchorEl(null);
    await fetch('/api/logout', { method: 'POST', credentials: 'include' });
    await mutate(null, false);
    router.push('/');
  }

  function handleNavigate(href) {
    setAnchorEl(null);
    router.push(href);
  }

  const main = (
    <Stack
      sx={{ cursor: 'pointer', userSelect: 'none' }}
      direction="row"
      alignItems="center"
      onClick={(e) => setAnchorEl(e.currentTarget)}
      spacing={1}
    >
      <Typography variant="body2" sx={{ display: { xs: 'none', sm: 'block' } }}>
        {user.lastName} {user.firstName}
      </Typography>

      <Avatar sx={{ width: 32, height: 32, fontSize: '0.8rem' }}>
        {user.lastName[0]}{user.firstName[0]}
      </Avatar>

      <KeyboardArrowDownIcon sx={{ fontSize: 18, display: { xs: 'none', sm: 'block' } }} />
    </Stack>
  );

  //MUI menu really hates fragments as a child for some reason, peak solution can be seen right here
  const menuItems = [
    <ListItem key={0} sx={{ py: 2, borderBottom: '1px solid', borderColor: 'divider' }}>
      <ListItemText
        primary={`${user.lastName} ${user.firstName}`}
        secondary={isUserAnExpert(user) ? 'Szakember' : 'Felhasználó'}
      />
    </ListItem>,

    ...navOptions.map((o) => (
      <MenuItem key={o.href} onClick={() => handleNavigate(o.href)}>
        <ListItemIcon>{o.icon}</ListItemIcon>
        {o.text}
      </MenuItem>
    )),

    <Divider key="divider" />,

    <MenuItem key="logout" onClick={handleLogout}>
      <ListItemIcon><LogoutIcon fontSize="small" /></ListItemIcon>
      Kijelentkezés
    </MenuItem>,
  ];

  return (
    <Box>
      {main}

      {isMobile ? (
        <Drawer
          slotProps={{ paper: { sx: { width: 240}} }}
          open={Boolean(anchorEl)} 
          anchor="right" 
          onClose={() => setAnchorEl(null)} 
        >
          <List disablePadding>
            {menuItems}
          </List>
        </Drawer>
      ) : (
        <Menu
          open={Boolean(anchorEl)}
          anchorEl={anchorEl}
          anchorOrigin={{ horizontal: 'right', vertical: 'bottom'}}
          disableScrollLock 
          transformOrigin={{ horizontal: 'right', vertical: 'top'}}
          slotProps={{ paper: { elevation: 2, sx: { minWidth: 180} } }}
          onClose={() => setAnchorEl(null)} 
        >
          {menuItems}
        </Menu>
      )}
    </Box>
  );
}
