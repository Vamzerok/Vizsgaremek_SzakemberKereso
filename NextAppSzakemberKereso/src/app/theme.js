"use client";

import { createTheme } from "@mui/material/styles";
import { blue } from "@mui/material/colors";

let theme = createTheme({
  cssVariables: true,

  mixins: {
    //can't put this into components: MuiAppBar: styleOverrides, because that can't be referenced 
    toolbar: { minHeight: 64 },
  },

  palette: {
    primary: {
      main: blue[700],
    },
    background: {
      default: "#f0f4f8",
      paper: "#ffffff",
    },
  },

  components: {
    MuiAppBar: {
      defaultProps: {
        elevation: 0,
        color: "inherit",
      },
      styleOverrides: {
        root: {
          borderRadius: 0,
          backgroundColor: "#ffffff",
          borderBottom: "1px solid #e0e7ef",
        },
      },
    },
    MuiButton: {
      styleOverrides: {
        root: {
          textTransform: "none",
          borderRadius: 6,
          fontWeight: 500,
        },
      },
    },
    MuiCard: {
      defaultProps: {
        elevation: 0,
      },
      styleOverrides: {
        root: {
          border: "1px solid #e0e7ef",
          borderRadius: 8,
        },
      },
    },
    MuiPaper: {
      styleOverrides: {
        root: {
          borderRadius: 8,
        },
      },
    },
    MuiChip: {
      styleOverrides: {
        root: {
          borderRadius: 6,
        },
      },
    },
    MuiDrawer: {
      styleOverrides: {
        paper: {
          borderRight: "1px solid #e0e7ef",
        },
      },
    },
  },
});

export default theme;
