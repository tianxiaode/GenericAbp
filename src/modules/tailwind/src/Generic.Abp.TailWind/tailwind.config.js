/** @type {import('Tailwindcss').Config} */
module.exports = {
    content: [
        "./Pages/**/*.{html,js,cshtml,ts,css,scss,sass}",
        "./Views/**/*.{html,js,cshtml,ts,css,scss,sass}",
        "./Theme/**/*.{html,js,cshtml,ts,css,scss,sass}",
        "./src/**/*.{html,js,ts}",
    ],
    theme: {
        extend: {},
    },
    plugins: [
        require("daisyui"),
        require('tailwindcss-rtl')
    ],
    safelist: [
        'bg-success',
        'bg-info',
        'bg-warning',
        'bg-error',
        'text-success',
        'text-info',
        'text-warning',
        'text-error',
        "progress",
        'progress-success',
        'progress-info',
        'progress-warning',
        'progress-error',
        "progress-gray-200",
        "grid",
        "grid-cols-12",
        "input-bordered",
        "focus:border-primary",
        "focus-within:border-primary",
        "flex",
        "items-center",
        "label-text-alt",
        "text-xs",
        "gap-1",
        "gap-2",
        {
            pattern: /col-(span|start)-(\d)/,
        },
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

