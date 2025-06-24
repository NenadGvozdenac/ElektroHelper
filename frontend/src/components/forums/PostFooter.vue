<template>
    <div class="flex items-center space-x-6 mt-4">
        <button @click="$emit('navigateToPost')"
            class="flex items-center space-x-2 text-slate-500 hover:text-emerald-600 hover:bg-emerald-50 rounded-full px-4 py-1.5 transition-colors text-sm">
            <MessageSquare class="w-4 h-4" />
            <span>{{ post.comments ?? 0 }} Comments</span>
        </button>

        <button @click="$emit('copyToClipboard')"
            class="flex items-center space-x-2 text-slate-500 hover:text-emerald-600 hover:bg-emerald-50 rounded-full px-4 py-1.5 transition-colors text-sm">
            <Share2 class="w-4 h-4" />
            <span>Share</span>
        </button>
    </div>
</template>

<script setup lang="ts">
import { MessageSquare, Share2, Bookmark } from 'lucide-vue-next';
import type { Post } from '@/app/models/forum_backend/Post';

defineProps<{
    post: Post
}>();

defineEmits<{
    (e: 'navigateToPost'): void
    (e: 'copyToClipboard'): void
}>();
</script>