<template>
    <div class="min-h-screen bg-slate-100">
        <!-- Enhanced Header -->
        <header class="bg-white shadow-sm sticky top-0 z-50">
            <div class="border-b border-slate-200">
                <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 h-20">
                    <div class="flex items-center justify-between h-full">
                        <!-- Logo and Brand -->
                        <div class="flex items-center space-x-8">
                            <span @click="goToHome()"
                                class="text-2xl font-bold bg-gradient-to-r from-emerald-600 to-indigo-600 bg-clip-text text-transparent cursor-pointer hover:opacity-90 transition-opacity">
                                ElektroHelper Forums
                            </span>

                            <!-- Primary Navigation -->
                            <nav class="hidden md:flex items-center space-x-6">
                                <a href="/forums"
                                    class="text-slate-600 hover:text-emerald-600 font-medium transition-colors">Browse</a>
                                <a href="#"
                                    class="text-slate-600 hover:text-emerald-600 font-medium transition-colors">Latest</a>
                                <a href="#"
                                    class="text-slate-600 hover:text-emerald-600 font-medium transition-colors">Popular</a>
                            </nav>
                        </div>

                        <!-- Search Bar -->
                        <div class="flex-1 max-w-2xl px-8">
                            <div class="relative">
                                <input type="text" v-model="searchQuery" placeholder="Search forums..."
                                    class="w-full pl-12 pr-4 py-2.5 rounded-full border border-slate-200 focus:ring-2 focus:ring-emerald-500 focus:border-transparent bg-slate-50 hover:bg-white transition-colors" />
                                <SearchIcon
                                    class="absolute left-4 top-1/2 transform -translate-y-1/2 text-slate-400 w-5 h-5" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </header>

        <main class="container mx-auto px-4 py-6 flex justify-center">
            <div class="flex gap-20">
                <!-- Main Content -->
                <div class="flex-grow max-w-3xl">
                    <!-- Forum Hero -->
                    <div class="bg-white rounded-xl shadow-sm border border-slate-200 mb-6 overflow-hidden">
                        <!-- Gradient Banner -->
                        <div class="h-24 bg-gradient-to-r from-emerald-500 via-teal-500 to-indigo-500 relative">
                            <div class="absolute inset-0 bg-grid-white/10"></div>
                            <div class="absolute bottom-0 left-0 right-0 h-24 bg-gradient-to-t from-white"></div>
                        </div>

                        <!-- Content Section -->
                        <div class="px-6 pb-6 relative">
                            <!-- Forum Icon -->
                            <div class="relative -mt-12 mb-4 flex items-center">
                                <div
                                    class="w-20 h-20 rounded-2xl bg-gradient-to-br from-emerald-500 to-indigo-500 p-0.5">
                                    <div class="w-full h-full bg-white rounded-2xl flex items-center justify-center">
                                        <MessageCircle class="w-10 h-10 text-emerald-500" />
                                    </div>
                                </div>
                                <div class="ml-4 flex-1">
                                    <div class="flex items-center justify-between">
                                        <div>
                                            <h1 class="text-2xl font-bold text-slate-900">{{ currentForum?.name }}</h1>
                                            <p class="text-slate-600 mt-1">{{ currentForum?.description }}</p>
                                        </div>
                                        <button @click="showModal = true"
                                            class="px-6 py-2.5 bg-emerald-600 text-white rounded-lg hover:bg-emerald-700 transition-colors font-medium shadow-sm hover:shadow flex items-center space-x-2">
                                            <PlusCircle class="w-5 h-5" />
                                            <span>New Post</span>
                                        </button>
                                    </div>
                                </div>
                            </div>

                            <!-- Stats Grid -->
                            <div class="grid grid-cols-4 gap-4 mt-6">
                                <!-- Posts Count -->
                                <div class="p-4 rounded-lg bg-slate-50 border border-slate-200">
                                    <div class="flex items-center space-x-3">
                                        <div
                                            class="w-10 h-10 rounded-full bg-emerald-100 flex items-center justify-center">
                                            <MessageSquare class="w-5 h-5 text-emerald-600" />
                                        </div>
                                        <div>
                                            <div class="text-2xl font-bold text-slate-900">
                                                {{ formatNumber(currentForum?.numberOfPosts ?? 0) }}
                                            </div>
                                            <div class="text-sm text-slate-600">Total Posts</div>
                                        </div>
                                    </div>
                                </div>

                                <!-- Members -->
                                <div class="p-4 rounded-lg bg-slate-50 border border-slate-200">
                                    <div class="flex items-center space-x-3">
                                        <div
                                            class="w-10 h-10 rounded-full bg-indigo-100 flex items-center justify-center">
                                            <Users class="w-5 h-5 text-indigo-600" />
                                        </div>
                                        <div>
                                            <div class="text-2xl font-bold text-slate-900">12.4k</div>
                                            <div class="text-sm text-slate-600">Members</div>
                                        </div>
                                    </div>
                                </div>

                                <!-- Daily Activity -->
                                <div class="p-4 rounded-lg bg-slate-50 border border-slate-200">
                                    <div class="flex items-center space-x-3">
                                        <div
                                            class="w-10 h-10 rounded-full bg-violet-100 flex items-center justify-center">
                                            <TrendingUp class="w-5 h-5 text-violet-600" />
                                        </div>
                                        <div>
                                            <div class="text-2xl font-bold text-slate-900">+24%</div>
                                            <div class="text-sm text-slate-600">Daily Activity</div>
                                        </div>
                                    </div>
                                </div>

                                <!-- Online Now -->
                                <div class="p-4 rounded-lg bg-slate-50 border border-slate-200">
                                    <div class="flex items-center space-x-3">
                                        <div
                                            class="w-10 h-10 rounded-full bg-teal-100 flex items-center justify-center">
                                            <Activity class="w-5 h-5 text-teal-600" />
                                        </div>
                                        <div>
                                            <div class="text-2xl font-bold text-slate-900">1.2k</div>
                                            <div class="text-sm text-slate-600">Online Now</div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- Status Tags -->
                            <div class="flex items-center space-x-3 mt-6">
                                <span v-if="currentForum?.isQuarantined"
                                    class="px-3 py-1 rounded-full bg-amber-100 text-amber-600 text-sm font-medium flex items-center">
                                    <ShieldAlert class="w-4 h-4 mr-1.5" />
                                    Quarantined Forum
                                </span>
                                <span
                                    class="px-3 py-1 rounded-full bg-emerald-100 text-emerald-600 text-sm font-medium flex items-center">
                                    <Shield class="w-4 h-4 mr-1.5" />
                                    Safe for Work
                                </span>
                            </div>
                        </div>
                    </div>

                    <!-- Posts Feed -->
                    <div class="space-y-6">
                        <!-- Individual Post -->
                        <div v-for="post in filteredPosts" :key="post.id"
                            class="bg-white rounded-xl shadow-sm hover:shadow-md transition-all border border-slate-200 group overflow-hidden">

                            <!-- Post Header -->
                            <div class="border-b border-slate-100 p-4">
                                <div class="flex items-center justify-between">
                                    <!-- Left side with author info -->
                                    <div class="flex items-center space-x-3">
                                        <div
                                            class="w-10 h-10 rounded-full bg-gradient-to-br from-emerald-500 to-indigo-500 flex items-center justify-center text-white font-bold">
                                            {{ post.author?.username?.[0]?.toUpperCase() ?? 'A' }}
                                        </div>
                                        <div class="flex-1">
                                            <div class="flex items-center space-x-3">
                                                <span class="font-medium text-slate-900">{{ post.author?.username
                                                    }}</span>
                                                <span class="text-sm text-slate-500">{{ formatDate(post.createdAt)
                                                    }}</span>
                                                <span v-if="post.isLocked"
                                                    class="px-2 py-0.5 rounded-full bg-amber-100 text-amber-600 text-xs font-medium flex items-center">
                                                    <LockIcon class="w-3 h-3 mr-1" />
                                                    Locked
                                                </span>
                                            </div>
                                        </div>
                                    </div>

                                    <!-- Right side with forum name and actions -->
                                    <div class="flex items-center space-x-3">
                                        <button
                                            class="p-1.5 hover:bg-slate-50 rounded-lg text-slate-400 hover:text-slate-600">
                                            <MoreVertical class="w-5 h-5" />
                                        </button>
                                    </div>
                                </div>
                            </div>

                            <!-- Post Content -->
                            <div class="flex">
                                <!-- Voting Section -->
                                <div
                                    class="py-4 px-2 flex flex-col items-center justify-start bg-slate-50 group-hover:bg-slate-100 transition-colors min-w-[60px]">
                                    <button :class="[
                                        'p-1.5 rounded-lg transition-all transform hover:scale-110',
                                        post.isUpvoted
                                            ? 'text-emerald-500 bg-emerald-50'
                                            : 'text-slate-400 hover:text-emerald-500 hover:bg-emerald-50'
                                    ]" @click="!post.isUpvoted ? upvotePost(post.id) : deleteUpvotePost(post.id)">
                                        <ArrowBigUp class="w-6 h-6" />
                                    </button>

                                    <span class="text-sm font-medium my-1" :class="{
                                        'text-emerald-500': post.numberOfUpvotes - post.numberOfDownvotes > 0,
                                        'text-red-500': post.numberOfUpvotes - post.numberOfDownvotes < 0,
                                        'text-slate-600': post.numberOfUpvotes - post.numberOfDownvotes === 0
                                    }">
                                        {{ post.numberOfUpvotes - post.numberOfDownvotes }}
                                    </span>

                                    <button :class="[
                                        'p-1.5 rounded-lg transition-all transform hover:scale-110',
                                        post.isDownvoted
                                            ? 'text-red-500 bg-red-50'
                                            : 'text-slate-400 hover:text-red-500 hover:bg-red-50'
                                    ]"
                                        @click="!post.isDownvoted ? downvotePost(post.id) : deleteDownvotePost(post.id)">
                                        <ArrowBigDown class="w-6 h-6" />
                                    </button>
                                </div>

                                <!-- Main Content -->
                                <div class="p-4 flex-grow">
                                    <h2 @click="navigateToPost(post.id)"
                                        class="text-xl font-semibold text-slate-900 group-hover:text-emerald-600 transition-colors cursor-pointer">
                                        {{ post.title }}
                                    </h2>

                                    <!-- Post Preview -->
                                    <div class="mt-3 prose-sm text-slate-600 leading-relaxed line-clamp-3">
                                        {{ post.content }}
                                    </div>

                                    <!-- Post Footer -->
                                    <div class="flex items-center space-x-6 mt-4">
                                        <button @click="navigateToPost(post.id)"
                                            class="flex items-center space-x-2 text-slate-500 hover:text-emerald-600 hover:bg-emerald-50 rounded-full px-4 py-1.5 transition-colors text-sm">
                                            <MessageSquare class="w-4 h-4" />
                                            <span>{{ post.numberOfComments ?? 0 }} Comments</span>
                                        </button>

                                        <button
                                            class="flex items-center space-x-2 text-slate-500 hover:text-emerald-600 hover:bg-emerald-50 rounded-full px-4 py-1.5 transition-colors text-sm">
                                            <Bookmark class="w-4 h-4" />
                                            <span>Save</span>
                                        </button>

                                        <button @click="copyToClipboard(post.id)"
                                            class="flex items-center space-x-2 text-slate-500 hover:text-emerald-600 hover:bg-emerald-50 rounded-full px-4 py-1.5 transition-colors text-sm">
                                            <Share2 class="w-4 h-4" />
                                            <span>Share</span>
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Loading State -->
                        <div v-if="loading" class="flex justify-center py-8">
                            <div class="animate-spin rounded-full h-8 w-8 border-b-2 border-emerald-500"></div>
                        </div>

                        <!-- No More Posts -->
                        <div v-if="!hasMore && !loading" class="text-center py-2">
                            <div
                                class="inline-flex items-center justify-center w-16 h-16 rounded-full bg-slate-100 mb-4">
                                <CheckCircle class="w-8 h-8 text-slate-400" />
                            </div>
                            <h3 class="text-lg font-medium text-slate-900">You're All Caught Up!</h3>
                            <p class="text-slate-600 mt-1">Check back later.</p>
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
                            <div v-for="forum in forums" :key="forum.id" @click="navigateToForum(forum.id)" :class="['flex flex-col p-2 pb-0 rounded-lg cursor-pointer transition-all hover:bg-slate-50 text-slate-700 w-80',
                                forum.id === forumId ? 'bg-slate-100' : '']">
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

    <CreatePostModal :is-open="showModal" :initial-forum-id="currentForum?.id" :forums="forums"
        @close="showModal = false" @submit="handleCreatePost" />

    <div>
        <ToastNotification ref="toastRef" />
    </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, onUnmounted } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import {
    SearchIcon,
    MessageSquare,
    MessageCircle,
    Share2,
    ArrowBigUp,
    ArrowBigDown,
    LockIcon,
    ShieldAlertIcon,
    Shield,
    ShieldAlert,
    PlusCircle,
    Users,
    TrendingUp,
    Activity,
    MoreVertical,
    Bookmark,
    CheckCircle
} from 'lucide-vue-next';
import { ForumService } from '@/app/services/forum_backend/forum_service';
import { PostService } from '@/app/services/forum_backend/post_service';
import { VotingService } from '@/app/services/forum_backend/voting_service';
import { getAccessToken } from '@/app/services/backend/auth_service';
import type { Forum } from '@/app/models/forum_backend/Forum';
import type { CreatePost, Post } from '@/app/models/forum_backend/Post';
import { goToForum, goToHome } from '@/app/routes';
import CreatePostModal from '@/components/forums/CreatePostModal.vue';
import ToastNotification from '@/components/forums/ToastNotification.vue';

const route = useRoute();
const router = useRouter();
const forumId = computed(() => route.params.forumId as string);
const currentForum = ref<Forum | null>(null);
const forums = ref<Forum[]>([]);
const posts = ref<Post[]>([]);
const searchQuery = ref('');
const page = ref(1);
const pageSize = ref(10);
const loading = ref(false);
const hasMore = ref(true);

const showModal = ref(false);
const toastRef = ref();

onMounted(async () => {
    const jwt = await getAccessToken();
    if (!jwt) {
        router.push('/login');
        return;
    }

    await Promise.all([
        fetchCurrentForum(jwt),
        fetchInitialPosts(jwt),
        fetchForums(jwt)
    ]);

    window.addEventListener('scroll', handleScroll);
});

onUnmounted(() => {
    window.removeEventListener('scroll', handleScroll);
});

async function fetchCurrentForum(jwt: string) {
    try {
        currentForum.value = await ForumService.getForum(jwt, forumId.value);
    } catch (error) {
        console.error('Error fetching forum:', error);
        router.push('/');
    }
}

async function fetchForums(jwt: string) {
    forums.value = await ForumService.getForums(jwt);
}

const filteredPosts = computed(() => {
    return posts.value.filter(post => {
        if (!searchQuery.value) return true;
        return post.title.toLowerCase().includes(searchQuery.value.toLowerCase()) ||
            post.content.toLowerCase().includes(searchQuery.value.toLowerCase());
    }).filter(post => !post.isDeleted);
});

async function fetchInitialPosts(jwt: string) {
    try {
        const initialPosts = await PostService.getForumPostsPaged(jwt, forumId.value, page.value, pageSize.value);
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

        const morePosts = await PostService.getForumPostsPaged(jwt, forumId.value, page.value, pageSize.value);

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

function formatNumber(num: number): string {
    return new Intl.NumberFormat('en-US', { notation: 'compact' }).format(num);
}

async function upvotePost(postId: string) {
    const jwt = await getAccessToken();
    if (!jwt) {
        router.push('/login');
        return;
    }

    await VotingService.upvotePost(jwt, postId);

    var post = posts.value.find(post => post.id === postId);

    if (post == undefined) return;

    post.numberOfUpvotes++;
    post.isUpvoted = true;

    if (post.isDownvoted) {
        post.isDownvoted = false;
        post.numberOfDownvotes--;
    }

    toastRef.value.showToast('Post upvoted!');
}

async function downvotePost(postId: string) {
    const jwt = await getAccessToken();
    if (!jwt) {
        router.push('/login');
        return;
    }

    await VotingService.downvotePost(jwt, postId);

    var post = posts.value.find(post => post.id === postId);

    if (post == undefined) return;

    post.numberOfDownvotes++;
    post.isDownvoted = true;

    if (post.isUpvoted) {
        post.isUpvoted = false;
        post.numberOfUpvotes--;
    }

    toastRef.value.showToast('Post downvoted!');
}

async function deleteUpvotePost(postId: string) {
    const jwt = await getAccessToken();
    if (!jwt) {
        router.push('/login');
        return;
    }

    await VotingService.deleteUpvotePost(jwt, postId);

    var post = posts.value.find(post => post.id === postId);

    if (post == undefined) return;

    post.numberOfUpvotes--;
    post.isUpvoted = false;

    post.isDownvoted = false;

    toastRef.value.showToast('Upvote removed!');
}

async function deleteDownvotePost(postId: string) {
    const jwt = await getAccessToken();
    if (!jwt) {
        router.push('/login');
        return;
    }

    await VotingService.deleteDownvotePost(jwt, postId);

    var post = posts.value.find(post => post.id === postId);

    if (post == undefined) return;

    post.numberOfDownvotes--;
    post.isDownvoted = false;

    post.isUpvoted = false;

    toastRef.value.showToast('Downvote removed!');
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
    goToForum(forumId);
}

async function handleCreatePost(data: CreatePost) {
    const jwt = await getAccessToken();

    if (!jwt) {
        router.push('/login');
        return;
    }

    await PostService.createPost(jwt, data);
    goToForum(forumId.value);

    showNotification('Post created successfully!');
}


async function copyToClipboard(postId: string) {
    try {
        const url = `${window.location.origin}/posts/${postId}`;
        await navigator.clipboard.writeText(url);
        toastRef.value.showToast('Link copied to clipboard!');
    } catch (err) {
        console.error('Failed to copy:', err);
        toastRef.value.showToast('Failed to copy link!');
    }
}

function showNotification(message: string) {
    toastRef.value.showToast(message);
}
</script>