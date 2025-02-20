<template>
    <div class="min-h-screen bg-slate-100">
        <!-- Reusing the same header from ForumView -->
        <UniversalNavbar />

        <main class="container mx-auto px-4 py-6">
            <div class="max-w-4xl mx-auto">
                <!-- Back to Forum Button -->
                <button @click="navigateToForum(post?.forum.id)"
                    class="mb-4 flex items-center text-slate-600 hover:text-emerald-600 transition-colors">
                    <ArrowLeft class="w-4 h-4 mr-2" />
                    Back to Forum
                </button>

                <!-- Main Post Card -->
                <div class="bg-white rounded-xl shadow-sm border border-slate-200 mb-6">
                    <!-- Post Header -->
                    <div class="border-b border-slate-100 p-4">
                        <div class="flex items-center justify-between">
                            <div class="flex items-center space-x-3">
                                <div @click="toProfile(post?.author.id!)"
                                    class="cursor-pointer w-10 h-10 rounded-full bg-gradient-to-br from-emerald-500 to-indigo-500 flex items-center justify-center text-white font-bold">
                                    {{ post?.author?.username?.[0]?.toUpperCase() ?? 'A' }}
                                </div>
                                <div>
                                    <div class="flex items-center space-x-3">
                                        <span class="font-medium text-slate-900 cursor-pointer"
                                            @click="toProfile(post?.author.id!)">{{ post?.author?.username }}</span>
                                        <span class="text-sm text-slate-500">{{ formatDate(post?.createdAt) }}</span>
                                        <span v-if="post?.isLocked"
                                            class="px-2 py-0.5 rounded-full bg-amber-100 text-amber-600 text-xs font-medium flex items-center">
                                            <LockIcon class="w-3 h-3 mr-1" />
                                            Locked
                                        </span>
                                    </div>
                                </div>
                            </div>

                            <!-- Author Actions Dropdown -->
                            <div v-if="isAuthor" class="relative dropdown">
                                <button @click="toggleDropdown()"
                                    class="p-2 text-slate-500 hover:text-slate-700 rounded-lg hover:bg-slate-100 transition-colors">
                                    <MoreVertical class="w-5 h-5" />
                                </button>

                                <div v-if="showDropdown"
                                    class="absolute right-0 mt-2 w-48 bg-white rounded-lg shadow-lg border border-slate-200 py-1 z-10">
                                    <button @click="deletePost"
                                        class="w-full px-4 py-2 text-left text-red-600 hover:bg-slate-50 flex items-center">
                                        <Trash2 class="w-4 h-4 mr-2" />
                                        Delete Post
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Post Content -->
                    <div class="flex">
                        <!-- Voting Section -->
                        <div class="py-4 px-2 flex flex-col items-center justify-start bg-slate-50 min-w-[60px]">
                            <button :class="[
                                'p-1.5 rounded-lg transition-all transform hover:scale-110',
                                post?.isUpvoted
                                    ? 'text-emerald-500 bg-emerald-50'
                                    : 'text-slate-400 hover:text-emerald-500 hover:bg-emerald-50'
                            ]" @click="!post?.isUpvoted ? upvotePost() : deleteUpvotePost()">
                                <ArrowBigUp class="w-6 h-6" />
                            </button>

                            <span class="text-sm font-medium my-1" :class="{
                                'text-emerald-500': (post?.upvotes ?? 0) - (post?.downvotes ?? 0) > 0,
                                'text-red-500': (post?.upvotes ?? 0) - (post?.downvotes ?? 0) < 0,
                                'text-slate-600': (post?.upvotes ?? 0) - (post?.downvotes ?? 0) === 0
                            }">
                                {{ (post?.upvotes ?? 0) - (post?.downvotes ?? 0) }}
                            </span>

                            <button :class="[
                                'p-1.5 rounded-lg transition-all transform hover:scale-110',
                                post?.isDownvoted
                                    ? 'text-red-500 bg-red-50'
                                    : 'text-slate-400 hover:text-red-500 hover:bg-red-50'
                            ]" @click="!post?.isDownvoted ? downvotePost() : deleteDownvotePost()">
                                <ArrowBigDown class="w-6 h-6" />
                            </button>
                        </div>

                        <!-- Main Content -->
                        <div class="p-6 flex-grow">
                            <h1 class="text-2xl font-bold text-slate-900 mb-4">{{ post?.title }}</h1>
                            <div class="prose max-w-none text-slate-700">
                                {{ post?.content }}
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Comments Section -->
                <div class="space-y-6">
                    <h2 class="text-xl font-semibold text-slate-900">Comments ({{ comments.length }})</h2>

                    <!-- New Comment Form -->
                    <div class="bg-white rounded-xl shadow-sm border border-slate-200 p-4">
                        <textarea v-model="newComment"
                            class="w-full p-3 border border-slate-200 rounded-lg focus:ring-2 focus:ring-emerald-500 focus:border-transparent resize-none"
                            rows="4" placeholder="Write a comment..."></textarea>
                        <div class="flex justify-end mt-3">
                            <button @click="submitComment"
                                class="px-4 py-2 bg-emerald-600 text-white rounded-lg hover:bg-emerald-700 transition-colors font-medium">
                                Post Comment
                            </button>
                        </div>
                    </div>

                    <!-- Comments List -->
                    <div class="space-y-4">
                        <div v-for="comment in comments" :key="comment.id"
                            class="bg-white rounded-xl shadow-sm border border-slate-200">
                            <!-- Comment Content -->
                            <div class="flex">
                                <!-- Voting Section -->
                                <div
                                    class="py-4 px-2 flex flex-col items-center justify-start bg-slate-50 min-w-[60px]">
                                    <button :class="[
                                        'p-1.5 rounded-lg transition-all transform hover:scale-110',
                                        comment.isUpvoted
                                            ? 'text-emerald-500 bg-emerald-50'
                                            : 'text-slate-400 hover:text-emerald-500 hover:bg-emerald-50'
                                    ]"
                                        @click="!comment.isUpvoted ? upvoteComment(comment.id) : deleteUpvoteComment(comment.id)">
                                        <ArrowBigUp class="w-5 h-5" />
                                    </button>

                                    <span class="text-sm font-medium my-1" :class="{
                                        'text-emerald-500': comment.upvotes - comment.downvotes > 0,
                                        'text-red-500': comment.upvotes - comment.downvotes < 0,
                                        'text-slate-600': comment.upvotes - comment.downvotes === 0
                                    }">
                                        {{ comment.upvotes - comment.downvotes }}
                                    </span>

                                    <button :class="[
                                        'p-1.5 rounded-lg transition-all transform hover:scale-110',
                                        comment.isDownvoted
                                            ? 'text-red-500 bg-red-50'
                                            : 'text-slate-400 hover:text-red-500 hover:bg-red-50'
                                    ]"
                                        @click="!comment.isDownvoted ? downvoteComment(comment.id) : deleteDownvoteComment(comment.id)">
                                        <ArrowBigDown class="w-5 h-5" />
                                    </button>
                                </div>

                                <!-- Comment Content -->
                                <div class="p-4 flex-grow">
                                    <div class="flex items-center space-x-3 mb-2 cursor-pointer">
                                        <div @click="toProfile(comment.author.id)"
                                            class="w-8 h-8 rounded-full bg-gradient-to-br from-emerald-500 to-indigo-500 flex items-center justify-center text-white font-bold">
                                            {{ comment.author.username[0].toUpperCase() }}
                                        </div>
                                        <div>
                                            <span class="font-medium text-slate-900 cursor-pointer" @click="toProfile(comment.author.id)">{{ comment.author.username }}</span>
                                            <span class="text-sm text-slate-500 ml-2">{{
                                                formatDate(comment.createdAt) }}</span>
                                        </div>
                                    </div>
                                    <div class="text-slate-700 pl-11">
                                        {{ comment.content }}
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Loading State -->
                    <div v-if="loading" class="flex justify-center py-8">
                        <div class="animate-spin rounded-full h-8 w-8 border-b-2 border-emerald-500"></div>
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
import { ref, onMounted, computed } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import {
    ArrowBigUp,
    ArrowBigDown,
    LockIcon,
    MoreVertical,
    ArrowLeft,
    Trash2
} from 'lucide-vue-next';
import { PostService } from '@/app/services/forum_backend/post_service';
import { CommentService } from '@/app/services/forum_backend/comment_service';
import { VotingService } from '@/app/services/forum_backend/voting_service';
import { getAccessToken, getUserData } from '@/app/services/backend/auth_service';
import type { Post } from '@/app/models/forum_backend/Post';
import type { Comment, CreateComment } from '@/app/models/forum_backend/Comment';
import { goToForum, goToForums, goToHome, goToProfile } from '@/app/routes';
import ToastNotification from '@/components/forums/ToastNotification.vue';
import UniversalNavbar from '@/components/forums/UniversalNavbar.vue';
import { ProfileService } from '@/app/services/forum_backend/profile_service';

const route = useRoute();
const router = useRouter();
const postId = computed(() => route.params.postId as string);
const post = ref<Post | null>(null);
const comments = ref<Comment[]>([]);
const newComment = ref('');
const loading = ref(false);
const toastRef = ref();

const showDropdown = ref(false);
const isAuthor = ref(false);

onMounted(async () => {
    const jwt = await getAccessToken();
    if (!jwt) {
        router.push('/login');
        return;
    }

    await Promise.all([
        fetchPost(jwt),
        fetchComments(jwt)
    ]);

    document.addEventListener('click', (event) => {
        const target = event.target as HTMLElement;
        if (!target.closest('.dropdown')) {
            showDropdown.value = false;
        }
    });

    const currentUser = await getUserData();
    isAuthor.value = currentUser?.userID == post.value?.author?.id;
});

async function fetchPost(jwt: string) {
    try {
        post.value = await PostService.getPost(jwt, postId.value);
    } catch (error) {
        console.error('Error fetching post:', error);
        router.push('/');
    }
}

async function fetchComments(jwt: string) {
    try {
        let postComments = await CommentService.getPostComments(jwt, postId.value);
        comments.value = postComments.sort((a, b) => b.createdAt.localeCompare(a.createdAt));
    } catch (error) {
        console.error('Error fetching comments:', error);
    }
}

async function submitComment() {
    if (!newComment.value.trim()) return;

    const jwt = await getAccessToken();
    if (!jwt) {
        router.push('/login');
        return;
    }

    try {
        var createComment: CreateComment = {
            postId: postId.value,
            content: newComment.value
        }

        await CommentService.createComment(jwt, createComment);
        await fetchComments(jwt);

        newComment.value = '';
    } catch (error) {
        console.error('Error creating comment:', error);
    }

    showNotification('Comment posted!');
}

function showNotification(message: string) {
    toastRef.value.showToast(message);
}

async function upvotePost() {
    const jwt = await getAccessToken();
    if (!jwt || !post.value) return;

    await VotingService.upvotePost(jwt, post.value.id);
    post.value.upvotes++;
    post.value.isUpvoted = true;

    if (post.value.isDownvoted) {
        post.value.isDownvoted = false;
        post.value.downvotes--;
    }

    showNotification('Upvoted post!');
}

async function downvotePost() {
    const jwt = await getAccessToken();
    if (!jwt || !post.value) return;

    await VotingService.downvotePost(jwt, post.value.id);
    post.value.downvotes++;
    post.value.isDownvoted = true;

    if (post.value.isUpvoted) {
        post.value.isUpvoted = false;
        post.value.upvotes--;
    }

    showNotification('Downvoted post!');
}

async function deleteUpvotePost() {
    const jwt = await getAccessToken();
    if (!jwt || !post.value) return;

    await VotingService.deleteUpvotePost(jwt, post.value.id);
    post.value.upvotes--;
    post.value.isUpvoted = false;

    showNotification('Removed upvote from post!');
}

async function deleteDownvotePost() {
    const jwt = await getAccessToken();
    if (!jwt || !post.value) return;

    await VotingService.deleteDownvotePost(jwt, post.value.id);
    post.value.downvotes--;
    post.value.isDownvoted = false;

    showNotification('Removed downvote from post!');
}

function formatDate(dateString: string | undefined): string {
    if (!dateString) return '';
    return new Date(dateString).toLocaleDateString('en-US', {
        month: 'short',
        day: 'numeric',
        year: 'numeric'
    });
}

function navigateToForum(forumId: string | undefined) {
    if (forumId) {
        goToForum(forumId);
    }
}

async function upvoteComment(commentId: string) {
    const jwt = await getAccessToken();
    if (!jwt) return;

    await VotingService.upvoteComment(jwt, commentId);

    const comment = comments.value.find(c => c.id === commentId);
    if (!comment) return;

    comment.upvotes++;
    comment.isUpvoted = true;

    if (comment.isDownvoted) {
        comment.isDownvoted = false;
        comment.downvotes--;
    }

    showNotification('Upvoted comment!');
}

async function downvoteComment(commentId: string) {
    const jwt = await getAccessToken();
    if (!jwt) return;

    await VotingService.downvoteComment(jwt, commentId);

    const comment = comments.value.find(c => c.id === commentId);
    if (!comment) return;

    comment.downvotes++;
    comment.isDownvoted = true;

    if (comment.isUpvoted) {
        comment.isUpvoted = false;
        comment.upvotes--;
    }

    showNotification('Downvoted comment!');
}

async function deleteUpvoteComment(commentId: string) {
    const jwt = await getAccessToken();
    if (!jwt) return;

    await VotingService.deleteUpvoteComment(jwt, commentId);

    const comment = comments.value.find(c => c.id === commentId);
    if (!comment) return;

    comment.upvotes--;
    comment.isUpvoted = false;

    showNotification('Removed upvote from comment!');
}

async function deleteDownvoteComment(commentId: string) {
    const jwt = await getAccessToken();
    if (!jwt) return;

    await VotingService.deleteDownvoteComment(jwt, commentId);

    const comment = comments.value.find(c => c.id === commentId);
    if (!comment) return;

    comment.downvotes--;
    comment.isDownvoted = false;

    showNotification('Removed downvote from comment!');
}

async function deletePost() {
    const jwt = await getAccessToken();
    if (!jwt || !post.value) return;

    try {
        await ProfileService.deletePost(jwt, post.value.id);
        showNotification('Post deleted successfully');
        goToForum(post.value.forum.id);
    } catch (error) {
        console.error('Error deleting post:', error);
        showNotification('Failed to delete post');
    }
    showDropdown.value = false;
}

function toggleDropdown() {
    showDropdown.value = !showDropdown.value;
}

function toProfile(userId: string) {
    goToProfile(userId);
}
</script>