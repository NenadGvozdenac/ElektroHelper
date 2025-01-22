<script setup lang="ts">
import { ref, computed } from 'vue'
import axios from 'axios'
import type { UserLogin, UserRegister } from '@/app/models/user'
import { AuthService } from '@/app/services/backend/auth_service'

const isLogin = ref(true)
const loading = ref(false)
const error = ref('')
const isPasswordVisible = ref(false)

const formData = ref({
    name: '',
    surname: '',
    email: '',
    phone: '',
    password: '',
    confirm_password: ''
})

const passwordMatch = computed(() => {
    return !formData.value.confirm_password ||
        formData.value.password === formData.value.confirm_password
})

const loginUser = (email: string, password: string): void => {
    const data: UserLogin = {
        email,
        password
    }

    AuthService.login(data)
        .then(_ => {
            window.location.href = '/'
        }).catch(error => {
            console.log(error.response.data)
            loading.value = false
        })
}

const registerUser = (name: string, surname: string, email: string, phone: string, password: string, confirm_password: string): void => {
    const data: UserRegister = {
        name,
        surname,
        email,
        phone,
        password,
        confirm_password
    }

    AuthService.register(data)
        .then(_ => {
            window.location.href = '/'
        }).catch(error => {
            console.log(error.response.data)
            loading.value = false
        })
}

const handleSubmit = async () => {
    error.value = ''
    loading.value = true

    if (!isLogin.value && !passwordMatch.value) {
        error.value = 'Passwords do not match'
        loading.value = false
        return
    }

    if (isLogin.value) {
        loginUser(formData.value.email, formData.value.password)
    } else {
        registerUser(formData.value.name, formData.value.surname, formData.value.email, formData.value.phone, formData.value.password, formData.value.confirm_password)
    }
}

const toggleForm = () => {
    isLogin.value = !isLogin.value
    error.value = ''
    formData.value = {
        name: '',
        surname: '',
        email: '',
        phone: '',
        password: '',
        confirm_password: ''
    }
}

const togglePasswordVisibility = () => {
    isPasswordVisible.value = !isPasswordVisible.value
}
</script>

<template>
    <div class="min-h-screen flex items-center justify-center bg-gradient-to-br from-background to-primary/5 p-4">
        <div class="card w-full max-w-md backdrop-blur-sm bg-surface/95 border border-primary/10"
            :class="{ 'scale-up-center': !isLogin }">
            <div class="relative mb-8">
                <div class="absolute inset-0 bg-gradient-to-r from-primary to-primary-dark blur-lg opacity-20"></div>
                <h1 class="text-center relative text-gradient">
                    {{ isLogin ? 'Welcome Back' : 'Create Account' }}
                </h1>
                <p class="text-center text-text-light relative">
                    {{ isLogin ? 'Sign in to continue' : 'Join our community' }}
                </p>
            </div>

            <form @submit.prevent="handleSubmit" class="space-y-6">
                <template v-if="!isLogin">
                    <div class="grid grid-cols-2 gap-4">
                        <div class="group">
                            <label class="label transition-colors group-focus-within:text-primary"
                                for="name">Name</label>
                            <input id="name" v-model="formData.name" type="text"
                                class="input hover:border-primary-light focus:border-primary-light transition-colors"
                                required />
                        </div>
                        <div class="group">
                            <label class="label transition-colors group-focus-within:text-primary"
                                for="surname">Surname</label>
                            <input id="surname" v-model="formData.surname" type="text"
                                class="input hover:border-primary-light focus:border-primary-light transition-colors"
                                required />
                        </div>
                    </div>
                    <div class="group">
                        <label class="label transition-colors group-focus-within:text-primary" for="phone">Phone</label>
                        <input id="phone" v-model="formData.phone" type="tel"
                            class="input hover:border-primary-light focus:border-primary-light transition-colors"
                            required />
                    </div>
                </template>

                <div class="group">
                    <label class="label transition-colors group-focus-within:text-primary" for="email">Email</label>
                    <input id="email" v-model="formData.email" type="email"
                        class="input hover:border-primary-light focus:border-primary-light transition-colors"
                        required />
                </div>

                <div class="group">
                    <label class="label transition-colors group-focus-within:text-primary"
                        for="password">Password</label>
                    <div class="relative">
                        <input id="password" v-model="formData.password" :type="isPasswordVisible ? 'text' : 'password'"
                            class="input pr-10 hover:border-primary-light focus:border-primary-light transition-colors"
                            required />
                        <button type="button" @click="togglePasswordVisibility"
                            class="absolute right-3 top-1/2 -translate-y-1/2 text-text-light hover:text-primary transition-colors">
                            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5"
                                stroke="currentColor" class="w-5 h-5">
                                <path v-if="isPasswordVisible" stroke-linecap="round" stroke-linejoin="round"
                                    d="M3.98 8.223A10.477 10.477 0 001.934 12C3.226 16.338 7.244 19.5 12 19.5c.993 0 1.953-.138 2.863-.395M6.228 6.228A10.45 10.45 0 0112 4.5c4.756 0 8.773 3.162 10.065 7.498a10.523 10.523 0 01-4.293 5.774M6.228 6.228L3 3m3.228 3.228l3.65 3.65m7.894 7.894L21 21m-3.228-3.228l-3.65-3.65m0 0a3 3 0 10-4.243-4.243m4.242 4.242L9.88 9.88" />
                                <path v-else stroke-linecap="round" stroke-linejoin="round"
                                    d="M2.036 12.322a1.012 1.012 0 010-.639C3.423 7.51 7.36 4.5 12 4.5c4.638 0 8.573 3.007 9.963 7.178.07.207.07.431 0 .639C20.577 16.49 16.64 19.5 12 19.5c-4.638 0-8.573-3.007-9.963-7.178z" />
                                <path v-if="!isPasswordVisible" stroke-linecap="round" stroke-linejoin="round"
                                    d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" />
                            </svg>
                        </button>
                    </div>
                </div>

                <div v-if="!isLogin" class="group">
                    <label class="label transition-colors group-focus-within:text-primary" for="confirmPassword">
                        Confirm Password
                    </label>
                    <div class="relative">
                        <input id="confirmPassword" v-model="formData.confirm_password"
                            :type="isPasswordVisible ? 'text' : 'password'" class="input pr-10 transition-colors"
                            :class="{
                                'border-error focus:border-error': !passwordMatch,
                                'hover:border-primary-light focus:border-primary-light': passwordMatch
                            }" required />
                        <button type="button" @click="togglePasswordVisibility"
                            class="absolute right-3 top-1/2 -translate-y-1/2 text-text-light hover:text-primary transition-colors">
                            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5"
                                stroke="currentColor" class="w-5 h-5">
                                <path v-if="isPasswordVisible" stroke-linecap="round" stroke-linejoin="round"
                                    d="M3.98 8.223A10.477 10.477 0 001.934 12C3.226 16.338 7.244 19.5 12 19.5c.993 0 1.953-.138 2.863-.395M6.228 6.228A10.45 10.45 0 0112 4.5c4.756 0 8.773 3.162 10.065 7.498a10.523 10.523 0 01-4.293 5.774M6.228 6.228L3 3m3.228 3.228l3.65 3.65m7.894 7.894L21 21m-3.228-3.228l-3.65-3.65m0 0a3 3 0 10-4.243-4.243m4.242 4.242L9.88 9.88" />
                                <path v-else stroke-linecap="round" stroke-linejoin="round"
                                    d="M2.036 12.322a1.012 1.012 0 010-.639C3.423 7.51 7.36 4.5 12 4.5c4.638 0 8.573 3.007 9.963 7.178.07.207.07.431 0 .639C20.577 16.49 16.64 19.5 12 19.5c-4.638 0-8.573-3.007-9.963-7.178z" />
                                <path v-if="!isPasswordVisible" stroke-linecap="round" stroke-linejoin="round"
                                    d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" />
                            </svg>
                        </button>
                    </div>
                </div>

                <div v-if="error" class="text-error text-sm mt-2 bg-error/10 p-3 rounded-lg">
                    {{ error }}
                </div>

                <button type="submit"
                    class="btn btn-primary w-full transform hover:scale-[1.02] active:scale-[0.98] transition-all duration-200 shadow-lg hover:shadow-primary/25"
                    :disabled="loading">
                    <span v-if="loading" class="inline-block animate-spin mr-2">
                        <svg class="w-5 h-5" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                            <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4">
                            </circle>
                            <path class="opacity-75" fill="currentColor"
                                d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z">
                            </path>
                        </svg>
                    </span>
                    {{ loading ? 'Processing...' : (isLogin ? 'Login' : 'Register') }}
                </button>
            </form>

            <div class="mt-8 text-center">
                <button @click="toggleForm" class="btn btn-outline hover-scale group relative overflow-hidden">
                    <span class="relative z-10 transition-colors group-hover:text-white">
                        {{ isLogin ? 'Need an account?' : 'Already have an account?' }}
                    </span>
                    <div
                        class="absolute inset-0 bg-gradient-to-r from-primary to-primary-dark scale-x-0 group-hover:scale-x-100 transition-transform origin-left">
                    </div>
                </button>
            </div>
        </div>
    </div>
</template>

<style scoped>
.scale-up-center {
    animation: scale-up-center 0.3s cubic-bezier(0.39, 0.575, 0.565, 1) both;
}

@keyframes scale-up-center {
    0% {
        transform: scale(0.8);
        opacity: 0;
    }

    100% {
        transform: scale(1);
        opacity: 1;
    }
}
</style>