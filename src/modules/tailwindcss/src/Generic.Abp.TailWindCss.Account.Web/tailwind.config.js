/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
      "./Pages/**/*.{html,js,cshtml,ts,css,scss,sass}",
      "./Views/**/*.{html,js,cshtml,ts,css,scss,sass}",
      "./Theme/**/*.{html,js,cshtml,ts,css,scss,sass}",
      "./scripts/**/*.{html,js,cshtml,ts,css,scss,sass}"
],
  theme: {
    extend: {},
  },
  plugins: [
    require("daisyui"),
    require('tailwindcss-rtl')
  ],
  safelist:[
      'bg-success',
      'bg-info',
      'bg-warning',
      'bg-error',
      'text-success',
      'text-info',
      'text-warning',
      'text-error',
      'progress-success',
      'progress-info',
      'progress-warning',
      'progress-error',
      'col-span-1',
      'col-span-2',
      'col-span-3',
      'col-span-4',
      'col-span-5',
      'col-span-6',
      'col-span-7',
      'col-span-8',
      'col-span-9',
      'col-span-10',
      'col-span-11',
      'col-span-12',
      'col-start-1',
      'col-start-2',
      'col-start-3',
      'col-start-4',
      'col-start-5',
      'col-start-6',
      'col-start-7',
      'col-start-8',
      'col-start-9',
      'col-start-10',
      'col-start-11',
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

