<script setup lang="ts">
const today = new Date();
</script>

<template>
  <footer class="block self-center p-1">
    QuestionBoxZ -
    <a href="https://github.com/ZX-Ning/question-box-z" class="link">
      Source Code
    </a>
    &copy; {{ today.getFullYear() }} Zhenxiang Ning
  </footer>
</template>
