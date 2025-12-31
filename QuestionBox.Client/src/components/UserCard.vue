<script setup lang="ts">
import MessageEdit from "./SubComponents/MessageEdit.vue";
const props = defineProps(["showFooter"]);
const user = CONFIG["User"];
</script>

<template>
  <div class="flex flex-col lg:max-h-full lg:min-w-[30%] lg:max-w-[50%] justify-between">
    <div
      :class="[
        'flex-auto lg:max-w-[700px] p-6',
        'flex flex-col items-center gap-2',
        'bg-white border shadow-md rounded-xl',
        'dark:bg-neutral-900 dark:border-neutral-700 dark:shadow-neutral-700/70',
      ]"
    >
      <img
        :src="user.avatar"
        class="inline-block rounded-full max-h-[72px] max-w-[72px] aspect-square"
      />
      <p class="text-2xl font-medium">{{ user.name }}</p>
      <p class="text-[0.96rem] italic whitespace-pre-line">
          {{ user.description }}
      </p>
      <MessageEdit />
    </div>
  </div>
</template>

