import { createApp } from "vue";
import App from "./App.vue";
import "./css/main.postcss";
import Index from "./pages/Index.vue";
import { createRouter, createWebHistory } from "vue-router";
import { createPinia } from "pinia";
import Login from "./pages/Login.vue";
import axios from "axios";
import Admin from "./pages/Admin.vue";

const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: "/", component: Index },
    { path: "/login", component: Login },
    {
      path: "/admin",
      component: Admin,
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
    responds = await axios.get(API_URL + "auth/status");
  } catch (e) {
    console.log(e);
  }
  return (responds != null) && responds.data["isLoggin"];
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

createApp(App).use(router).use(createPinia()).mount("#app");
