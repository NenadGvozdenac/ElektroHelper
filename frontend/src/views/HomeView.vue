<template>
  <div class="min-h-screen bg-gray-50">
    <Navbar />

    <WelcomeSection :userData="userData" />

    <ForumsAndPayments v-if="userData" :userData="userData"/>

    <!-- Hero Section -->
    <section :class="['pt-24', 'pb-16', 'px-4', userData ? 'sm:pt-4' : 'sm:pt-24']">
      <div class="max-w-7xl mx-auto flex flex-col md:flex-row items-center px-8">
        <div class="md:w-1/2 mb-8 md:mb-0">
          <h1 class="text-4xl md:text-5xl font-bold text-gray-900 mb-6 opacity-0 animate-fadeInUp">
            Manage Your Electricity Payments with Ease
          </h1>
          <p class="text-lg text-gray-600 mb-8 opacity-0 animate-fadeInUp stagger-1">
            Track payments, manage utilities, and join our community forum. All in one place with ElektroHelper.
          </p>
          <div class="flex space-x-4 opacity-0 animate-fadeInUp stagger-2">
            <button
              class="bg-emerald-600 text-white px-8 py-3 rounded-lg hover:bg-emerald-700 transition-all hover:scale-105">
              Get Started
            </button>
            <button
              class="border-2 border-emerald-600 text-emerald-600 px-8 py-3 rounded-lg hover:bg-emerald-50 transition-all hover:scale-105">
              Learn More
            </button>
          </div>
        </div>
        <div class="md:w-1/2 opacity-0 animate-fadeInUp stagger-3">
          <svg viewBox="0 0 800 600" class="w-full h-auto shadow-xl rounded-lg animate-float">
            <!-- Dashboard SVG with animations -->
            <rect x="50" y="50" width="700" height="500" rx="20" fill="#ffffff" stroke="#e5e7eb" stroke-width="2"
              class="animate-pulse">
              <animate attributeName="opacity" values="0;1" dur="0.5s" begin="0s" fill="freeze" />
            </rect>

            <!-- Animated Graph Line -->
            <path d="M300 200 L500 300 L700 250" stroke="#10b981" stroke-width="3" fill="none" stroke-dasharray="500"
              stroke-dashoffset="500">
              <animate attributeName="stroke-dashoffset" values="500;0" dur="1.5s" begin="0.5s" fill="freeze" />
            </path>

            <!-- Animated Cards -->
            <rect x="300" y="100" width="200" height="80" rx="10" fill="#f0fdf4" opacity="0">
              <animate attributeName="opacity" values="0;1" dur="0.3s" begin="0.3s" fill="freeze" />
            </rect>
            <rect x="520" y="100" width="200" height="80" rx="10" fill="#f0fdf4" opacity="0">
              <animate attributeName="opacity" values="0;1" dur="0.3s" begin="0.6s" fill="freeze" />
            </rect>
          </svg>
        </div>
      </div>
    </section>

    <!-- Features Section -->
    <section id="features" class="py-16 bg-white">
      <div class="max-w-7xl mx-auto px-4">
        <h2 class="text-3xl font-bold text-center text-gray-900 mb-12">
          Powerful Features for Your Needs
        </h2>
        <div class="grid grid-cols-1 md:grid-cols-3 gap-8">
          <div class="p-6 bg-gray-50 rounded-lg">
            <div class="w-12 h-12 bg-emerald-100 rounded-lg flex items-center justify-center mb-4">
              <svg class="w-6 h-6 text-emerald-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 10V3L4 14h7v7l9-11h-7z" />
              </svg>
            </div>
            <h3 class="text-xl font-semibold mb-2">Payment Tracking</h3>
            <p class="text-gray-600">Keep track of all your electricity payments in one place with detailed history and
              analytics.</p>
          </div>
          <div class="p-6 bg-gray-50 rounded-lg">
            <div class="w-12 h-12 bg-emerald-100 rounded-lg flex items-center justify-center mb-4">
              <svg class="w-6 h-6 text-emerald-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                  d="M17 8h2a2 2 0 012 2v6a2 2 0 01-2 2h-2v4l-4-4H9a1.994 1.994 0 01-1.414-.586m0 0L11 14h4a2 2 0 002-2V6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2v4l.586-.586z" />
              </svg>
            </div>
            <h3 class="text-xl font-semibold mb-2">Community Forum</h3>
            <p class="text-gray-600">Connect with other users, share tips, and get help from our active community.</p>
          </div>
          <div class="p-6 bg-gray-50 rounded-lg">
            <div class="w-12 h-12 bg-emerald-100 rounded-lg flex items-center justify-center mb-4">
              <svg class="w-6 h-6 text-emerald-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                  d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2" />
              </svg>
            </div>
            <h3 class="text-xl font-semibold mb-2">Smart Payments</h3>
            <p class="text-gray-600">Make secure payments directly through our platform with multiple payment options.
            </p>
          </div>
        </div>
      </div>
    </section>

    <!-- Benefits Section -->
    <section id="benefits" class="py-16 bg-emerald-50">
      <div class="max-w-7xl mx-auto px-4">
        <h2 class="text-3xl font-bold text-center text-gray-900 mb-12">
          Why Choose ElektroHelper?
        </h2>
        <div class="grid grid-cols-1 md:grid-cols-2 gap-12">
          <div class="flex flex-col justify-center">
            <ul class="space-y-6">
              <li class="flex items-start">
                <svg class="w-6 h-6 text-emerald-600 mt-1 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7" />
                </svg>
                <div>
                  <h3 class="text-lg font-semibold mb-1">Easy Payment Management</h3>
                  <p class="text-gray-600">Track and manage all your electricity payments in one convenient dashboard.
                  </p>
                </div>
              </li>
              <li class="flex items-start">
                <svg class="w-6 h-6 text-emerald-600 mt-1 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7" />
                </svg>
                <div>
                  <h3 class="text-lg font-semibold mb-1">Secure Transactions</h3>
                  <p class="text-gray-600">Bank-level security for all your payment transactions and personal data.</p>
                </div>
              </li>
              <li class="flex items-start">
                <svg class="w-6 h-6 text-emerald-600 mt-1 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7" />
                </svg>
                <div>
                  <h3 class="text-lg font-semibold mb-1">24/7 Support</h3>
                  <p class="text-gray-600">Get help anytime through our community forum and dedicated support team.</p>
                </div>
              </li>
            </ul>
          </div>
          <div class="flex items-center justify-center">
            <img src="../assets/analytics.svg" alt="ElektroHelper Benefits" class="rounded-lg shadow-xl" />
          </div>
        </div>
      </div>
    </section>

    <!-- CTA Section -->
    <section id="contact" class="pt-16 pb-1 bg-emerald-600">
      <div class="max-w-7xl mx-auto px-4 text-center">
        <h2 class="text-3xl font-bold text-white mb-6">
          Ready to Simplify Your Electricity Management?
        </h2>
        <p class="text-emerald-100 mb-8 max-w-2xl mx-auto">
          Join thousands of satisfied users who have transformed their electricity payment management with
          ElektroHelper.
        </p>
        <div class="flex flex-col sm:flex-row justify-center space-y-4 sm:space-y-0 sm:space-x-4">
          <button class="bg-white text-emerald-600 px-8 py-3 rounded-lg hover:bg-emerald-50 transition-colors">
            Get Started
          </button>
          <button class="border-2 border-white text-white px-8 py-3 rounded-lg hover:bg-emerald-700 transition-colors">
            Contact Sales
          </button>
        </div>
        <p class="text-emerald-100 mt-8">© 2021 ElektroHelper. All rights reserved.</p>
      </div>
    </section>
  </div>
</template>

<script setup lang="ts">
import { getUserData } from '@/app/services/backend/auth_service';
import Navbar from '@/components/Navbar.vue';
import WelcomeSection from '@/components/landing_page/WelcomeBack.vue';
import ForumsAndPayments from '@/components/landing_page/ForumsAndPayments.vue';
import { ref, onMounted } from 'vue';
import type { UserData } from '@/app/models/user';

const userData = ref<UserData | null>(null);

const getUser = async () => {
  userData.value = await getUserData();
};

onMounted(() => {
  getUser();
});
</script>

<style scoped>
@import '../assets/animations.css';

/* Intersection Observer animations */
.fade-in-section {
  opacity: 0;
  transform: translateY(20px);
  visibility: hidden;
  transition: opacity 0.6s ease-out, transform 0.6s ease-out;
  will-change: opacity, visibility;
}

.fade-in-section.is-visible {
  opacity: 1;
  transform: none;
  visibility: visible;
}
</style>