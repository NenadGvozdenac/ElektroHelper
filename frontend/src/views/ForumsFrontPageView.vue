<template>
    <div class="min-h-screen bg-slate-100">
        <!-- Header -->
        <UniversalNavbar />

        <main class="container mx-auto px-4 py-6 flex justify-center">
            <div class="flex gap-6">
                <FollowersSidebar />

                <!-- Main Content -->
                <div class="flex-grow max-w-3xl" v-if="posts.length > 0">
                    <!-- Posts Feed -->
                    <div class="space-y-6">
                        <PostItem v-for="post in filteredPosts" :key="post.id" :post="post"
                            @navigate-to-forum="navigateToForum" @navigate-to-post="navigateToPost" @upvote="upvotePost"
                            @downvote="downvotePost" @delete-upvote="deleteUpvotePost"
                            @delete-downvote="deleteDownvotePost" @copy-to-clipboard="copyToClipboard" />

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

    <div>
        <ToastNotification ref="toastRef" />
    </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, onUnmounted } from 'vue';
import {
    SearchIcon,
    MessageCircle,
    ShieldAlertIcon,
    CheckCircle
} from 'lucide-vue-next';
import { ForumService } from '@/app/services/forum_backend/forum_service';
import { PostService } from '@/app/services/forum_backend/post_service';
import { VotingService } from '@/app/services/forum_backend/voting_service';
import { getAccessToken, getUserData } from '@/app/services/backend/auth_service';
import type { Forum } from '@/app/models/forum_backend/Forum';
import type { Post } from '@/app/models/forum_backend/Post';
import { goToForum, goToLoginScreen, goToPost, goToRss } from '@/app/routes';
import ToastNotification from '@/components/forums/ToastNotification.vue';
import type { UserData } from '@/app/models/backend/user';
import FollowersSidebar from '@/components/forums/FollowersSidebar.vue';

import PostItem from '@/components/forums/PostItem.vue';
import UniversalNavbar from '@/components/forums/UniversalNavbar.vue';

const forums = ref<Forum[]>([]);
const posts = ref<Post[]>([]);
const searchQuery = ref('');
const page = ref(1);
const pageSize = ref(10);
const loading = ref(false);
const hasMore = ref(true);

const toastRef = ref();
const user = ref<UserData | null>();
const showForumModal = ref(false);

onMounted(async () => {
    const jwt = await getAccessToken();
    if (!jwt) {
        goToLoginScreen();
        return;
    }

    await fetchInitialPosts(jwt);
    forums.value = await ForumService.getForums(jwt);
    window.addEventListener('scroll', handleScroll);

    user.value = await getUserData()
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
            goToLoginScreen();
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
        goToLoginScreen();
        return;
    }

    await VotingService.upvotePost(jwt, postId);

    var post = posts.value.find(post => post.id === postId);

    if (post == undefined) return;

    post.upvotes++;
    post.isUpvoted = true;

    if (post.isDownvoted) {
        post.isDownvoted = false;
        post.downvotes--;
    }

}

async function downvotePost(postId: string) {
    const jwt = await getAccessToken();
    if (!jwt) {
        goToLoginScreen();
        return;
    }

    await VotingService.downvotePost(jwt, postId);

    var post = posts.value.find(post => post.id === postId);

    if (post == undefined) return;

    post.downvotes++;
    post.isDownvoted = true;

    if (post.isUpvoted) {
        post.isUpvoted = false;
        post.upvotes--;
    }
}

async function deleteUpvotePost(postId: string) {
    const jwt = await getAccessToken();
    if (!jwt) {
        goToLoginScreen();
        return;
    }

    await VotingService.deleteUpvotePost(jwt, postId);

    var post = posts.value.find(post => post.id === postId);

    if (post == undefined) return;

    post.upvotes--;
    post.isUpvoted = false;

    post.isDownvoted = false;
}

async function deleteDownvotePost(postId: string) {
    const jwt = await getAccessToken();
    if (!jwt) {
        goToLoginScreen();
        return;
    }

    await VotingService.deleteDownvotePost(jwt, postId);

    var post = posts.value.find(post => post.id === postId);

    if (post == undefined) return;

    post.downvotes--;
    post.isDownvoted = false;

    post.isUpvoted = false;
}

function navigateToPost(postId: string) {
    goToPost(postId);
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
</script>