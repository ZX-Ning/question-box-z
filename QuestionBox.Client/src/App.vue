<script setup lang="ts">
import { onMounted } from "vue";

import { useSizeBreakpoint } from "./states";
import { RouterView } from "vue-router";

const state = useSizeBreakpoint();

onMounted(() => {
  window.addEventListener("resize", state.update);
});
</script>

<template>
  <RouterView />
</template>
