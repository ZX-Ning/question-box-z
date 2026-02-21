// vite.config.ts
import { defineConfig } from "file:///workspaces/question-box-z/QuestionBox.Client/node_modules/.pnpm/vite@5.4.0_@types+node@22.1.0/node_modules/vite/dist/node/index.js";
import vue from "file:///workspaces/question-box-z/QuestionBox.Client/node_modules/.pnpm/@vitejs+plugin-vue@5.1.2_vite@5.4.0_@types+node@22.1.0__vue@3.4.36_typescript@5.5.4_/node_modules/@vitejs/plugin-vue/dist/index.mjs";
import tailwindcss from "file:///workspaces/question-box-z/QuestionBox.Client/node_modules/.pnpm/tailwindcss@3.4.8/node_modules/tailwindcss/lib/index.js";
import autoprefixer from "file:///workspaces/question-box-z/QuestionBox.Client/node_modules/.pnpm/autoprefixer@10.4.20_postcss@8.4.41/node_modules/autoprefixer/lib/autoprefixer.js";
var vite_config_default = defineConfig({
  plugins: [vue()],
  css: {
    postcss: {
      plugins: [tailwindcss(), autoprefixer()]
    }
  },
  server: {
    proxy: {
      "/api": "http://localhost:5000"
    }
  }
});
export {
  vite_config_default as default
};
//# sourceMappingURL=data:application/json;base64,ewogICJ2ZXJzaW9uIjogMywKICAic291cmNlcyI6IFsidml0ZS5jb25maWcudHMiXSwKICAic291cmNlc0NvbnRlbnQiOiBbImNvbnN0IF9fdml0ZV9pbmplY3RlZF9vcmlnaW5hbF9kaXJuYW1lID0gXCIvd29ya3NwYWNlcy9xdWVzdGlvbi1ib3gtei9RdWVzdGlvbkJveC5DbGllbnRcIjtjb25zdCBfX3ZpdGVfaW5qZWN0ZWRfb3JpZ2luYWxfZmlsZW5hbWUgPSBcIi93b3Jrc3BhY2VzL3F1ZXN0aW9uLWJveC16L1F1ZXN0aW9uQm94LkNsaWVudC92aXRlLmNvbmZpZy50c1wiO2NvbnN0IF9fdml0ZV9pbmplY3RlZF9vcmlnaW5hbF9pbXBvcnRfbWV0YV91cmwgPSBcImZpbGU6Ly8vd29ya3NwYWNlcy9xdWVzdGlvbi1ib3gtei9RdWVzdGlvbkJveC5DbGllbnQvdml0ZS5jb25maWcudHNcIjtpbXBvcnQgeyBkZWZpbmVDb25maWcgfSBmcm9tIFwidml0ZVwiO1xuaW1wb3J0IHZ1ZSBmcm9tIFwiQHZpdGVqcy9wbHVnaW4tdnVlXCI7XG5pbXBvcnQgdGFpbHdpbmRjc3MgZnJvbSBcInRhaWx3aW5kY3NzXCI7XG5pbXBvcnQgYXV0b3ByZWZpeGVyIGZyb20gXCJhdXRvcHJlZml4ZXJcIjtcbi8vIGh0dHBzOi8vdml0ZWpzLmRldi9jb25maWcvXG5cblxuZXhwb3J0IGRlZmF1bHQgZGVmaW5lQ29uZmlnKHtcbiAgcGx1Z2luczogW3Z1ZSgpXSxcbiAgY3NzOiB7XG4gICAgcG9zdGNzczoge1xuICAgICAgcGx1Z2luczogW3RhaWx3aW5kY3NzKCksIGF1dG9wcmVmaXhlcigpXSxcbiAgICB9LFxuICB9LFxuICBzZXJ2ZXI6IHtcbiAgICBwcm94eToge1xuICAgICAgXCIvYXBpXCI6IFwiaHR0cDovL2xvY2FsaG9zdDo1MDAwXCIsXG4gICAgfSxcbiAgfSxcbn0pO1xuIl0sCiAgIm1hcHBpbmdzIjogIjtBQUF5VCxTQUFTLG9CQUFvQjtBQUN0VixPQUFPLFNBQVM7QUFDaEIsT0FBTyxpQkFBaUI7QUFDeEIsT0FBTyxrQkFBa0I7QUFJekIsSUFBTyxzQkFBUSxhQUFhO0FBQUEsRUFDMUIsU0FBUyxDQUFDLElBQUksQ0FBQztBQUFBLEVBQ2YsS0FBSztBQUFBLElBQ0gsU0FBUztBQUFBLE1BQ1AsU0FBUyxDQUFDLFlBQVksR0FBRyxhQUFhLENBQUM7QUFBQSxJQUN6QztBQUFBLEVBQ0Y7QUFBQSxFQUNBLFFBQVE7QUFBQSxJQUNOLE9BQU87QUFBQSxNQUNMLFFBQVE7QUFBQSxJQUNWO0FBQUEsRUFDRjtBQUNGLENBQUM7IiwKICAibmFtZXMiOiBbXQp9Cg==
