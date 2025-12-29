import tailwind_forms from "@tailwindcss/forms";
import { type Config } from "tailwindcss";

const config: Config = {
  content: ["./index.html", "./src/**/*.{js,ts,jsx,tsx,vue}"],
  darkMode: 'selector',
  theme: {
    screens: {
      "lg": {
        "raw": '(min-width: 1024px) and (min-height: 580px)'
      }
    },
    extend: {
      colors: {},
      keyframes: {
        slideDown: {
          from: { height: "0" },
          to: { height: "var(--radix-accordion-content-height)" },
        },
        slideUp: {
          from: { height: "var(--radix-accordion-content-height)" },
          to: { height: "0" },
        },
        overlayShow: {
          from: { opacity: "0" },
          to: { opacity: "1" },
        },
        contentShow: {
          from: {
            opacity: "0",
            transform: "translate(-50%, -48%) scale(0.96)",
          },
          to: { opacity: "1", transform: "translate(-50%, -50%) scale(1)" },
        },
      },
      animation: {
        slideDown: "slideDown 100ms cubic-bezier(0.87, 0, 0.13, 1)",
        slideUp: "slideUp 100ms cubic-bezier(0.87, 0, 0.13, 1)",
        overlayShow: "overlayShow 150ms cubic-bezier(0.16, 1, 0.3, 1)",
        contentShow: "contentShow 150ms cubic-bezier(0.16, 1, 0.3, 1)",
      },
    },
    fontFamily: {
      sans: ["'Noto Sans'", "'Noto Sans SC'", "'Noto Color Emoji'", "sans"],
      mono: ["'Roboto Mono'", "monospace"],
    },
  },
  plugins: [tailwind_forms()],
};

export default config;
