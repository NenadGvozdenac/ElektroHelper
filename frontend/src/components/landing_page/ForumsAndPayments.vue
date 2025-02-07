<template>
    <div v-if="userData" class="py-8">
        <div class="max-w-7xl mx-auto px-4">
            <div class="grid grid-cols-1 md:grid-cols-2 gap-8">
                <!-- Forums Section -->
                <div class="bg-gray-50 rounded-xl p-8 shadow-lg">
                    <div class="flex items-center mb-6">
                        <div class="w-12 h-12 bg-emerald-100 rounded-lg flex items-center justify-center">
                            <svg class="w-6 h-6 text-emerald-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                    d="M17 8h2a2 2 0 012 2v6a2 2 0 01-2 2h-2v4l-4-4H9a1.994 1.994 0 01-1.414-.586m0 0L11 14h4a2 2 0 002-2V6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2v4l.586-.586z" />
                            </svg>
                        </div>
                        <h2 class="text-2xl font-bold ml-4">Community Forums</h2>
                    </div>
                    <p class="text-gray-600 mb-6">Join discussions, share experiences, and get help from other ElektroHelper users.</p>
                    <button @click="goToForums"
                        class="w-full bg-emerald-600 text-white px-6 py-3 rounded-lg hover:bg-emerald-700 transition-all hover:scale-105">
                        Browse Forums
                    </button>
                </div>

                <!-- Payments Section -->
                <div class="bg-gray-50 rounded-xl p-8 shadow-lg">
                    <div class="flex items-center mb-6">
                        <div class="w-12 h-12 bg-emerald-100 rounded-lg flex items-center justify-center">
                            <svg class="w-6 h-6 text-emerald-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                    d="M17 9V7a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2m2 4h10a2 2 0 002-2v-6a2 2 0 00-2-2H9a2 2 0 00-2 2v6a2 2 0 002 2zm7-5a2 2 0 11-4 0 2 2 0 014 0z" />
                            </svg>
                        </div>
                        <h2 class="text-2xl font-bold ml-4">Payments</h2>
                    </div>
                    <p class="text-gray-600 mb-6">Manage your electricity payments, view history, and track usage patterns.<br><br></p>
                    <button @click="goToPayments"
                        class="w-full bg-emerald-600 text-white px-6 py-3 rounded-lg hover:bg-emerald-700 transition-all hover:scale-105">
                        Manage Payments
                    </button>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import type { UserData } from '@/app/models/backend/user';

defineProps<{
    userData: UserData | null;
}>();

const goToForums = () => {
    window.location.href = '/forums';
};

const goToPayments = () => {
    window.location.href = '/payments';
};
</script>