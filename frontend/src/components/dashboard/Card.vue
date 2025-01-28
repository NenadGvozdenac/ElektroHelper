<!-- DashboardCard.vue -->
<template>
    <div class="group relative overflow-hidden rounded-xl bg-gradient-to-br shadow-lg transition-all duration-300 hover:shadow-xl"
        :class="[backgroundGradient, { 'transform hover:-translate-y-1': !disabled }]">
        <!-- Glass effect overlay -->
        <div class="absolute inset-0 bg-white/40 backdrop-blur-sm"></div>

        <!-- Card content -->
        <div class="relative p-6">
            <!-- Header -->
            <div class="mb-4 flex items-start justify-between">
                <div>
                    <h3 class="text-lg font-bold" :class="textColorClass">{{ title }}</h3>
                    <p class="mt-1 text-sm" :class="secondaryTextColorClass">{{ subtitle }}</p>
                </div>
                <div :class="[`bg-${colorScheme}-100`, `text-${colorScheme}-700`]"
                    class="rounded-full px-3 py-1 text-xs font-medium">
                    {{ statusText }}
                </div>
            </div>

            <!-- Metrics -->
            <div class="mb-4 grid grid-cols-2 gap-4">
                <div v-for="(metric, idx) in metrics" :key="idx" class="rounded-lg bg-white/50 p-3 backdrop-blur-sm">
                    <p class="text-xs font-medium" :class="secondaryTextColorClass">
                        {{ metric.label }}
                    </p>
                    <p class="mt-1 text-lg font-bold" :class="textColorClass">
                        {{ metric.value }}
                    </p>
                </div>
            </div>

            <!-- Footer -->
            <div class="mt-6 flex items-center justify-between">
                <div class="flex items-center space-x-2">
                    <component :is="icon" class="h-4 w-4" :class="secondaryTextColorClass" />
                    <span class="text-xs" :class="secondaryTextColorClass">
                        {{ footerText }}
                    </span>
                </div>
                <button v-if="!disabled" @click="$emit('action')"
                    class="group relative inline-flex items-center justify-center overflow-hidden rounded-lg px-4 py-2 font-medium transition-all duration-300">
                    <!-- Background layers -->
                    <span class="absolute inset-0 bg-gradient-to-br transition-all duration-300" :class="[
                        `from-${colorScheme}-500 to-${colorScheme}-500`,
                        'group-hover:from-${colorScheme}-500 group-hover:to-${colorScheme}-500'
                    ]"></span>

                    <!-- Shine effect -->
                    <span
                        class="absolute -right-8 -top-8 h-20 w-20 translate-x-full rotate-45 transform bg-white opacity-10 transition-all duration-700 ease-out group-hover:translate-x-0"></span>

                    <!-- Button text -->
                    <span class="relative text-sm text-dark">
                        {{ actionLabel }}
                    </span>
                </button>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import {
    HomeIcon,
    BoltIcon,
    ClipboardListIcon
} from 'lucide-vue-next';

interface Metric {
    label: string;
    value: string | number;
}

interface Props {
    type: 'location' | 'meter' | 'reading';
    title: string;
    subtitle: string;
    metrics: Metric[];
    statusText: string;
    footerText: string;
    actionLabel?: string;
    disabled?: boolean;
}

const props = withDefaults(defineProps<Props>(), {
    actionLabel: 'View Details',
    disabled: false
});

defineEmits<{
    (e: 'action'): void;
}>();

const colorScheme = computed(() => {
    switch (props.type) {
        case 'location':
            return 'emerald';
        case 'meter':
            return 'blue';
        case 'reading':
            return 'purple';
    }
});

const backgroundGradient = computed(() => ({
    'from-emerald-50 to-emerald-100/50': props.type === 'location',
    'from-blue-50 to-blue-100/50': props.type === 'meter',
    'from-purple-50 to-purple-100/50': props.type === 'reading',
}));

const textColorClass = computed(() => `text-${colorScheme.value}-900`);
const secondaryTextColorClass = computed(() => `text-${colorScheme.value}-700`);

const icon = computed(() => {
    switch (props.type) {
        case 'location':
            return HomeIcon;
        case 'meter':
            return BoltIcon;
        case 'reading':
            return ClipboardListIcon;
    }
});
</script>