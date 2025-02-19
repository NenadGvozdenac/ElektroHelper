<template>
    <div class="relative" ref="dropdownRef">
        <button @click="isOpen = !isOpen"
            class="p-1.5 hover:bg-slate-50 rounded-lg text-slate-400 hover:text-slate-600">
            <MoreVertical class="w-5 h-5" />
        </button>

        <Transition enter-active-class="transition duration-100 ease-out"
            enter-from-class="transform scale-95 opacity-0" enter-to-class="transform scale-100 opacity-100"
            leave-active-class="transition duration-75 ease-in" leave-from-class="transform scale-100 opacity-100"
            leave-to-class="transform scale-95 opacity-0">
            <div v-if="isOpen"
                class="absolute right-0 mt-1 w-48 bg-white rounded-lg shadow-lg border border-slate-200 py-1 z-50">
                <button @click="handleDelete"
                    class="w-full px-4 py-2 text-left text-red-600 hover:bg-slate-50 flex items-center space-x-2">
                    <Trash2 class="w-4 h-4" />
                    <span>Delete Post</span>
                </button>
            </div>
        </Transition>
    </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted } from 'vue';
import { MoreVertical, Trash2 } from 'lucide-vue-next';

interface Props {
    postId: string;
    onDeletePost: (postId: string) => void;
}

const props = defineProps<Props>();
const isOpen = ref(false);
const dropdownRef = ref<HTMLElement | null>(null);

const handleClickOutside = (event: MouseEvent) => {
    if (dropdownRef.value && !dropdownRef.value.contains(event.target as Node)) {
        isOpen.value = false;
    }
};

const handleDelete = () => {
    props.onDeletePost(props.postId);
    isOpen.value = false;
};

onMounted(() => {
    document.addEventListener('mousedown', handleClickOutside);
});

onUnmounted(() => {
    document.removeEventListener('mousedown', handleClickOutside);
});
</script>