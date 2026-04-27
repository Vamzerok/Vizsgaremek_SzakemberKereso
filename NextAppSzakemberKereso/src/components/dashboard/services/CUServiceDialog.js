"use client";

import { useState, useRef } from "react";

import { PricingType } from "@/utils/pricing";

import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import Dialog from "@mui/material/Dialog";
import Stack from "@mui/material/Stack";
import InputAdornment from "@mui/material/InputAdornment";
import TextField from "@mui/material/TextField";
import DialogTitle from "@mui/material/DialogTitle";
import Alert from "@mui/material/Alert";
import CircularProgress from "@mui/material/CircularProgress";
import Autocomplete from "@mui/material/Autocomplete";
import Button from "@mui/material/Button";
import FormControlLabel from "@mui/material/FormControlLabel";
import Switch from "@mui/material/Switch"

const FORM_TEMPLATE = {
  specialty: null,
  name: "",
  description: "",
  pricingType: PricingType.Fixed,
  fixedPrice: "",
  unitPrice: "",
  unitName: "",
};

//CU = Create + Update (ik I'm the best at naming things)
export default function CUServiceDialog({ open, onClose, specialties = [], onSuccess, service = null, initialSpecialty = null }) {
  const isEditMode = !!service;

  const [formData, setForm] = useState(FORM_TEMPLATE);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState(null);

  const isFormPopulated = useRef(false) //clever lil trick

  if(open){ 
    //good luck... i don't even know, it works... maybe
    if (isEditMode && !isFormPopulated.current) {
      isFormPopulated.current = true;
      setForm(setupForm());
    } else if (!isEditMode && !isFormPopulated.current && initialSpecialty) {
      isFormPopulated.current = true;
      setForm((prev) => ({ ...prev, specialty: initialSpecialty }));
    }
  }

  function setupForm(){
    const p = service.pricing;
    return ({
      specialty: specialties.find((s) => s.id === service.expertSpecialtyId),
      name: service.name,
      description: service.description ?? "",
      pricingType: p?.pricingType ?? PricingType.Fixed,
      fixedPrice: p?.fixedPrice != null ? String(p.fixedPrice) : "",
      unitPrice: p?.unitPrice != null ? String(p.unitPrice) : "",
      unitName: p?.unitName ?? "",
    });
  }

  function set(field, value) {
    setForm((prev) => ({ ...prev, [field]: value }));
  }

  async function handleSave() {
    if (!formData.specialty || !formData.name.trim()) return;
    setSaving(true);
    setError(null);

    let pricing = null;

    switch (formData.pricingType) {
      case PricingType.Fixed:
        pricing = { pricingType: PricingType.Fixed, fixedPrice: parseFloat(formData.fixedPrice) || 0 }
        break;
      case PricingType.FixedAndUnitBased:
        pricing = {
          pricingType: PricingType.FixedAndUnitBased,
          fixedPrice: parseFloat(formData.fixedPrice) || 0,
          unitPrice: parseFloat(formData.unitPrice) || 0,
          unitName: formData.unitName.trim(),
        };
        break;
      default:
        setError("Ismeretlen árazási típus.");
        setSaving(false);
        return;
    }

    const mode = isEditMode ? {
      method: "PUT",
      url: `/api/Experts/me/occupations/${formData.specialty.occupationId}/services/${service.id}`,
      errorMessage: "A szolgáltatás frissítése sikertelen.",
    } : {
      method: "POST",
      url: `/api/Experts/me/occupations/${formData.specialty.occupationId}/services`,
      errorMessage: "A szolgáltatás hozzáadása sikertelen.",
    } 

    try {
      const res = await fetch(mode.url, {
        method: mode.method, 
        credentials: "include",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          name: formData.name.trim(),
          description: formData.description.trim() || undefined,
          pricing,
        }),
      });

      if (!res.ok) throw new Error(mode.errorMessage);

      await onSuccess();
      handleClose();
    } catch (e) {
      setError(e.message);
    } finally {
      setSaving(false);
    }
  }

  function handleClose(){
    if (saving) return;
    setForm(FORM_TEMPLATE);
    isFormPopulated.current = false;
    setError(null);
    onClose();
  }

  function isFormValid(){
    if (!formData.specialty || !formData.name.trim()) return false;

    switch (formData.pricingType) {
      case PricingType.Fixed:
        return !!formData.fixedPrice;
      case PricingType.FixedAndUnitBased:
        return !!formData.unitPrice && !!formData.unitName.trim();
      default:
        return false;
    }
  }

  return (
    <Dialog open={open} onClose={handleClose} fullWidth maxWidth="sm">
      <DialogTitle>{isEditMode ? "Szolgáltatás szerkesztése" : "Új szolgáltatás"}</DialogTitle>
      <DialogContent sx={{ pt: "16px", overflow: "visible" }}>
        <Stack spacing={2.5}>
          {error && <Alert severity="error">{error}</Alert>}

          <Autocomplete
            options={specialties}
            getOptionLabel={(s) => `${s.name} (FEOR ${s.occupationId})`}
            value={formData.specialty}
            onChange={ (_, v) => set("specialty", v) }
            disabled={ isEditMode }
            renderInput={(params) => (
              <TextField {...params} label="Foglalkozás" required />
            )}
          />

          <TextField
            label="Szolgáltatás neve"
            value={formData.name}
            onChange={(e) => set("name", e.target.value)}
            required
            fullWidth
            slotProps={{ htmlInput: { maxLength: 100 } }}
          />

          <TextField
            label="Leírás"
            value={formData.description}
            onChange={(e) => set("description", e.target.value)}
            multiline
            minRows={2}
            fullWidth
            slotProps={{ htmlInput: { maxLength: 512 } }}
          />

          <TextField
            label="Alapár"
            type="number"
            value={formData.fixedPrice}
            onChange={(e) => set("fixedPrice", e.target.value)}
            required={formData.pricingType === PricingType.Fixed}
            fullWidth
            slotProps={{ input: { endAdornment: <InputAdornment position="end">Ft</InputAdornment> } }}
          />

          <FormControlLabel
            label="Egységalapú árazás"
            control={
              <Switch
                checked={formData.pricingType === PricingType.FixedAndUnitBased}
                onChange={(e) => set("pricingType", e.target.checked ? PricingType.FixedAndUnitBased : PricingType.Fixed)}
              />
            }
          />

          {formData.pricingType === PricingType.FixedAndUnitBased && (
            <Stack direction={{ xs: "column", sm: "row" }} spacing={2}>
              <TextField
                label="Egységár"
                type="number"
                value={formData.unitPrice}
                onChange={(e) => set("unitPrice", e.target.value)}
                required
                fullWidth
                slotProps={{ input: { endAdornment: <InputAdornment position="end">Ft</InputAdornment> } }}
              />
              <TextField
                label="Egység neve"
                placeholder="pl. óra, nap"
                value={formData.unitName}
                onChange={(e) => set("unitName", e.target.value)}
                required
                fullWidth
                slotProps={{ htmlInput: { maxLength: 64 } }}
              />
            </Stack>
          )}
        </Stack>
      </DialogContent>
      <DialogActions>
        <Button onClick={handleClose} disabled={saving}>Mégse</Button>
        <Button
          onClick={handleSave}
          variant="contained"
          disabled={!isFormValid() || saving}
          startIcon={saving ? <CircularProgress size={16} color="inherit" /> : null}
        >
          {isEditMode ? "Mentés" : "Hozzáadás"}
        </Button>
      </DialogActions>
    </Dialog>
  );
}
