<template>
    <!-- Toast Container - Fixed position at the bottom -->
    <div class="fixed bottom-4 right-4 z-50">
        <TransitionGroup name="toast">
            <div v-for="toast in toasts" :key="toast.id"
                class="mb-2 p-4 bg-white rounded-lg shadow-lg border border-slate-200 flex items-center space-x-2 transform transition-all duration-300">
                <CheckCircle class="w-5 h-5 text-emerald-500" />
                <span class="text-slate-700">{{ toast.message }}</span>
            </div>
        </TransitionGroup>
    </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { CheckCircle } from 'lucide-vue-next';

interface Toast {
    id: number;
    message: string;
}

const toasts = ref<Toast[]>([]);
let toastId = 0;

const showToast = (message: string) => {
    const id = toastId++;
    toasts.value.push({ id, message });
    setTimeout(() => {
        toasts.value = toasts.value.filter(t => t.id !== id);
    }, 3000);
};

defineExpose({ showToast });
</script>

<style scoped>
.toast-enter-active,
.toast-leave-active {
    transition: all 0.3s ease;
}

.toast-enter-from {
    opacity: 0;
    transform: translateX(100%);
}

.toast-leave-to {
    opacity: 0;
    transform: translateY(100%);
}
</style>