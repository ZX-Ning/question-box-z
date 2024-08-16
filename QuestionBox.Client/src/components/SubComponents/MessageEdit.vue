<script setup lang="ts">
import { ref, onMounted, computed } from "vue";
import * as AppConfig from "../../../../AppConfig.json";
import SendingDialog from "./SendingDialog.vue";

const charaterLimit = AppConfig.QuestionLengthLimit;
const interval = {};
let changedSinceLastSave = false;
const message = ref("");
const lastSave = ref("--");
const currenrLength = computed(() => message.value.length);

onMounted(() => {
  const msg = localStorage.getItem("message");
  if (msg) {
    message.value = msg;
  }
  (interval as any)["interval"] = setInterval(autoSave, 10e3);
});

function onInput(e: Event) {
  const CHARACTER_ALLOWED = 5000;
  const textarea = e.target as HTMLTextAreaElement;
  if (textarea.value.length >= CHARACTER_ALLOWED) {
    textarea.value = textarea.value.slice(0, CHARACTER_ALLOWED);
  }
  message.value = textarea.value;
  localStorage.setItem("message", message.value);
  changedSinceLastSave = true;
}

function onFocusOut() {
  autoSave();
}

function autoSave() {
  if (changedSinceLastSave) {
    localStorage.setItem("message", message.value);
    const date = new Date();
    lastSave.value = `${date.getHours()}:${date.getMinutes()}:${date.getSeconds()}`;
    changedSinceLastSave = false;
  }
}

function clear() {
  changedSinceLastSave = false;
  message.value = "";
  localStorage.setItem("message", "");
  lastSave.value = "--";
}
</script>

<template>
  <textarea
    :value="message"
    :class="[
      'min-h-[80px] h-[120px] lg:h-[300px] lg:max-h-[500px] w-full resize-y',
      'bg-neutral-50 px-3 py-2 rounded-lg',
      'font-medium text-gray-700 text-[0.95rem]',
      'overflow-y-auto better-scroll',
      'disabled:opacity-50 disabled:pointer-events-none',
      'dark:bg-neutral-900 dark:border-neutral-700 dark:text-neutral-400',
      'dark:placeholder-neutral-500 dark:focus:ring-neutral-600',
      currenrLength > charaterLimit
        ? 'focus:border-red-500 focus:ring-red-500'
        : 'focus:border-sky-600 focus:ring-sky-600',
    ]"
    @input="onInput"
    @focusout="onFocusOut"
    placeholder="Message..."
    rows="1"
  ></textarea>
  <div
    class="flex flex-row flex-wrap-reverse w-full items-center px-2.5 py-1.5 justify-between bg-neutral-50 rounded-lg shadow-sm"
  >
    <p class="text-gray-600 text-[0.75rem] italic">
      <span class="text-gray-600">Saved on {{ lastSave }}</span>
      <span v-if="changedSinceLastSave">*</span>
    </p>
    <p
      :class="[
        'text-[0.86rem]',
        currenrLength > charaterLimit
          ? 'text-red-500 scale-105 '
          : 'text-gray-800',
      ]"
    >
      Characters: {{ currenrLength }}/{{ charaterLimit }}
    </p>
    <SendingDialog
      :disabled="currenrLength > charaterLimit || currenrLength === 0"
      :message="message.trim().slice(0, charaterLimit)"
      @success="clear"
    />
  </div>
</template>
