import Typography from '@mui/material/Typography';
import dayjs from 'dayjs';

function parseDateTime(t) {
  return dayjs(`${t.date} ${t.startTime}`);
}

export default function AppointmentList({ appointments }) {
  if (!appointments?.length) {
    return <Typography variant="body2" color="text.secondary">Nincsenek meghatározott időpontok</Typography>;
  }

  const now = dayjs();
  const sorted = [...appointments].sort((a, b) => parseDateTime(a).diff(parseDateTime(b)));
  const upcomingIndex = sorted.findIndex((t) => parseDateTime(t).isAfter(now));

  return sorted.map((ap, i) => {
    const isPast = !parseDateTime(ap).isAfter(now);
    const isUpcoming = i === upcomingIndex;
    return (
      <Typography
        key={ap.id}
        variant="body2"
        color={isPast ? 'text.disabled' : 'text.primary'}
        fontWeight={isUpcoming ? 'bold' : 'normal'}
      >
        {`${ap.date} ${ap.startTime} - ${ap.endTime}`}
        {isUpcoming && (
          <Typography component="span" variant="caption" color="primary" sx={{ ml: 1 }}>
            (következő)
          </Typography>
        )}
      </Typography>
    );
  });
}
