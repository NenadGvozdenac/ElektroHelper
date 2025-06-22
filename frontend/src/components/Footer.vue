<template>
    <!-- CTA Section -->
    <section id="contact" class="pt-16 pb-1 bg-emerald-600">
        <div class="max-w-7xl mx-auto px-4 text-center">
            <h2 class="text-3xl font-bold text-white mb-6">
                Ready to Simplify Your Electricity Management?
            </h2>
            <p class="text-emerald-100 mb-8 max-w-2xl mx-auto">
                Join thousands of satisfied users who have transformed their electricity payment management
                with
                ElektroHelper.
            </p>
            <div class="flex flex-col sm:flex-row justify-center space-y-4 sm:space-y-0 sm:space-x-4">
                <button @click="userLoggedIn ? goToDashboard() : goToLogin()"
                    class="bg-white text-emerald-600 px-8 py-3 rounded-lg hover:bg-emerald-50 transition-colors">
                    Get Started
                </button>
                <button
                    class="border-2 border-white text-white px-8 py-3 rounded-lg hover:bg-emerald-700 transition-colors">
                    Contact Sales
                </button>
            </div>
            <p class="text-emerald-100 mt-8">© 2025 ElektroHelper. All rights reserved.</p>
        </div>
    </section>
</template>
<script setup lang="ts">
import { isUserLoggedIn } from '@/app/services/backend/auth_service';
import { onMounted, ref } from 'vue';

const userLoggedIn = ref(false);

const goToLogin = () => {
  window.location.href = '/login'
}

const goToDashboard = () => {
  window.location.href = '/dashboard'
}

onMounted(async () => {
  userLoggedIn.value = await isUserLoggedIn()
});

</script>