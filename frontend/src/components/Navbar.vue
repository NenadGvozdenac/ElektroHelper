<template>
  <nav class="bg-white shadow-lg fixed w-full top-0 z-50">
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
      <div class="flex justify-between h-16">
        <div class="flex items-center">
          <a href="/">
            <span class="text-2xl font-bold text-emerald-600">ElektroHelper</span>
          </a>
        </div>
        
        <!-- Desktop menu -->
        <div class="hidden md:flex items-center space-x-8">
          <a href="#features" class="text-gray-600 hover:text-emerald-600 transition-colors">Features</a>
          <a href="#benefits" class="text-gray-600 hover:text-emerald-600 transition-colors">Benefits</a>
          <a href="/forums" class="text-gray-600 hover:text-emerald-600 transition-colors">Forums</a>
          <a href="#contact" class="text-gray-600 hover:text-emerald-600 transition-colors">Contact</a>
          <button 
            class="bg-emerald-600 text-white px-4 py-2 rounded-lg hover:bg-emerald-700 transition-colors"
            @click="userLoggedIn ? goToDashboard() : goToLogin()"
          >
            {{ userLoggedIn ? 'Dashboard' : 'Get Started' }}
          </button>
        </div>

        <!-- Mobile menu button -->
        <div class="md:hidden flex items-center">
          <button @click="toggleMenu" class="text-gray-600">
            <svg class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path v-if="!isMenuOpen" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16" />
              <path v-else stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
            </svg>
          </button>
        </div>
      </div>
    </div>

    <!-- Mobile menu -->
    <div v-if="isMenuOpen" class="md:hidden">
      <div class="px-2 pt-2 pb-3 space-y-1">
        <a href="#features" class="block px-3 py-2 text-gray-600 hover:text-emerald-600 transition-colors">Features</a>
        <a href="#benefits" class="block px-3 py-2 text-gray-600 hover:text-emerald-600 transition-colors">Benefits</a>
        <a href="/forums" class="block px-3 py-2 text-gray-600 hover:text-emerald-600 transition-colors">Forums</a>
        <a href="#contact" class="block px-3 py-2 text-gray-600 hover:text-emerald-600 transition-colors">Contact</a>
        <button 
          class="w-full mt-2 bg-emerald-600 text-white px-4 py-2 rounded-lg hover:bg-emerald-700 transition-colors"
          @click="userLoggedIn ? goToDashboard() : goToLogin()"
        >
          {{ userLoggedIn ? 'Dashboard' : 'Get Started' }}
        </button>
      </div>
    </div>
  </nav>
</template>

<script setup lang="ts">
import { isUserLoggedIn } from '@/app/services/backend/auth_service'
import { ref } from 'vue'

const isMenuOpen = ref(false)
const userLoggedIn: boolean = isUserLoggedIn()

const toggleMenu = () => {
  isMenuOpen.value = !isMenuOpen.value
}

const goToLogin = () => {
  window.location.href = '/login'
}

const goToDashboard = () => {
  window.location.href = '/dashboard'
}
</script>