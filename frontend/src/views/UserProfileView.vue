<template>
    <div class="min-h-screen bg-slate-100">
        <UniversalNavbar @search="handleSearch" />
        <!-- Profile Hero Section -->
        <div class="bg-white shadow-sm border border-slate-200 mb-6">
            <!-- Profile Content -->
            <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 pb-6 relative mt-32">
                <!-- Profile Info -->
                <div class="relative -mt-16 mb-4 flex items-start">
                    <!-- Avatar -->
                    <div class="w-32 h-32 rounded-2xl bg-gradient-to-br from-emerald-500 to-indigo-500 p-0.5 shadow-lg">
                        <div class="w-full h-full bg-white rounded-2xl flex items-center justify-center">
                            <span class="text-4xl font-bold text-emerald-500">
                                {{ profile?.username?.[0]?.toUpperCase() ?? 'U' }}
                            </span>
                        </div>
                    </div>

                    <!-- User Info -->
                    <div class="ml-6 flex-1">
                        <div class="flex items-center justify-between">
                            <div>
                                <div class="flex items-center space-x-4">
                                    <h1 class="text-3xl pt-5 font-bold text-slate-900">{{ profile?.username }}</h1>
                                    <span
                                        class="px-3 py-1 rounded-full bg-emerald-100 text-emerald-600 text-sm font-medium">
                                        {{ profile?.role }}
                                    </span>
                                    <span v-if="profile?.isBanned"
                                        class="px-3 py-1 rounded-full bg-red-100 text-red-600 text-sm font-medium flex items-center">
                                        <BanIcon class="w-4 h-4 mr-1.5" />
                                        Banned
                                    </span>
                                </div>
                                <p class="text-slate-600 mt-2">{{ profile?.email }}</p>
                            </div>

                            <!-- Action Buttons -->
                            <div class="flex items-center space-x-4">
                                <button v-if="!isOwnProfile" @click="toggleFollow" :class="[
                                    'px-6 py-2.5 rounded-lg font-medium shadow-sm transition-colors flex items-center space-x-2',
                                    profile?.isFollowed
                                        ? 'bg-slate-200 hover:bg-slate-300 text-slate-700'
                                        : 'bg-emerald-600 hover:bg-emerald-700 text-white'
                                ]">
                                    <UserPlus v-if="!profile?.isFollowed" class="w-5 h-5" />
                                    <UserMinus v-else class="w-5 h-5" />
                                    <span>{{ profile?.isFollowed ? 'Unfollow' : 'Follow' }}</span>
                                </button>
                            </div>
                        </div>

                        <!-- Stats Grid -->
                        <div class="grid grid-cols-4 gap-4 mt-6">
                            <!-- Posts -->
                            <div class="p-4 rounded-lg bg-slate-50 border border-slate-200">
                                <div class="flex items-center space-x-3">
                                    <div class="w-10 h-10 rounded-full bg-emerald-100 flex items-center justify-center">
                                        <MessageSquare class="w-5 h-5 text-emerald-600" />
                                    </div>
                                    <div>
                                        <div class="text-2xl font-bold text-slate-900">
                                            {{ formatNumber(profile?.numberOfPosts ?? 0) }}
                                        </div>
                                        <div class="text-sm text-slate-600">Posts</div>
                                    </div>
                                </div>
                            </div>

                            <!-- Comments -->
                            <div class="p-4 rounded-lg bg-slate-50 border border-slate-200">
                                <div class="flex items-center space-x-3">
                                    <div class="w-10 h-10 rounded-full bg-indigo-100 flex items-center justify-center">
                                        <MessagesSquare class="w-5 h-5 text-indigo-600" />
                                    </div>
                                    <div>
                                        <div class="text-2xl font-bold text-slate-900">
                                            {{ formatNumber(profile?.numberOfComments ?? 0) }}
                                        </div>
                                        <div class="text-sm text-slate-600">Comments</div>
                                    </div>
                                </div>
                            </div>

                            <!-- Followers -->
                            <div class="p-4 rounded-lg bg-slate-50 border border-slate-200">
                                <div class="flex items-center space-x-3">
                                    <div class="w-10 h-10 rounded-full bg-violet-100 flex items-center justify-center">
                                        <Users class="w-5 h-5 text-violet-600" />
                                    </div>
                                    <div>
                                        <div class="text-2xl font-bold text-slate-900">
                                            {{ formatNumber(profile?.numberOfFollowers ?? 0) }}
                                        </div>
                                        <div class="text-sm text-slate-600">Followers</div>
                                    </div>
                                </div>
                            </div>

                            <!-- Following -->
                            <div class="p-4 rounded-lg bg-slate-50 border border-slate-200">
                                <div class="flex items-center space-x-3">
                                    <div class="w-10 h-10 rounded-full bg-teal-100 flex items-center justify-center">
                                        <UserCheck class="w-5 h-5 text-teal-600" />
                                    </div>
                                    <div>
                                        <div class="text-2xl font-bold text-slate-900">
                                            {{ formatNumber(profile?.numberOfFollowing ?? 0) }}
                                        </div>
                                        <div class="text-sm text-slate-600">Following</div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Posts Feed -->
        <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
            <div class="space-y-6">
                <div v-for="post in posts" :key="post.id"
                    class="bg-white rounded-xl shadow-sm hover:shadow-md transition-all border border-slate-200 group overflow-hidden">
                    <!-- Post Header -->
                    <div class="border-b border-slate-100 p-4">
                        <div class="flex items-center justify-between">
                            <!-- Left side with forum info -->
                            <div class="flex items-center space-x-3">
                                <div class="flex items-center space-x-3">
                                    <span class="text-sm text-slate-500">Posted in</span>
                                    <a :href="`/forums/${post.forum.id}`"
                                        class="font-medium text-emerald-600 hover:text-emerald-700">
                                        {{ post.forum.name }}
                                    </a>
                                    <span class="text-sm text-slate-500">{{ formatDate(post.createdAt) }}</span>
                                </div>
                            </div>

                            <!-- Right side actions -->
                            <div class="flex items-center space-x-3">
                                <span v-if="post.isLocked"
                                    class="px-2 py-0.5 rounded-full bg-amber-100 text-amber-600 text-xs font-medium flex items-center">
                                    <LockIcon class="w-3 h-3 mr-1" />
                                    Locked
                                </span>
                                <PostDropdownMenu :postId="post.id" :onDeletePost="handleDeletePost"
                                    v-if="isOwnProfile" />
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
                                'text-emerald-500': post.upvotes - post.downvotes > 0,
                                'text-red-500': post.upvotes - post.downvotes < 0,
                                'text-slate-600': post.upvotes - post.downvotes === 0
                            }">
                                {{ post.upvotes - post.downvotes }}
                            </span>

                            <button :class="[
                                'p-1.5 rounded-lg transition-all transform hover:scale-110',
                                post.isDownvoted
                                    ? 'text-red-500 bg-red-50'
                                    : 'text-slate-400 hover:text-red-500 hover:bg-red-50'
                            ]" @click="!post.isDownvoted ? downvotePost(post.id) : deleteDownvotePost(post.id)">
                                <ArrowBigDown class="w-6 h-6" />
                            </button>
                        </div>

                        <!-- Main Content -->
                        <div class="p-4 flex-grow">
                            <h2 @click="navigateToPost(post.id)"
                                class="text-xl font-semibold text-slate-900 group-hover:text-emerald-600 transition-colors cursor-pointer">
                                {{ post.title }}
                            </h2>

                            <div class="mt-3 prose-sm text-slate-600 leading-relaxed line-clamp-3">
                                {{ post.content }}
                            </div>

                            <!-- Post Footer -->
                            <div class="flex items-center space-x-6 mt-4">
                                <button @click="navigateToPost(post.id)"
                                    class="flex items-center space-x-2 text-slate-500 hover:text-emerald-600 hover:bg-emerald-50 rounded-full px-4 py-1.5 transition-colors text-sm">
                                    <MessageSquare class="w-4 h-4" />
                                    <span>{{ post.comments ?? 0 }} Comments</span>
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
                <div v-if="!hasMore && !loading" class="text-center py-8">
                    <div class="inline-flex items-center justify-center w-16 h-16 rounded-full bg-slate-100 mb-4">
                        <CheckCircle class="w-8 h-8 text-slate-400" />
                    </div>
                    <h3 class="text-lg font-medium text-slate-900">No More Posts</h3>
                    <p class="text-slate-600 mt-1">You've seen all posts from this user.</p>
                </div>
            </div>
        </div>
    </div>

    <div>
        <ToastNotification ref="toastRef" />
    </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, onUnmounted } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import {
    MessageSquare,
    MessagesSquare,
    Share2,
    ArrowBigUp,
    ArrowBigDown,
    LockIcon,
    UserPlus,
    UserMinus,
    Users,
    UserCheck,
    BanIcon,
    MoreVertical,
    Bookmark,
    CheckCircle
} from 'lucide-vue-next';
import { VotingService } from '@/app/services/forum_backend/voting_service';
import { ProfileService } from '@/app/services/forum_backend/profile_service';
import { getAccessToken } from '@/app/services/backend/auth_service';
import type { Profile } from '@/app/models/forum_backend/Profile';
import type { Post } from '@/app/models/forum_backend/Post';
import ToastNotification from '@/components/forums/ToastNotification.vue';
import UniversalNavbar from '@/components/forums/UniversalNavbar.vue';
import PostDropdownMenu from '@/components/forums/PostDropdownMenu.vue';

const route = useRoute();
const router = useRouter();
const userId = computed(() => route.params.userId as string);
const profile = ref<Profile | null>(null);
const posts = ref<Post[]>([]);
const page = ref(1);
const pageSize = ref(10);
const loading = ref(false);
const hasMore = ref(true);
const isOwnProfile = ref(false);

const toastRef = ref();

onMounted(async () => {
    const jwt = await getAccessToken();
    if (!jwt) {
        router.push('/login');
        return;
    }

    await Promise.all([
        fetchProfile(jwt),
        fetchInitialPosts(jwt)
    ]);

    window.addEventListener('scroll', handleScroll);
});

onUnmounted(() => {
    window.removeEventListener('scroll', handleScroll);
});

async function fetchProfile(jwt: string) {
    try {
        profile.value = await ProfileService.getProfile(jwt, userId.value);
        const currentUser = await ProfileService.getCurrentProfile(jwt);
        isOwnProfile.value = currentUser.id === profile.value?.id;
    } catch (error) {
        console.error('Error fetching profile:', error);
    }
}

async function fetchInitialPosts(jwt: string) {
    try {
        const initialPosts = await ProfileService.getProfilePostsPaged(jwt, userId.value, page.value, pageSize.value);
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

        const morePosts = await ProfileService.getProfilePostsPaged(jwt, userId.value, page.value, pageSize.value);

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

async function toggleFollow() {
    const jwt = await getAccessToken();
    if (!jwt) {
        router.push('/login');
        return;
    }

    try {
        if (profile.value?.isFollowed) {
            await ProfileService.unfollowProfile(jwt, userId.value);
            profile.value.isFollowed = false;
            profile.value.numberOfFollowers--;
            toastRef.value.showToast('Unfollowed successfully!');
        } else {
            await ProfileService.followProfile(jwt, userId.value);
            if (profile.value) {
                profile.value.isFollowed = true;
                profile.value.numberOfFollowers++;
            }
            toastRef.value.showToast('Followed successfully!');
        }
    } catch (error) {
        console.error('Error toggling follow:', error);
        toastRef.value.showToast('Failed to update follow status!');
    }
}

async function upvotePost(postId: string) {
    const jwt = await getAccessToken();
    if (!jwt) {
        router.push('/login');
        return;
    }

    await VotingService.upvotePost(jwt, postId);

    const post = posts.value.find(post => post.id === postId);
    if (!post) return;

    post.upvotes++;
    post.isUpvoted = true;

    if (post.isDownvoted) {
        post.isDownvoted = false;
        post.downvotes--;
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

    const post = posts.value.find(post => post.id === postId);
    if (!post) return;

    post.downvotes++;
    post.isDownvoted = true;

    if (post.isUpvoted) {
        post.isUpvoted = false;
        post.upvotes--;
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

    const post = posts.value.find(post => post.id === postId);
    if (!post) return;

    post.upvotes--;
    post.isUpvoted = false;

    toastRef.value.showToast('Upvote removed!');
}

async function deleteDownvotePost(postId: string) {
    const jwt = await getAccessToken();
    if (!jwt) {
        router.push('/login');
        return;
    }

    await VotingService.deleteDownvotePost(jwt, postId);

    const post = posts.value.find(post => post.id === postId);
    if (!post) return;

    post.downvotes--;
    post.isDownvoted = false;

    toastRef.value.showToast('Downvote removed!');
}

function navigateToPost(postId: string) {
    router.push(`/posts/${postId}`);
}

function formatNumber(num: number): string {
    return new Intl.NumberFormat('en-US', { notation: 'compact' }).format(num);
}

function formatDate(dateString: string): string {
    return new Date(dateString).toLocaleDateString('en-US', {
        month: 'short',
        day: 'numeric',
        year: 'numeric'
    });
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

function handleSearch(query: string) {
    posts.value = posts.value.filter(post =>
        post.title.toLowerCase().includes(query.toLowerCase()) ||
        post.content.toLowerCase().includes(query.toLowerCase())
    );
}

async function handleDeletePost(postId: string) {
    const jwt = await getAccessToken();
    if (!jwt) {
        router.push('/login');
        return;
    }

    try {
        await ProfileService.deletePost(jwt, postId);
        posts.value = posts.value.filter(post => post.id !== postId);
        toastRef.value.showToast('Post deleted successfully!');
    } catch (error) {
        console.error('Error deleting post:', error);
        toastRef.value.showToast('Failed to delete post!');
    }
}
</script>