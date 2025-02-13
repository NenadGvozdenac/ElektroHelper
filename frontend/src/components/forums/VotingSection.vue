<template>
    <div
        class="py-4 px-2 flex flex-col items-center justify-start bg-slate-50 group-hover:bg-slate-100 transition-colors min-w-[60px]">
        <button :class="[
            'p-1.5 rounded-lg transition-all transform hover:scale-110',
            post.isUpvoted
                ? 'text-emerald-500 bg-emerald-50'
                : 'text-slate-400 hover:text-emerald-500 hover:bg-emerald-50'
        ]" @click="!post.isUpvoted ? $emit('upvote') : $emit('deleteUpvote')">
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
        ]" @click="!post.isDownvoted ? $emit('downvote') : $emit('deleteDownvote')">
            <ArrowBigDown class="w-6 h-6" />
        </button>
    </div>
</template>

<script setup lang="ts">
import { ArrowBigUp, ArrowBigDown } from 'lucide-vue-next';
import type { Post } from '@/app/models/forum_backend/Post';

defineProps<{
    post: Post
}>();

defineEmits<{
    (e: 'upvote'): void
    (e: 'downvote'): void
    (e: 'deleteUpvote'): void
    (e: 'deleteDownvote'): void
}>();
</script>