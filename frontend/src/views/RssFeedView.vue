<template>
    <div>
        <pre>{{ rssFeed }}</pre>
    </div>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { RssService } from '@/app/services/forum_backend/rss_service';

const rssFeed = ref<string>('');

onMounted(() => {
    getRssFeed();
});

async function getRssFeed() {
    try {
        rssFeed.value = await RssService.getRssFeed();
    } catch (error) {
        console.error('Failed to fetch RSS feed:', error);
        rssFeed.value = 'Error loading RSS feed';
    }
}
</script>

<style scoped>
pre {
    white-space: pre-wrap;
    word-wrap: break-word;
    background-color: black;
    color: white;
    padding: 10px;
    border-radius: 5px;
    overflow-x: auto;
}
</style>
