import { createApp } from "vue";
import App from "./App.vue";
import "./css/main.postcss";
// import Index from "./pages/Index.vue";
import { createRouter, createWebHistory } from "vue-router";
import { createPinia } from "pinia";
// import Login from "./pages/Login.vue";
import axios from "axios";
// import Admin from "./pages/Admin.vue";
import { components } from "../openapi.gen";
import { type Configs } from "./inject";

const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: "/", component: () => import("./pages/Index.vue") },
    { path: "/login", component: () => import("./pages/Login.vue") },
    {
      path: "/admin",
      component: () => import("./pages/Admin.vue"),
      meta: { requiresAuth: true },
    },
    {
      path: "/:pathMatch(.*)*",
      name: "NotFoundRedirect",
      redirect: "/",
    },
  ],
});

async function checkLogin() {
  let responds = null;
  try {
    responds = await axios.get("api/auth/status");
  } catch (e) {
    console.log(e);
  }
  return responds != null && responds.data["isLoggin"];
}

router.beforeEach(async (to, _) => {
  if (to.meta.requiresAuth) {
    let login = await checkLogin();
    if (!login) {
      console.log("Not Authenticated!");
      return "/login";
    }
  }
});

(async () => {
  const config: components["schemas"]["ConfigDto"] = (
    await axios.get("api/config")
  ).data;
  createApp(App)
    .use(router)
    .use(createPinia())
    .provide<Configs>("configs", config)
    .mount("#app");
})();
