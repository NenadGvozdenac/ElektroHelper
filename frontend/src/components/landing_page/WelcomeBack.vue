<template>
    <div v-if="userData" class="max-w-7xl mx-auto px-4 pt-24" :class="{ 'fade-in-section is-visible': isVisible }">
        <div class="bg-gradient-to-r from-emerald-500 to-teal-600 rounded-2xl p-8 shadow-xl">
            <div class="flex flex-col md:flex-row justify-between items-center space-y-6 md:space-y-0">
                <div class="flex-1">
                    <h1 class="text-4xl font-bold text-white mb-2">
                        Welcome back, {{ userData.userName }}!
                    </h1>
                    <p class="text-emerald-50">Ready to manage your electricity payments?</p>
                </div>
                <div>
                    <router-link to="/dashboard"
                        class="inline-flex items-center px-6 py-3 bg-white text-emerald-600 rounded-lg font-semibold hover:bg-emerald-50 transition-all hover:scale-105">
                        Go to Dashboard
                        <svg class="w-5 h-5 ml-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                d="M13 7l5 5m0 0l-5 5m5-5H6" />
                        </svg>
                    </router-link>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import type { UserData } from '@/app/models/user';

defineProps<{
    userData: UserData | null;
}>();

const isVisible = ref(false);

onMounted(() => {
    setTimeout(() => {
        isVisible.value = true;
    }, 100);
});
</script>

<style scoped>
.fade-in-section {
    opacity: 0;
    transform: translateY(20px);
    transition: opacity 0.6s ease-out, transform 0.6s ease-out;
}

.fade-in-section.is-visible {
    opacity: 1;
    transform: translateY(0);
}
</style>