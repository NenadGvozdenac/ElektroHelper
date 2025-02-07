<template>
    <div class="min-h-screen bg-slate-100">
        <!-- Header -->
        <header class="bg-white shadow-md sticky top-0 z-50 border-b border-slate-200">
            <div
                class="container mx-auto px-4 h-16 flex items-center justify-between max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                <span
                    class="text-2xl font-bold bg-gradient-to-r from-emerald-600 to-indigo-600 bg-clip-text text-transparent cursor-pointer"
                    @click="goToHome()">
                    ElektroHelper Forums
                </span>
                <div class="relative flex-1 max-w-xl">
                    <input type="text" v-model="searchQuery" placeholder="Search forums..."
                        class="w-full pl-12 pr-4 py-2.5 rounded-full border border-slate-200 focus:ring-2 focus:ring-emerald-500 focus:border-transparent bg-slate-50 hover:bg-white transition-colors" />
                    <SearchIcon class="absolute left-4 top-1/2 transform -translate-y-1/2 text-slate-400 w-5 h-5" />
                </div>
            </div>
        </header>

        <main class="container mx-auto px-4 py-6 flex justify-center">
            <div class="flex gap-20">
                <!-- Main Content -->
                <div class="flex-grow max-w-3xl">
                    <!-- Posts Feed -->
                    <div class="space-y-4">
                        <div v-for="post in filteredPosts" :key="post.id"
                            class="bg-white rounded-xl shadow-sm hover:shadow-md transition-all border border-slate-200 group">
                            <div class="flex">
                                <div
                                    class="py-4 flex flex-col items-center justify-center bg-slate-50 rounded-l-xl group-hover:bg-slate-100 transition-colors">
                                    <button :class="[
                                        'transition-colors transform',
                                        post.isUpvoted
                                            ? 'text-blue-500'
                                            : 'text-slate-400 hover:text-blue-500'
                                    ]" @click="!post.isUpvoted ? upvotePost(post.id) : deleteUpvotePost(post.id)">
                                        <ArrowBigUp class="w-6 h-6" />
                                    </button>
                                    <span class="text-sm font-medium my-1 text-slate-600">
                                        {{ post.numberOfUpvotes - post.numberOfDownvotes }}
                                    </span>
                                    <button :class="[
                                        'transition-colors transform hover:scale-110',
                                        post.isDownvoted
                                            ? 'text-red-500'
                                            : 'text-slate-400 hover:text-red-500'
                                    ]" @click="!post.isDownvoted ? downvotePost(post.id) : deleteDownvotePost(post.id)">
                                        <ArrowBigDown class="w-6 h-6" />
                                    </button>
                                </div>

                                <!-- Post Content -->
                                <div class="p-4 flex-grow cursor-pointer" @click="navigateToPost(post.id)">
                                    <div class="flex items-center space-x-3 text-sm text-slate-500 mb-2">
                                        <span class="font-medium">Posted {{ formatDate(post.createdAt) }}</span>
                                        <span v-if="post.isLocked"
                                            class="px-2 py-0.5 rounded-full bg-amber-100 text-amber-600 text-xs font-medium flex items-center">
                                            <LockIcon class="w-3 h-3 mr-1" />
                                            Locked
                                        </span>
                                    </div>
                                    <h2
                                        class="text-lg font-semibold text-slate-900 mb-2 group-hover:text-emerald-600 transition-colors">
                                        {{ post.title }}
                                    </h2>
                                    <p class="text-slate-600 leading-relaxed">{{ post.content }}</p>

                                    <!-- Post Actions -->
                                    <div class="flex items-center space-x-4 mt-4">
                                        <button
                                            class="flex items-center space-x-2 text-slate-500 hover:text-emerald-600 hover:bg-emerald-50 rounded-full px-4 py-1.5 transition-colors text-sm">
                                            <MessageSquare class="w-4 h-4" />
                                            <span>Comments</span>
                                        </button>
                                        <button
                                            class="flex items-center space-x-2 text-slate-500 hover:text-emerald-600 hover:bg-emerald-50 rounded-full px-4 py-1.5 transition-colors text-sm">
                                            <Share2 class="w-4 h-4" />
                                            <span>Share</span>
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Loading indicator -->
                        <div v-if="loading" class="text-center py-4">
                            <span class="text-slate-500">Loading more posts...</span>
                        </div>

                        <!-- No more posts indicator -->
                        <div v-if="!hasMore" class="text-center py-4">
                            <span class="text-slate-500">No more posts to load</span>
                        </div>
                    </div>
                </div>

                <!-- Sidebar -->
                <div class="flex-shrink-0 space-y-6">
                    <!-- Forum Selection -->
                    <div class="bg-white rounded-xl shadow-sm border border-slate-200">
                        <div class="p-4 border-b border-slate-200 flex justify-center">
                            <span class="text-lg font-semibold text-slate-900">Forums</span>
                        </div>
                        <div class="p-3">
                            <div v-for="forum in forums" :key="forum.id" @click="navigateToForum(forum.id)"
                                class="flex flex-col p-2 pb-0 rounded-lg cursor-pointer transition-all hover:bg-slate-50 text-slate-700 w-80">
                                <div class="flex items-center space-x-3">
                                    <div
                                        class="w-8 h-8 rounded-full bg-gradient-to-br from-emerald-500 to-indigo-500 flex items-center justify-center">
                                        <MessageCircle class="w-4 h-4 text-white" />
                                    </div>
                                    <div class="flex-1">
                                        <span class="font-medium block">{{ forum.name }}</span>
                                        <span v-if="forum.isQuarantined"
                                            class="text-xs px-2 py-0.5 bg-amber-100 text-amber-600 rounded-full inline-flex items-center mt-1">
                                            <ShieldAlertIcon class="w-3 h-3 mr-1" />
                                            Quarantined
                                        </span>
                                    </div>
                                </div>
                                <p class="text-sm text-slate-600 pl-11">{{ forum.description }}</p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </main>
    </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, onUnmounted } from 'vue';
import { useRouter } from 'vue-router';
import {
    SearchIcon,
    MessageSquare,
    MessageCircle,
    Share2,
    ArrowBigUp,
    ArrowBigDown,
    LockIcon,
    ShieldAlertIcon
} from 'lucide-vue-next';
import { ForumService } from '@/app/services/forum_backend/forum_service';
import { PostService } from '@/app/services/forum_backend/post_service';
import { VotingService } from '@/app/services/forum_backend/voting_service';
import { getAccessToken } from '@/app/services/backend/auth_service';
import type { Forum } from '@/app/models/forum_backend/Forum';
import type { Post } from '@/app/models/forum_backend/Post';
import { goToForums, goToHome } from '@/app/routes';

const router = useRouter();
const forums = ref<Forum[]>([]);
const posts = ref<Post[]>([]);
const searchQuery = ref('');
const page = ref(1);
const pageSize = ref(10);
const loading = ref(false);
const hasMore = ref(true);

onMounted(async () => {
    const jwt = await getAccessToken();
    if (!jwt) {
        router.push('/login');
        return;
    }

    await fetchInitialPosts(jwt);
    forums.value = await ForumService.getForums(jwt);
    window.addEventListener('scroll', handleScroll);
});

onUnmounted(() => {
    window.removeEventListener('scroll', handleScroll);
});

const filteredPosts = computed(() => {
    return posts.value.filter(post => {
        if (!searchQuery.value) return true;
        return post.title.toLowerCase().includes(searchQuery.value.toLowerCase()) ||
            post.content.toLowerCase().includes(searchQuery.value.toLowerCase());
    }).filter(post => !post.isDeleted);
});

async function fetchInitialPosts(jwt: string) {
    try {
        const initialPosts = await PostService.getAllPostsPaged(jwt, page.value, pageSize.value);
        posts.value = initialPosts;
        page.value++;
    } catch (error) {
        console.error('Error fetching initial posts:', error);
    }
}

async function fetchMorePosts() {
    if (loading.value || !hasMore.value) return;

    loading.value = true;
    try {
        const jwt = await getAccessToken();
        if (!jwt) {
            router.push('/login');
            return;
        }

        const morePosts = await PostService.getAllPostsPaged(jwt, page.value, pageSize.value);

        if (morePosts.length === 0) {
            hasMore.value = false;
        } else {
            posts.value = [...posts.value, ...morePosts];
            page.value++;
        }
    } catch (error) {
        console.error('Error fetching more posts:', error);
    } finally {
        loading.value = false;
    }
}

function handleScroll() {
    if (window.innerHeight + window.scrollY >= document.body.offsetHeight - 500) {
        fetchMorePosts();
    }
}

async function upvotePost(postId: string) {
    const jwt = await getAccessToken();
    if (!jwt) {
        router.push('/login');
        return;
    }

    await VotingService.upvotePost(jwt, postId);

    var post = posts.value.find(post => post.id === postId);

    if(post == undefined) return;

    post.numberOfUpvotes++;
    post.isUpvoted = true;

    if(post.isDownvoted) {
        post.isDownvoted = false;
        post.numberOfDownvotes--;
    }

}

async function downvotePost(postId: string) {
    const jwt = await getAccessToken();
    if (!jwt) {
        router.push('/login');
        return;
    }

    await VotingService.downvotePost(jwt, postId);

    var post = posts.value.find(post => post.id === postId);

    if(post == undefined) return;

    post.numberOfDownvotes++;
    post.isDownvoted = true;

    if(post.isUpvoted) {
        post.isUpvoted = false;
        post.numberOfUpvotes--;
    }
}

async function deleteUpvotePost(postId: string) {
    const jwt = await getAccessToken();
    if (!jwt) {
        router.push('/login');
        return;
    }

    await VotingService.deleteUpvotePost(jwt, postId);

    var post = posts.value.find(post => post.id === postId);

    if(post == undefined) return;

    post.numberOfUpvotes--;
    post.isUpvoted = false;

    post.isDownvoted = false;
}

async function deleteDownvotePost(postId: string) {
    const jwt = await getAccessToken();
    if (!jwt) {
        router.push('/login');
        return;
    }

    await VotingService.deleteDownvotePost(jwt, postId);

    var post = posts.value.find(post => post.id === postId);

    if(post == undefined) return;

    post.numberOfDownvotes--;
    post.isDownvoted = false;

    post.isUpvoted = false;
}

function navigateToPost(postId: string) {
    router.push(`/posts/${postId}`);
}

function formatDate(dateString: string): string {
    return new Date(dateString).toLocaleDateString('en-US', {
        month: 'short',
        day: 'numeric',
        year: 'numeric'
    });
}

function navigateToForum(forumId: string) {
    router.push(`/forums/${forumId}`);
}
</script>