/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [],
  theme: {
    extend: {},
  },
 plugins: [
    require("daisyui"),
    require('tailwindcss-rtl')
  ],
  daisyui: {
    themes: [
      {
        'light': {
          "color-scheme": "light",
          "primary": "#0073E6",
          "primary-content": "#1C2025",
          "secondary": "#99A7BB",
          "secondary-content": "#303740",
          "accent": "oklch(76.76% 0.184 183.61)",
          "neutral": "#2B3440",
          "base-100": "oklch(100% 0 0)",
          "info": "#0288d1",
          "success": "#1AA251",
          "warning": "#DEA500",
          "error": "#eb0014",
          "--rounded-box": "0",
          "--rounded-btn": "0"
        },
        'dark': {
          "color-scheme": "dark",
          "primary": "#90caf9",
          "secondary": "#99A7BB",
          "accent": "oklch(74.51% 0.167 183.61)",
          "neutral": "#2a323c",
          "base-100": "#1f2937",
          "base-100": "#101418",
          "info": "#0288d1",
          "info-content": "#fff",
          "success": "#1AA251",
          "warning": "#DEA500",
          "error": "#EB0014",
          "--rounded-box": "0",
          "--rounded-btn": "0",
        }
      }
    ]
  }
}

