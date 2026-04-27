"use client";

import { useState } from "react";

import useSWR from "swr";
import { genericFetcher } from "@/utils/fetcher";

import Alert from "@mui/material/Alert";
import Autocomplete from "@mui/material/Autocomplete";
import Button from "@mui/material/Button";
import CircularProgress from "@mui/material/CircularProgress";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import DialogTitle from "@mui/material/DialogTitle";
import TextField from "@mui/material/TextField";


export default function AddOccupationDialog({ open, onClose, existingOccupationIds = [], onSuccess }) {
  const [selected, setSelected] = useState(null);
  const [adding, setAdding] = useState(false);
  const [error, setError] = useState(null);

  const { data: allOccupations } = useSWR(open ? "/api/Occupations" : null, genericFetcher);

  const availableOccupations =
    allOccupations?.filter((o) => !existingOccupationIds.includes(o.id)) ?? [];

  async function handleAdd() {
    if (!selected) return;
    setAdding(true);
    setError(null);
    try {
      const res = await fetch(
        `/api/Experts/me/occupations?occupationId=${selected.id}`,
        { method: "POST", credentials: "include" }
      );
      if (!res.ok) throw new Error("A foglalkozás hozzáadása sikertelen.");
      await onSuccess();
      handleClose();
    } catch (e) {
      setError(e.message);
    } finally {
      setAdding(false);
    }
  }

  function handleClose() {
    if (adding) return;
    setSelected(null);
    setError(null);
    onClose();
  }

  return (
    <Dialog open={open} onClose={handleClose} fullWidth maxWidth="sm">
      <DialogTitle>Foglalkozás felvétele</DialogTitle>
      <DialogContent sx={{ pt: "16px", overflow: "visible" }}>
        {error && (
          <Alert severity="error" sx={{ mb: 2 }}>
            {error}
          </Alert>
        )}
        <Autocomplete
          options={availableOccupations}
          getOptionLabel={(o) => `${o.name} (FEOR ${o.id})`}
          value={selected}
          onChange={(_, v) => setSelected(v)}
          loading={open && !allOccupations}
          renderInput={(params) => (
            <TextField
              {...params}
              label="Foglalkozás"
              placeholder="Keresés név vagy FEOR szám alapján"
            />
          )}
        />
      </DialogContent>
      <DialogActions>
        <Button onClick={handleClose} disabled={adding}>
          Mégse
        </Button>
        <Button
          onClick={handleAdd}
          variant="contained"
          disabled={!selected || adding}
          startIcon={adding ? <CircularProgress size={16} color="inherit" /> : null}
        >
          Hozzáadás
        </Button>
      </DialogActions>
    </Dialog>
  );
}
