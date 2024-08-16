<script setup lang="ts">
import {
  DialogClose,
  DialogContent,
  DialogOverlay,
  DialogPortal,
  DialogRoot,
  DialogTitle,
  DialogTrigger,
} from "radix-vue";
import { Icon } from "@iconify/vue";

const props = defineProps(["tittle", "description", "disabled", "open"]);
defineEmits(["update"]);
</script>

<template>
  <DialogRoot :open="props.open" @update:open="$emit('update')">
    <DialogTrigger
      class="py-1.5 px-3.5 inline-flex justify-center items-center text-sm font-medium rounded-md text-white bg-blue-500 hover:bg-blue-500/90 hover:shadow-md hover:scale-[102%] disabled:opacity-45 disabled:pointer-events-none active:scale-[85%] active:shadow-inner transition-transform duration-100"
      :disabled="props.disabled"
    >
      <slot name="trigerButton" />
    </DialogTrigger>
    <DialogPortal>
      <DialogOverlay
        class="bg-neutral-800/60 data-[state=open]:animate-overlayShow fixed inset-0 z-9999"
      />
      <DialogContent
        class="flex flex-col overflow-auto better-scroll max-h-[80vh] data-[state=open]:animate-contentShow fixed top-[50%] left-[50%] w-[90vw] max-w-[450px] translate-x-[-50%] translate-y-[-50%] rounded-[6px] bg-white p-[25px] shadow-[hsl(206_22%_7%_/_35%)_0px_10px_38px_-10px,_hsl(206_22%_7%_/_20%)_0px_10px_20px_-15px] focus:outline-none z-[100]"
      >
        <DialogTitle class="text-neutral-700 mb-2 text-[1.1rem] font-semibold">
          {{ props.tittle || "This Is Tittle" }}
        </DialogTitle>
        <slot name="dialogContent" />
        <DialogClose
          class="text-neutral-700 hover:bg-blue-400 hover:text-neutral-100 absolute top-[10px] right-[10px] inline-flex p-1.5 appearance-none items-center justify-center rounded-full"
          aria-label="Close"
        >
          <Icon icon="lucide:x" width="24px" height="24px" />
        </DialogClose>
      </DialogContent>
    </DialogPortal>
  </DialogRoot>
</template>
