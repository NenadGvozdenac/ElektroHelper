<template>
    <div v-if="isOpen" class="fixed inset-0 z-50 overflow-y-auto">
        <!-- Backdrop -->
        <div class="fixed inset-0 bg-slate-900/50 backdrop-blur-sm" @click="close"></div>

        <!-- Modal -->
        <div class="min-h-screen px-4 flex items-center justify-center">
            <div class="relative bg-white rounded-xl shadow-xl max-w-2xl w-full mx-auto">
                <!-- Modal Header -->
                <div class="flex items-center justify-between p-6 border-b border-slate-200">
                    <div class="flex items-center space-x-3">
                        <div
                            class="w-10 h-10 rounded-full bg-gradient-to-br from-emerald-500 to-indigo-500 flex items-center justify-center">
                            <MessageCircle class="w-5 h-5 text-white" />
                        </div>
                        <div>
                            <h2 class="text-xl font-semibold text-slate-900">Create New Forum</h2>
                            <p class="text-sm text-slate-500 mt-0.5">Create a new space for discussions</p>
                        </div>
                    </div>
                    <button @click="close" class="p-2 hover:bg-slate-100 rounded-lg transition-colors group">
                        <X class="w-5 h-5 text-slate-400 group-hover:text-slate-600" />
                    </button>
                </div>

                <!-- Modal Content -->
                <div class="p-6">
                    <!-- Title Input with character count -->
                    <div class="mb-6">
                        <div class="flex justify-between mb-2">
                            <label class="text-sm font-medium text-slate-700">Forum Title</label>
                            <span class="text-xs text-slate-400">{{ title.length }}/20</span>
                        </div>
                        <input type="text" v-model="title" maxlength="20" placeholder="Enter forum title..."
                            class="w-full rounded-lg border-slate-200 px-4 py-3 focus:ring-2 focus:ring-emerald-500 focus:border-transparent bg-slate-50 hover:bg-white transition-colors" />
                    </div>

                    <!-- Description Input with character count -->
                    <div class="mb-6">
                        <div class="flex justify-between mb-2">
                            <label class="text-sm font-medium text-slate-700">Description</label>
                            <span class="text-xs text-slate-400">{{ description.length }}/100</span>
                        </div>
                        <textarea v-model="description" rows="3" maxlength="100"
                            placeholder="Brief description of the forum..."
                            class="w-full rounded-lg border-slate-200 px-4 py-3 focus:ring-2 focus:ring-emerald-500 focus:border-transparent bg-slate-50 hover:bg-white transition-colors resize-none" />
                    </div>

                    <!-- Validation Messages -->
                    <div v-if="!isValid && (title.length > 0 || description.length > 0)"
                        class="mb-6 p-3 bg-amber-50 border border-amber-200 rounded-lg">
                        <div class="flex items-center space-x-2 text-amber-600">
                            <AlertCircle class="w-5 h-5" />
                            <span class="text-sm font-medium">Please fill in all required fields</span>
                        </div>
                    </div>
                </div>

                <!-- Modal Footer -->
                <div class="border-t border-slate-200">
                    <div class="flex items-center justify-between p-6">
                        <div class="flex items-center space-x-2 text-sm text-slate-500">
                            <InfoIcon class="w-4 h-4" />
                            <span>Forums should follow community guidelines</span>
                        </div>
                        <div class="flex items-center gap-3">
                            <button @click="close"
                                class="px-4 py-2 text-slate-700 hover:bg-slate-100 rounded-lg transition-colors">
                                Cancel
                            </button>
                            <button @click="handleSubmit" :disabled="!isValid || isSubmitting" :class="[
                                'px-6 py-2 rounded-lg font-medium transition-colors flex items-center space-x-2',
                                isValid
                                    ? 'bg-emerald-600 hover:bg-emerald-700 text-white'
                                    : 'bg-slate-100 text-slate-400 cursor-not-allowed'
                            ]">
                                <Loader2 v-if="isSubmitting" class="w-4 h-4 animate-spin" />
                                <span>{{ isSubmitting ? 'Creating...' : 'Create Forum' }}</span>
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue';
import { X, Loader2, MessageCircle, AlertCircle, InfoIcon } from 'lucide-vue-next';
import type { CreateForum } from '@/app/models/forum_backend/Forum';

const props = defineProps<{
    isOpen: boolean;
}>();

const emit = defineEmits<{
    (e: 'close'): void;
    (e: 'submit', data: CreateForum): void;
}>();

const title = ref('');
const description = ref('');
const isSubmitting = ref(false);

const isValid = computed(() => {
    return title.value.trim().length > 0 &&
        description.value.trim().length > 0 &&
        title.value.length <= 20 &&
        description.value.length <= 100;
});

function close() {
    emit('close');
    resetForm();
}

function resetForm() {
    title.value = '';
    description.value = '';
}

async function handleSubmit() {
    if (!isValid.value || isSubmitting.value) return;

    isSubmitting.value = true;
    try {
        await emit('submit', {
            name: title.value.trim(),
            description: description.value.trim()
        });
        close();
    } finally {
        isSubmitting.value = false;
    }
}
</script>