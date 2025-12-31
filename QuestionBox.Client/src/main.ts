import { createApp } from "vue";
import App from "./App.vue";
import "./css/main.postcss";
import Index from "./pages/Index.vue";
import { createRouter, createWebHistory } from "vue-router";
import { createPinia } from "pinia";
import Login from "./pages/Login.vue";

const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: "/", component: Index },
    { path: "/login", component: Login },
  ],
});

createApp(App).use(router).use(createPinia()).mount("#app");
