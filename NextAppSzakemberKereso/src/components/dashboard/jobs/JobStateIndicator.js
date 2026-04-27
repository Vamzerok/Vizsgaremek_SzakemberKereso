import Chip from '@mui/material/Chip';
import CheckCircleIcon from '@mui/icons-material/CheckCircle';
import HourglassEmptyIcon from '@mui/icons-material/HourglassEmpty';
import MoveToInboxIcon from '@mui/icons-material/MoveToInbox';
import BuildIcon from '@mui/icons-material/Build';
import CancelIcon from '@mui/icons-material/Cancel';

import { JOB_STATES } from '@/utils/jobState';

export const USER_STATE_CONFIG = {
  [JOB_STATES.Pending]: { label: 'Ajánlatra várakozás', color: 'warning', Icon: HourglassEmptyIcon },
  [JOB_STATES.Offered]: { label: 'Ajánlat érkezett', color: 'info',    Icon: MoveToInboxIcon },
  [JOB_STATES.Accepted]: { label: 'Munka folyamatban', color: 'primary', Icon: BuildIcon },
  [JOB_STATES.Completed]: { label: 'Munka befejezve', color: 'success', Icon: CheckCircleIcon },
  [JOB_STATES.Cancelled]: { label: 'Munka lemondva', color: 'error',   Icon: CancelIcon },
};

export const EXPERT_STATE_CONFIG = {
  [JOB_STATES.Pending]: { label: 'Ajánlat készítése', color: 'warning', Icon: MoveToInboxIcon },
  [JOB_STATES.Offered]: { label: 'Várakozás az ajánlat elfogadására',  color: 'info',    Icon: HourglassEmptyIcon },
  [JOB_STATES.Accepted]: { label: 'Munka folyamatban', color: 'primary', Icon: BuildIcon },
  [JOB_STATES.Completed]: { label: 'Munka befejezve', color: 'success', Icon: CheckCircleIcon },
  [JOB_STATES.Cancelled]: { label: 'Munka lemondva', color: 'error',   Icon: CancelIcon },
};

export default function JobStateIndicator({ status, config, sx }) {
  const entry = config[status];
  if (!entry) return null;
  const { label, color, Icon } = entry;
  return (
    <Chip
      icon={<Icon fontSize="small" />}
      label={label}
      color={color}
      size="small"
      sx={sx}
    />
  );
}
