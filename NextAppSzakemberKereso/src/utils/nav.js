import WorkHistoryIcon from '@mui/icons-material/WorkHistory';
import PersonIcon from '@mui/icons-material/Person';
import EngineeringIcon from '@mui/icons-material/Engineering';
import BuildIcon from '@mui/icons-material/Build';

export const GENERIC_NAV_OPTIONS = [
  { text: 'Profil beállítások', icon: <PersonIcon />, href: '/dashboard/profile' },
  { text: 'Munkák', icon: <WorkHistoryIcon />, href: '/dashboard/jobs' },
];

export const EXPERT_NAV_OPTIONS = [
  { text: 'Profil beállítások', icon: <PersonIcon />, href: '/dashboard/profile' },
  { text: 'Foglalkozások', icon: <EngineeringIcon />, href: '/dashboard/occupations' },
  { text: 'Szolgáltatások', icon: <BuildIcon />, href: '/dashboard/services' },
  { text: 'Munkák', icon: <WorkHistoryIcon />, href: '/dashboard/jobs' },
];
