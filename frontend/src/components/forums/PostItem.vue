<template>
    <div
        class="bg-white rounded-xl shadow-sm hover:shadow-md transition-all border border-slate-200 group overflow-hidden">
        <!-- Post Header -->
        <div class="border-b border-slate-100 p-4">
            <div class="flex items-center justify-between">
                <!-- Left side with author info -->
                <div class="flex items-center space-x-3">
                    <div @click="goToProfile(post.author?.id)"
                        class="w-10 h-10 rounded-full bg-gradient-to-br from-emerald-500 to-indigo-500 flex items-center justify-center text-white font-bold cursor-pointer">
                        {{ post.author?.username?.[0]?.toUpperCase() ?? 'A' }}
                    </div>
                    <div class="flex-1">
                        <div class="flex items-center space-x-3">
                            <span class="font-medium text-slate-900 cursor-pointer" @click="goToProfile(post.author?.id)">{{ post.author?.username }}</span>
                            <span class="text-sm text-slate-500">{{ formatDate(post.createdAt) }}</span>
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
                    <span class="text-sm font-medium text-slate-500 cursor-pointer"
                        @click="$emit('navigateToForum', post.forum?.id)">
                        {{ post.forum?.name }}
                    </span>
                    <button class="p-1.5 hover:bg-slate-50 rounded-lg text-slate-400 hover:text-slate-600">
                        <MoreVertical class="w-5 h-5" />
                    </button>
                </div>
            </div>
        </div>

        <!-- Post Content -->
        <div class="flex">
            <!-- Voting Section -->
            <VotingSection :post="post" @upvote="$emit('upvote', post.id)" @downvote="$emit('downvote', post.id)"
                @deleteUpvote="$emit('deleteUpvote', post.id)" @deleteDownvote="$emit('deleteDownvote', post.id)" />

            <!-- Main Content -->
            <div class="p-4 flex-grow">
                <h2 @click="$emit('navigateToPost', post.id)"
                    class="text-xl font-semibold text-slate-900 group-hover:text-emerald-600 transition-colors cursor-pointer">
                    {{ post.title }}
                </h2>

                <!-- Post Preview -->
                <div class="mt-3 prose-sm text-slate-600 leading-relaxed line-clamp-3">
                    {{ post.content }}
                </div>

                <!-- Post Footer -->
                <PostFooter :post="post" @navigateToPost="$emit('navigateToPost', post.id)"
                    @copyToClipboard="$emit('copyToClipboard', post.id)" />
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { LockIcon, MoreVertical } from 'lucide-vue-next';
import type { Post } from '@/app/models/forum_backend/Post';
import VotingSection from '@/components/forums/VotingSection.vue';
import PostFooter from '@/components/forums/PostFooter.vue';
import { goToProfile } from '@/app/routes';

defineProps<{
    post: Post
}>();

defineEmits<{
    (e: 'navigateToForum', forumId: string): void
    (e: 'navigateToPost', postId: string): void
    (e: 'upvote', postId: string): void
    (e: 'downvote', postId: string): void
    (e: 'deleteUpvote', postId: string): void
    (e: 'deleteDownvote', postId: string): void
    (e: 'copyToClipboard', postId: string): void
}>();

function formatDate(dateString: string): string {
    return new Date(dateString).toLocaleDateString('en-US', {
        month: 'short',
        day: 'numeric',
        year: 'numeric'
    });
}
</script>