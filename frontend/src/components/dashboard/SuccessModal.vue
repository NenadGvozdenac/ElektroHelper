<template>
    <TransitionRoot appear :show="modelValue" as="template">
        <Dialog as="div" @close="$emit('update:modelValue', false)" class="relative z-50">
            <TransitionChild as="template" enter="duration-300 ease-out" enter-from="opacity-0" enter-to="opacity-100"
                leave="duration-200 ease-in" leave-from="opacity-100" leave-to="opacity-0">
                <div class="fixed inset-0 bg-black/25" />
            </TransitionChild>

            <div class="fixed inset-0 overflow-y-auto">
                <div class="flex min-h-full items-center justify-center p-4 text-center">
                    <TransitionChild as="template" enter="duration-300 ease-out" enter-from="opacity-0 scale-95"
                        enter-to="opacity-100 scale-100" leave="duration-200 ease-in" leave-from="opacity-100 scale-100"
                        leave-to="opacity-0 scale-95">
                        <DialogPanel
                            class="w-full max-w-md transform overflow-hidden rounded-lg bg-white p-6 text-left align-middle shadow-xl transition-all">
                            <div class="flex items-center gap-3 mb-4">
                                <div class="flex-shrink-0 bg-emerald-100 rounded-full p-2">
                                    <CheckIcon class="h-6 w-6 text-emerald-600" />
                                </div>
                                <DialogTitle as="h3" class="text-lg font-semibold text-emerald-700">
                                    {{ title }}
                                </DialogTitle>
                            </div>

                            <div class="mt-2">
                                <p class="text-sm text-emerald-600">
                                    {{ message }}
                                </p>
                            </div>

                            <div class="mt-6">
                                <button type="button"
                                    class="w-full inline-flex justify-center rounded-md bg-emerald-600 px-4 py-2 text-sm font-medium text-white hover:bg-emerald-700 focus:outline-none focus-visible:ring-2 focus-visible:ring-emerald-500 focus-visible:ring-offset-2 transition-colors"
                                    @click="$emit('update:modelValue', false)">
                                    Continue
                                </button>
                            </div>
                        </DialogPanel>
                    </TransitionChild>
                </div>
            </div>
        </Dialog>
    </TransitionRoot>
</template>

<script setup lang="ts">
import { Dialog, DialogPanel, DialogTitle, TransitionChild, TransitionRoot } from '@headlessui/vue'
import { CheckIcon } from 'lucide-vue-next'

defineProps({
    modelValue: {
        type: Boolean,
        required: true
    },
    title: {
        type: String,
        default: 'Success!'
    },
    message: {
        type: String,
        default: 'Operation completed successfully.'
    }
})

defineEmits(['update:modelValue'])
</script>