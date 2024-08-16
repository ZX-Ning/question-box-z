<script setup lang="ts">
import axios from "axios";
import { questionApiUrl } from "../../config";
import { type Ref, ref } from "vue";
import Dialog from "../BasicComponents/Dialog.vue";
import Button from "../BasicComponents/Button.vue";
import { Icon } from "@iconify/vue/dist/iconify.js";

const props = defineProps(["message", "disabled"]);
const emit = defineEmits(["success"]);
const sendingState: Ref<null | "Error" | "Success" | "Pending"> = ref(null);

async function onSubmit() {
  sendingState.value = "Pending";
  try {
    const msg = props.message;
    await axios.post(questionApiUrl, { question: msg });
    sendingState.value = "Success";
  } catch (err) {
    sendingState.value = "Error";
  }
}
const isOpen = ref(false);

function onDialogUpdate() {
  if (!isOpen.value) {
    isOpen.value = true;
    return;
  }
  isOpen.value = false;
  if (sendingState.value === "Success") {
    emit("success");
  }
  sendingState.value = null;
}
</script>

<template>
  <Dialog
    tittle="Send Message"
    :disabled="props.disabled"
    @update="onDialogUpdate"
    :open="isOpen"
  >
    <template #trigerButton>
      Send
      <Icon
        icon="icon-park-outline:send"
        class="h-[20px] w-[20px] mt-[2px] ml-1"
      />
    </template>
    <template #dialogContent>
      <div class="flex flex-col gap-2 min-h-[50px] min-w-[50px]">
        <template v-if="sendingState === 'Success'">
          <p class="text-lg text-blue-600 self-center font-semibold">
            Sended!
            <Icon
              icon="weui:done-filled"
              width="1.75rem"
              height="1.75rem"
              class="inline"
            />
          </p>
        </template>
        <template v-else>
          Comfirm Your Message:
          <div
            :class="[
              'w-full p-4 border border-neutral-500 rounded-lg',
              'text-warp whitespace-pre-line font-medium text-gray-700 text-[0.95rem]',
              'better-scroll overflow-y-auto ',
            ]"
          >
            {{ props.message }}
          </div>
          <Button @click="onSubmit" :disabled="!!sendingState">
            <template v-if="!sendingState">
              Send!
              <Icon
                icon="icon-park-outline:send"
                class="h-[20px] w-[20px] mt-[2px] ml-1"
              />
            </template>
            <template v-else-if="sendingState === 'Pending'">
              <div
                class="animate-spin inline-block size-4 border-[2px] border-current border-t-transparent text-neutral-50 rounded-full dark:text-neutral-300"
                role="status"
                aria-label="sending"
              ></div>
              &nbsp;Sending...
            </template>
            <template v-else-if="sendingState === 'Error'">
              <Icon
                icon="weui:close-filled"
                width="1.2rem"
                height="1.2rem"
                class="text-red-600 mt-[1px] mr-1"
              />
              Some thing went wrong {{ ": (" }}
            </template>
          </Button>
        </template>
      </div>
    </template>
  </Dialog>
</template>
