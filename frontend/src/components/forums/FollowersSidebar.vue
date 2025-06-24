<template>
    <div class="flex-shrink-0 w-64" v-if="followers.length > 0">
        <div class="bg-white rounded-xl shadow-sm border border-slate-200">
            <div class="p-4 border-b border-slate-200 flex justify-between items-center">
                <span class="text-lg font-semibold text-slate-900">Followers</span>
                <span class="text-sm text-slate-500">{{ followers.length }}</span>
            </div>

            <div class="p-3">
                <div v-if="loading" class="flex justify-center py-4">
                    <div class="animate-spin rounded-full h-6 w-6 border-b-2 border-emerald-500"></div>
                </div>

                <div v-else-if="followers.length === 0" class="text-center py-4">
                    <span class="text-sm text-slate-500">No followers yet</span>
                </div>

                <div v-else class="space-y-2">
                    <div v-for="follower in followers" :key="follower.id" @click="navigateToProfile(follower.id)"
                        class="flex items-center p-2 rounded-lg cursor-pointer transition-all hover:bg-slate-50">
                        <div
                            class="w-8 h-8 rounded-full bg-gradient-to-br from-emerald-500 to-indigo-500 flex items-center justify-center">
                            <UserCircle class="w-4 h-4 text-white" />
                        </div>
                        <div class="ml-3 flex-1">
                            <div class="font-medium text-slate-900">{{ follower.username }}</div>
                            <div class="text-xs text-slate-500" v-if="follower.role != 'user'">{{ follower.role.toUpperCase() }}</div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { UserCircle } from 'lucide-vue-next';
import { getAccessToken } from '@/app/services/backend/auth_service';
import { ProfileService } from '@/app/services/forum_backend/profile_service';
import { goToProfile, goToLoginScreen } from '@/app/routes';
import type { SmallProfile } from '@/app/models/forum_backend/Profile';

const followers = ref < SmallProfile[] > ([]);
const loading = ref(true);

onMounted(async () => {
    const jwt = await getAccessToken();
    if (!jwt) {
        goToLoginScreen();
        return;
    }

    try {
        followers.value = await ProfileService.getMyProfileFollowers(jwt);
    } catch (error) {
        console.error('Error fetching followers:', error);
    } finally {
        loading.value = false;
    }
});

function navigateToProfile(userId: string) {
    goToProfile(userId);
}
</script>