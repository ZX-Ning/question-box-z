import { defineConfig } from "vite";
import vue from "@vitejs/plugin-vue";
import tailwindcss from "tailwindcss";
import autoprefixer from "autoprefixer";
import JSON5 from "json5";
import fs from "node:fs";
import path from "node:path"
// https://vitejs.dev/config/

const configFile = path.join(__dirname, "../AppConfig.jsonc")
const config = JSON5.parse(fs.readFileSync(configFile, "utf8"));

export default defineConfig({
  define: {
    CONFIG: config,
    API_URL: "'api/'"
  },
  plugins: [vue()],
  css: {
    postcss: {
      plugins: [tailwindcss(), autoprefixer()],
    },
  },
  server: {
    proxy: {
      "/api": config.ServerDevUrl,
    },
  },
});
