import { forwardRef, useImperativeHandle, useMemo, useState } from 'react';

import Box from '@mui/material/Box';
import Chip from '@mui/material/Chip';
import IconButton from '@mui/material/IconButton';
import Stack from '@mui/material/Stack';
import TextField from '@mui/material/TextField';
import Typography from '@mui/material/Typography';
import AddIcon from '@mui/icons-material/Add';
import DeleteOutlineIcon from '@mui/icons-material/DeleteOutline';

const EMPTY_INTERVAL = { date: '', startTime: '', endTime: '' };

const DateTimeSpecifier = forwardRef(function DateTimeSpecifier({ value: valueProp, onChange, disabled = false }, ref) {
  const isControlled = valueProp !== undefined;
  const [internalValue, setInternalValue] = useState([]);

  const value = useMemo(
    () => isControlled ? (valueProp ?? []) : internalValue,
    [isControlled, valueProp, internalValue]
  );

  useImperativeHandle(ref, () => ({
    getValue: () => value,
  }), [value]);

  if (disabled) {
    return (
      <Stack spacing={0.5}>
        {value.map((row, index) => (
          <Stack key={index} direction="row" alignItems={{ sm: 'center' }} spacing={1}>
            <Chip label={row.date} size="small" />
            <Chip label={`${row.startTime} - ${row.endTime}`} size="small" />
          </Stack>
        ))}
      </Stack>
    );
  }

  const displayValue = value.length === 0 ? [{ ...EMPTY_INTERVAL }] : value;

  function handleChange(newValue) {
    if (!isControlled) setInternalValue(newValue);
    onChange?.(newValue);
  }

  function updateRow(index, field, fieldValue) {
    const updated = [...displayValue];
    updated[index] = { ...displayValue[index], [field]: fieldValue };
    handleChange(updated);
  }

  function addRow() {
    handleChange([...displayValue, { ...EMPTY_INTERVAL }]);
  }

  function deleteRow(index) {
    handleChange(displayValue.filter((_, i) => i !== index));
  }

  return (
    <Stack spacing={1}>
      {displayValue.map((row, index) => (
        <Stack key={index} direction={{ xs: 'column', sm: 'row' }} alignItems={{ sm: "center" }} spacing={1}>
          <TextField
            label="Dátum"
            type="date"
            value={row.date}
            size="small" 
            sx={{ flex: 2 }}
            slotProps={{ inputLabel: { shrink: true } }} 
            onChange={(e) => updateRow(index, 'date', e.target.value)} 
          />
          <Stack direction="row" alignItems="center" spacing={1} sx={{ flex: 3 }}>
            <TextField
              label="Kezdés"
              type="time" 
              value={row.startTime}
              size="small" 
              sx={{ flex: 1 }} 
              slotProps={{ inputLabel: { shrink: true } }}
              onChange={(e) => updateRow(index, 'startTime', e.target.value)}
            />
            <Typography variant="body2" color="text.secondary">-</Typography>
            <TextField
              label="Befejezés"
              type="time"
              value={row.endTime} 
              size="small" 
              sx={{ flex: 1 }} 
              slotProps={{ inputLabel: { shrink: true } }}
              onChange={(e) => updateRow(index, 'endTime', e.target.value)}
            />
            <IconButton
              size="small"
              color="error"
              disabled={displayValue.length === 1}
              onClick={() => deleteRow(index)}
            >
              <DeleteOutlineIcon fontSize="small" />
            </IconButton>
          </Stack>
        </Stack>
      ))}
      <Box>
        <IconButton size="small" color="primary" onClick={addRow}>
          <AddIcon />
        </IconButton>
      </Box>
    </Stack>
  );
});

export default DateTimeSpecifier;
