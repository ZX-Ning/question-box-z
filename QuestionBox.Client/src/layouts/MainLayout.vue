<script setup lang="ts">
import Footer from '../components/SubComponents/Footer.vue';

</script>

<template>
  <div class="w-screen lg:h-screen max-w-full max-h-full flex flex-col">
    <div
      :class="[
        'overflow-auto flex-1 ',
        'pt-2 pb-6 px-1 lg:pt-5 lg:pb-5',
        'flex flex-col gap-3',
        'lg:flex-row lg:gap-0 lg:justify-evenly lg:items-start',
      ]"
    >
      <slot />
    </div>
    <Footer class="flex-grow-0" />
  </div>
</template>
