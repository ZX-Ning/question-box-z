<script setup lang="ts">
import { onMounted, ref } from "vue";
import QuestionsList from "./components/QuestionsList.vue";
import UserCard from "./components/UserCard.vue";
import MainLayout from "./layouts/MainLayout.vue";
import Footer from "./components/SubComponents/Footer.vue";
import { isLg } from "./utils/Breakpoint";

onMounted(() => {
  window.addEventListener("resize", onResize);
});

const isFooterUnderCard = ref();

function onResize() {
  isFooterUnderCard.value = isLg();
}

onMounted(() => {
  onResize();
})

</script>

<template>
  <MainLayout>
    <UserCard :show-footer="isFooterUnderCard" />
    <QuestionsList />
    <template v-if="!isFooterUnderCard">
      <Footer />
    </template>
  </MainLayout>
</template>
