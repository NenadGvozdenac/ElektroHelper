<template>
    <header class="bg-white shadow-sm sticky top-0 z-50">
        <div class="border-b border-slate-200">
            <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 h-20">
                <div class="flex items-center justify-between h-full">
                    <!-- Left Section -->
                    <div class="flex items-center space-x-8">
                        <!-- Back Button (shown only if not on forums page) -->
                        <button v-if="!isForumsPage" @click="goToForums"
                            class="flex items-center space-x-2 text-slate-600 hover:text-emerald-600 font-medium transition-colors">
                            <ArrowLeft class="w-5 h-5" />
                            <span>Back to Forums</span>
                        </button>

                        <button v-if="isForumsPage" @click="goToHome()"
                            class="flex items-center space-x-2 text-slate-600 hover:text-emerald-600 font-medium transition-colors">
                            <ArrowLeft class="w-5 h-5" />
                            <span>Home</span>
                        </button>

                        <!-- Logo/Brand -->
                        <span @click="goToForums"
                            class="text-2xl font-bold bg-gradient-to-r from-emerald-600 to-indigo-600 bg-clip-text text-transparent cursor-pointer hover:opacity-90 transition-opacity">
                            ElektroHelper Forums
                        </span>

                        <!-- Page Title (shown only if on forums page) -->
                    </div>

                    <!-- Search Bar -->
                    <SearchBar />

                    <!-- Right Section -->
                    <div class="flex items-center space-x-4">
                        <!-- Admin Button (shown only if user is admin and on forums page) -->
                        <button v-if="isAdmin && isForumsPage" @click="goToAdmin"
                            class="px-4 py-2 bg-slate-800 text-white rounded-lg hover:bg-slate-900 transition-colors font-medium shadow-sm flex items-center space-x-2">
                            <ShieldCheck class="w-5 h-5" />
                            <span>Admin Panel</span>
                        </button>

                        <!-- RSS Button -->
                        <div>
                            <button @click="navigateToRss"
                                class="inline-flex items-center px-4 py-2 bg-white border border-slate-200 rounded-lg shadow-sm hover:bg-slate-50 transition-colors text-slate-700">
                                <RssIcon class="w-4 h-4 mr-2" />
                                RSS
                            </button>
                        </div>

                        <!-- User Dropdown -->
                        <div class="relative">
                            <button @click="isDropdownOpen = !isDropdownOpen"
                                class="flex items-center space-x-2 p-2 rounded-lg hover:bg-slate-100 transition-colors">
                                <div
                                    class="w-8 h-8 rounded-full bg-gradient-to-br from-emerald-500 to-indigo-500 flex items-center justify-center text-white font-medium">
                                    {{ currentUser?.userName[0]?.toUpperCase() ?? 'U' }}
                                </div>
                                <ChevronDown class="w-4 h-4 text-slate-600" />
                            </button>

                            <!-- Dropdown Menu -->
                            <div v-if="isDropdownOpen"
                                class="absolute right-0 mt-2 w-48 bg-white rounded-lg shadow-lg border border-slate-200 py-1">

                                <button @click="goToProfile1()"
                                    class="w-full px-4 py-2 text-left text-slate-700 hover:bg-slate-50 flex items-center space-x-2">
                                    <User class="w-4 h-4" />
                                    <span>Profile</span>
                                </button>

                                <div class="border-t border-slate-200 my-1"></div>

                                <button @click="createNewForum()" v-if="currentUser?.userRole == 'admin'"
                                    class="w-full px-4 py-2 text-left text-slate-700 hover:bg-slate-50 flex items-center space-x-2">
                                    <PlusCircle class="w-4 h-4" />
                                    <span>Create Forum</span>
                                </button>

                                <button @click="createNewPost" v-if="isOnForumPage()"
                                    class="w-full px-4 py-2 text-left text-slate-700 hover:bg-slate-50 flex items-center space-x-2">
                                    <PlusCircle class="w-4 h-4" />
                                    <span>Create Post</span>
                                </button>

                                <div class="border-t border-slate-200 my-1"></div>

                                <button @click="handleLogout"
                                    class="w-full px-4 py-2 text-left text-red-600 hover:bg-red-50 flex items-center space-x-2">
                                    <LogOut class="w-4 h-4" />
                                    <span>Logout</span>
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </header>

    <CreateForumModal :is-open="showForumModal" @close="showForumModal = false" @submit="handleForumCreate" />
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import {
    ArrowLeft,
    Search as SearchIcon,
    LogOut,
    User,
    PlusCircle,
    ChevronDown,
    ShieldCheck,
    RssIcon
} from 'lucide-vue-next';
import { AuthService, getAccessToken, getUserData } from '@/app/services/backend/auth_service';
import type { UserData } from '@/app/models/backend/user';
import { goToHome, goToLoginScreen, goToProfile, goToRss } from '@/app/routes';
import CreateForumModal from './CreateForumModal.vue';
import type { CreateForum } from '@/app/models/forum_backend/Forum';
import { ForumService } from '@/app/services/forum_backend/forum_service';
import { SearchService } from '@/app/services/forum_backend/search_service';
import SearchBar from './SearchBar.vue';

const router = useRouter();
const route = useRoute();
const isDropdownOpen = ref(false);
const searchQuery = ref('');

const currentUser = ref<UserData | null>(null);
const showForumModal = ref(false);

onMounted(async () => {
    currentUser.value = await getUserData();
    document.addEventListener('click', (event) => {
        const target = event.target as HTMLElement;
        if (!target.closest('.relative')) {
            isDropdownOpen.value = false;
        }
    });
});

const isForumsPage = computed(() => route.path === '/forums');
const isAdmin = computed(() => currentUser?.value?.userRole === 'Admin');

function goToForums() {
    router.push('/forums');
    isDropdownOpen.value = false;
}

function goToAdmin() {
    router.push('/admin');
    isDropdownOpen.value = false;
}

function goToProfile1() {
    if (currentUser.value?.userID)
        goToProfile(currentUser.value?.userID.toString());
    isDropdownOpen.value = false;
}

function createNewPost() {
    isDropdownOpen.value = false;
}

async function handleLogout() {
    try {
        await AuthService.logout();
        router.push('/login');
    } catch (error) {
        console.error('Error logging out:', error);
    }
}

function isOnForumPage() {
    return route.path.startsWith('/forums/');
}

function createNewForum() {
    showForumModal.value = true;
    isDropdownOpen.value = false;
}

async function handleForumCreate(data: CreateForum) {
    const jwt = await getAccessToken();

    if (!jwt) {
        goToLoginScreen();
        return;
    }

    try {
        await ForumService.createForum(jwt, data);
        showForumModal.value = false;
        window.location.reload();
    } catch (error) {
        console.error('Error creating forum:', error);
    }
}

function navigateToRss() {
    goToRss();
}
</script>