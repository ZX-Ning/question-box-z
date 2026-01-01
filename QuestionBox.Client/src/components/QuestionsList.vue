<script setup lang="ts">
import {
  AccordionContent,
  AccordionHeader,
  AccordionItem,
  AccordionRoot,
  AccordionTrigger,
} from "radix-vue";
import { Icon } from "@iconify/vue";
import Spinner from "./BasicComponents/Spinner.vue";
import axios, { AxiosError } from "axios";
import { type Ref, ref, onMounted } from "vue";

type QuestionDto = {
  index: number;
  question: string;
  answer: string;
  questionTime: string;
  answerTime: string;
};

const questions: Ref<QuestionDto[] | null> = ref(null);
const questionFetchErrors: Ref<string | null> = ref(null);
const questionsUrl = API_URL + "questions";

async function getQuestions() {
  try {
    questions.value = (await axios.get(questionsUrl)).data;
  } catch (err) {
    if (err instanceof AxiosError) {
      questionFetchErrors.value = err.message;
    }
  }
}

onMounted(async () => {
  await getQuestions();
});
</script>

<template>
  <div
    :class="[
      'lg:max-h-full lg:min-w-[600px] flex-grow min-w-[300px] lg:max-w-[880px] lg:overflow-hidden',
      'flex flex-col items-stretch',
      'bg-white lg:border lg:shadow-md lg:rounded-xl',
      'dark:bg-neutral-900 dark:border-neutral-700 dark:shadow-neutral-700/70',
    ]"
  >
    <div
      v-if="!questions && !questionFetchErrors"
      :class="[
        'text-xl text-gray-500 font-medium italic',
        'p-10 flex flex-col items-center justify-center gap-2 grow',
      ]"
    >
      <Spinner />
      <p>Loading...</p>
    </div>
    <div
      v-else-if="questionFetchErrors"
      :class="[
        `text-base text-red-500 font-mono`,
        'p-10 flex flex-col items-center justify-center gap-2 grow',
      ]"
    >
      ❌ Cannot load questions: {{ questionFetchErrors }}. Try refreshing.
    </div>
    <div
      v-else
      class="lg:overflow-y-auto lg:better-scroll flex-auto p-2 lg:p-8"
    >
      <AccordionRoot type="single" collapsible>
        <template v-for="item of questions" :key="item.index">
          <AccordionItem
            :class="[
              'accordion-item w-full overflow-hidden ring-[2px] ring-neutral-200',
              'mt-px first:mt-0 first:rounded-t-lg last:rounded-b-lg',
              'data-[state=open]:my-5 data-[state=open]:rounded-lg',
              'data-[state=open]:shadow-lg data-[state=open]:ring-2 data-[state=open]:ring-neutral-400',
              'focus-within:relative focus-within:z-10',
            ]"
            :value="'' + item.index"
          >
            <AccordionHeader class="flex">
              <AccordionTrigger
                :class="[
                  'group',
                  'flex w-full  gap-5 cursor-default items-center justify-between',
                  'min-h-[45px] px-5 py-5 outline-none',
                  'text-zinc-800 bg-white',
                  'hover:bg-neutral-200 ',
                ]"
              >
                <p
                  :class="[
                    'font-[460] text-[0.97rem] leading-[1.3rem] grow',
                    'text-justify text-pretty whitespace-pre-line font-sans',
                    'group-data-[state=open]:cursor-text select-text',
                    'group-hover:text-blue-500',
                    'group-data-[state=open]:text-blue-500',
                    'transition-transform duration-[170ms]',
                  ]"
                >
                  Q. "{{ item.question }}"
                  <span
                    class="block text-end text-[0.8rem] leading-[1.6rem] text-sm text-gray-500"
                    >{{ item.questionTime }}</span
                  >
                </p>
                <Icon
                  icon="radix-icons:chevron-down"
                  width="1.75rem"
                  height="1.75rem"
                  :class="[
                    'min-w-[1.75rem] text-blue-900/70',
                    'group-hover:scale-[120%] group-hover:text-blue-500',
                    'group-data-[state=open]:rotate-180',
                    'transition-transform duration-[170ms]',
                  ]"
                />
              </AccordionTrigger>
            </AccordionHeader>
            <AccordionContent
              :class="[
                'bg-neutral-100 py-2 pr-[calc(1.75rem+1.25rem)]',
                'text-gray-700 text-[1.02rem] leading-[1.2rem]',
                'data-[state=open]:animate-slideDown data-[state=closed]:animate-slideUp',
              ]"
            >
              <p
                class="px-5 py-4 text-wrap text-justify whitespace-pre-line font-[450] font-sans"
              >
                A. {{ item.answer }}
                <span
                  class="block text-end text-[0.9rem] leading-[1.6rem] text-sm text-gray-500"
                  >{{ item.answerTime || "Unknown Time" }}
                </span>
              </p>
            </AccordionContent>
          </AccordionItem>
        </template>
      </AccordionRoot>
    </div>
  </div>
</template>
