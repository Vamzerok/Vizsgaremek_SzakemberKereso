"use client";

import { useState, useEffect, useRef } from "react";
import useSWR from "swr";

import Stack from "@mui/material/Stack";
import CircularProgress from "@mui/material/CircularProgress";
import TextField from "@mui/material/TextField";
import Autocomplete from "@mui/material/Autocomplete";

import { genericFetcher } from "@/utils/fetcher";

export default function WorkLocationSelector({ initialSettlement, onSettlementChange, required = false }) {
  const [selectedCounty, setSelectedCounty] = useState(() => initialSettlement?.countyName ?? null);
  const [selectedSettlement, setSelectedSettlement] = useState(null);
  const initialized = useRef(false);

  const { data: counties = [], isLoading: countiesLoading } = useSWR(
    "/api/Settlements/allCounties",
    genericFetcher
  );

  const { data: settlements = [], isLoading: settlementsLoading } = useSWR(
    selectedCounty ? `/api/Settlements/allByCounty?countyName=${encodeURIComponent(selectedCounty)}` : null,
    genericFetcher
  );

  useEffect(() => {
    if (initialized.current || !initialSettlement || !settlements.length) return;
    const match = settlements.find((s) => s.id === initialSettlement.id);
    if (!match) return;
    // eslint-disable-next-line react-hooks/set-state-in-effect
    setSelectedSettlement(match);
    onSettlementChange(match);
    initialized.current = true;
  }, [settlements, initialSettlement, onSettlementChange]);

  function handleCountyChange(county) {
    setSelectedCounty(county);
    setSelectedSettlement(null);
    onSettlementChange(null);
  }

  function handleSettlementChange(settlement) {
    setSelectedSettlement(settlement);
    onSettlementChange(settlement);
  }

  return (
    <Stack spacing={2}>
      <Autocomplete
        options={counties}
        value={selectedCounty}
        onChange={(_, county) => handleCountyChange(county)}
        loading={countiesLoading}
        renderInput={(params) => (
          <TextField
            {...params}
            label="Vármegye"
            required={required}
            slotProps={{
              input: {
                ...params.InputProps,
                endAdornment: (
                  <>
                    {countiesLoading && <CircularProgress size={16} />}
                    {params.InputProps.endAdornment}
                  </>
                ),
              },
            }}
          />
        )}
        fullWidth
      />
      <Autocomplete
        options={settlements}
        value={selectedSettlement}
        disabled={!selectedCounty}
        loading={settlementsLoading}
        isOptionEqualToValue={(opt, val) => opt.id === val?.id}
        onChange={(_, settlement) => handleSettlementChange(settlement)}
        getOptionLabel={(s) => (s ? `${s.name} (${s.postalCode})`  : "")}
        renderInput={(params) => (
          <TextField
            {...params}
            required={required}
            label="Település"
            slotProps={{
              input: {
                ...params.InputProps,
                endAdornment: (
                  <>
                    {settlementsLoading && <CircularProgress size={16} />}
                    {params.InputProps.endAdornment}
                  </>
                ),
              },
            }}
          />
        )}
        fullWidth
      />
    </Stack>
  );
}
