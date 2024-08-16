<script setup lang="ts"></script>

<template>
  <div
    :class="[
      'w-screen h-screen overflow-auto',
      'pt-2 pb-6 px-1 lg:pt-5 lg:pb-5',
      'flex flex-col gap-3',
      'lg:flex-row lg:gap-0 lg:justify-evenly lg:items-start',
    ]"
  >
    <slot />
  </div>
</template>
