<script setup lang="ts">
import { useRouter } from "vue-router";
import Button from "../components/BasicComponents/Button.vue";
import CenterADiv from "../layouts/CenterADiv.vue";
import axios from "axios";
import { Ref, ref } from "vue";

const router = useRouter();
let credential = ref({
  name: "",
  password: "",
});

let loginResult: Ref<"Error" | "Success" | "Pending" | null> = ref(null);
let errorMsg = ref("");

const goBack = () => {
  router.push("/");
};

const onLogin = async () => {
  loginResult.value = "Pending";
  const loginUrl = API_URL + "auth/login";
  try {
    console.log(JSON.stringify(credential.value));
    await axios.post(loginUrl, credential.value);
    loginResult.value = "Success";
    router.push("admin");
  } catch (err) {
    loginResult.value = "Error";
    console.log((err as Error).message);
    if (axios.isAxiosError(err)) {
      // console.log(JSON.stringify(err.response))
      if (err.response?.status === 401) {
        errorMsg.value = "Login failed, please check your credential";
      }
    }
    if (!errorMsg.value) {
      errorMsg.value = "Sorry, unknown error happened.";
    }
  }
};
</script>

<template>
  <CenterADiv>
    <!-- Styles copy from Preline UI -->
    <divs
      :class="[
        'flex flex-col',
        'space-y-3 p-5 flex-auto max-h-[300px] max-w-[500px] w-full',
        'border border-gray-200 shadow-md rounded-xl dark:bg-neutral-900 dark:border-neutral-700 dark:shadow-neutral-700/70',
      ]"
    >
      <p class="text-gray-500 font-medium">Login to Admin panel:</p>
      <div class="relative">
        <input
          type="email"
          class="peer py-2.5 sm:py-3 px-4 ps-11 block w-full bg-gray-100 border-transparent rounded-lg sm:text-sm focus:border-blue-500 focus:ring-blue-500 disabled:opacity-50 disabled:pointer-events-none dark:bg-neutral-700 dark:border-transparent dark:text-neutral-400 dark:placeholder-neutral-500 dark:focus:ring-neutral-600"
          placeholder="Enter name"
          v-model="credential.name"
        />
        <div
          class="absolute inset-y-0 start-0 flex items-center pointer-events-none ps-4 peer-disabled:opacity-50 peer-disabled:pointer-events-none"
        >
          <svg
            class="shrink-0 size-4 text-gray-500 dark:text-neutral-500"
            xmlns="http://www.w3.org/2000/svg"
            width="24"
            height="24"
            viewBox="0 0 24 24"
            fill="none"
            stroke="currentColor"
            stroke-width="2"
            stroke-linecap="round"
            stroke-linejoin="round"
          >
            <path d="M19 21v-2a4 4 0 0 0-4-4H9a4 4 0 0 0-4 4v2"></path>
            <circle cx="12" cy="7" r="4"></circle>
          </svg>
        </div>
      </div>

      <div class="relative">
        <input
          type="password"
          class="peer py-2.5 sm:py-3 px-4 ps-11 block w-full bg-gray-100 border-transparent rounded-lg sm:text-sm focus:border-blue-500 focus:ring-blue-500 disabled:opacity-50 disabled:poinstartter-events-none dark:bg-neutral-700 dark:border-transparent dark:text-neutral-400 dark:placeholder-neutral-500 dark:focus:ring-neutral-600"
          placeholder="Enter password"
          v-model="credential.password"
        />
        <div
          class="absolute inset-y-0 start-0 flex items-center pointer-events-none ps-4 peer-disabled:opacity-50 peer-disabled:pointer-events-none"
        >
          <svg
            class="shrink-0 size-4 text-gray-500 dark:text-neutral-500"
            xmlns="http://www.w3.org/2000/svg"
            width="24"
            height="24"
            viewBox="0 0 24 24"
            fill="none"
            stroke="currentColor"
            stroke-width="2"
            stroke-linecap="round"
            stroke-linejoin="round"
          >
            <path
              d="M2 18v3c0 .6.4 1 1 1h4v-3h3v-3h2l1.4-1.4a6.5 6.5 0 1 0-4-4Z"
            ></path>
            <circle cx="16.5" cy="7.5" r=".5"></circle>
          </svg>
        </div>
      </div>

      <div class="flex flex-row gap-1 justify-end">
        <Button @click="onLogin">Login</Button>
        <Button @click="goBack">Back </Button>
      </div>
      <p v-if="loginResult" class="text-red-500">
        {{ errorMsg }}
      </p>
    </divs>
  </CenterADiv>
</template>
