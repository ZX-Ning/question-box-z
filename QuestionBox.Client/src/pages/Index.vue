<script setup lang="ts">
import QuestionsList from "../components/QuestionsList.vue";
import UserCard from "../components/UserCard.vue";
import MainLayout from "../layouts/MainLayout.vue";
import { useSizeBreakpoint } from "../states";
import { computed } from "vue";

const state = useSizeBreakpoint();
const isFooterUnderCard = computed(() => state.isLarge);
</script>

<template>
  <MainLayout>
    <UserCard :show-footer="isFooterUnderCard" />
    <QuestionsList />
  </MainLayout>
</template>
