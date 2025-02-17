<template>
    <div class="relative w-full max-w-xl">
        <!-- Search Input -->
        <div class="relative">
            <input type="text" v-model="searchQuery" @keyup.enter="handleSearch" placeholder="Search..."
                class="w-full pl-12 pr-4 py-2.5 rounded-full border border-slate-200 focus:ring-2 focus:ring-emerald-500 focus:border-transparent bg-slate-50 hover:bg-white transition-colors" />
            <SearchIcon @click="handleSearch"
                class="absolute left-4 top-1/2 transform -translate-y-1/2 text-slate-400 w-5 h-5 hover:text-emerald-500" />
        </div>

        <!-- Search Results Dropdown -->
        <div v-if="showResults && results.length > 0"
            class="absolute w-full mt-2 bg-white rounded-lg shadow-lg border border-slate-200 max-h-96 overflow-y-auto z-50">
            <div v-for="result in results" :key="result.id"
                class="p-4 hover:bg-slate-50 border-b border-slate-100 last:border-b-0 cursor-pointer transition-colors"
                @click="navigateToResult(result)">
                <!-- Forum Name -->
                <div class="flex items-center justify-between mb-1">
                    <span class="text-sm font-medium text-emerald-600">
                        {{ result.forum.name }}
                    </span>
                </div>

                <!-- Title -->
                <h3 class="text-lg font-semibold text-slate-900 mb-1">
                    {{ result.title }}
                </h3>

                <!-- Content Preview -->
                <p class="text-sm text-slate-600 line-clamp-2">
                    {{ result.content }}
                </p>
            </div>
        </div>

        <!-- No Results Message -->
        <div v-if="showResults && results.length === 0 && !isLoading"
            class="absolute mt-2 w-full bg-white rounded-lg shadow-lg border border-slate-200 p-4 text-center text-slate-600">
            No results found for "{{ searchQuery }}"
        </div>

        <!-- Loading State -->
        <div v-if="isLoading"
            class="absolute mt-2 w-full bg-white rounded-lg shadow-lg border border-slate-200 p-4 text-center text-slate-600">
            Searching...
        </div>
    </div>
</template>

<script setup lang="ts">
import { onMounted, onUnmounted, ref } from 'vue';
import { Search as SearchIcon } from 'lucide-vue-next';
import { getAccessToken } from '@/app/services/backend/auth_service';
import { goToLoginScreen, goToPost } from '@/app/routes';
import { SearchService } from '@/app/services/forum_backend/search_service';

interface SearchResult {
    id: string;
    title: string;
    content: string;
    forum: {
        id: string;
        name: string;
    };
}

const searchQuery = ref('');
const results = ref<SearchResult[]>([]);
const isLoading = ref(false);
const showResults = ref(false);

const handleSearch = async () => {
    if (!searchQuery.value.trim()) return;

    isLoading.value = true;
    showResults.value = true;

    try {
        const jwt = await getAccessToken();
        if (!jwt) {
            goToLoginScreen();
            return;
        }

        const searchResults = await SearchService.search(jwt, searchQuery.value);
        results.value = searchResults;
    } catch (error) {
        console.error('Search failed:', error);
        results.value = [];
    } finally {
        isLoading.value = false;
    }
};

const navigateToResult = (result: SearchResult) => {
    goToPost(result.id);
    showResults.value = false;
    searchQuery.value = '';
};

const handleClickOutside = (event: MouseEvent) => {
    const target = event.target as HTMLElement;
    if (!target.closest('.relative')) {
        showResults.value = false;
    }
};

onMounted(() => {
    document.addEventListener('click', handleClickOutside);
});

onUnmounted(() => {
    document.removeEventListener('click', handleClickOutside);
});
</script>