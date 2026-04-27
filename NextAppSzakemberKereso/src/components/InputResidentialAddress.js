'use client';

import { forwardRef, useImperativeHandle, useState } from 'react';

import Stack from '@mui/material/Stack';
import TextField from '@mui/material/TextField';
import Typography from '@mui/material/Typography';

const EMPTY_LOCATION = {
  settlement: { postalCode: '', name: '' },
  streetAddress: '',
};

const InputResidentialAddress = forwardRef(function InputResidentialAddress(_, ref) {
  const [value, setValue] = useState(EMPTY_LOCATION);

  useImperativeHandle(ref, () => ({
    getValue: () => ({
      settlement: {
        postalCode: Number(value.settlement.postalCode),
        name: value.settlement.name,
      },
      streetAddress: value.streetAddress.trim(),
    }),
  }), [value]);

  function updateSettlement(field, fieldValue) {
    setValue((v) => ({ ...v, settlement: { ...v.settlement, [field]: fieldValue } }));
  }

  return (
    <Stack spacing={1.5}>
      <Typography variant="caption" color="text.secondary">Helyszín</Typography>

      <Stack direction={{ xs: 'column', sm: 'row' }} spacing={1}>
        <TextField
          label="Irányítószám"
          type="number"
          size="small"
          required
          value={value.settlement.postalCode}
          onChange={(e) => updateSettlement('postalCode', e.target.value)}
          sx={{ flex: 1 }}
        />
        <TextField
          label="Település"
          size="small"
          required
          value={value.settlement.name}
          onChange={(e) => updateSettlement('name', e.target.value)}
          sx={{ flex: 2 }}
          slotProps={{ htmlInput: { maxLength: 100 } }}
        />
      </Stack>

      <TextField
        label="Utca, házszám"
        placeholder="pl. Zrínyi Ilona utca 12"
        size="small"
        required
        fullWidth
        value={value.streetAddress}
        onChange={(e) => setValue((v) => ({ ...v, streetAddress: e.target.value }))}
        helperText="Formátum: [utcanév] [közterület típusa] [házszám] (pl.: Kossuth Lajos utca 15)"
        slotProps={{ htmlInput: { maxLength: 175 } }}
      />
    </Stack>
  );
});

export default InputResidentialAddress;
