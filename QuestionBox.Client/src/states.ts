import { defineStore } from 'pinia'
import { isLg } from "./utils/Breakpoint";
import { ref } from 'vue';

export const useSizeBreakpoint = defineStore('SizeBreakpoint', () => {
   const isLarge = ref(false);
   function update() {
    console.log("update: isLg = ", isLg());
    isLarge.value = isLg();
   }
   return {isLarge, update}
})