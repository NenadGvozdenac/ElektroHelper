<template>
    <div class="flex h-screen">
        <!-- Expandable Sidebar -->
        <nav @mouseenter="isExpanded = true" @mouseleave="isExpanded = false"
            class="bg-emerald-800 text-white transition-all ease-in-out duration-500 flex flex-col"
            :class="isExpanded ? 'w-64' : 'w-20'">
            <div class="p-4 flex items-center justify-center cursor-pointer flex flex-col" @click="goToHome()">
                <BoltIcon class="w-10 h-10" />

                <span class="ml-3 text-xl font-bold text-white transition-all opacity-0 duration-500 ease-in-out"
                    :class="{ 'opacity-100': isExpanded }">
                    ElektroHelper
                </span>
            </div>

            <div class="mt-10 flex-grow">
                <div v-for="(item, index) in navItems" :key="index"
                    @click="selectNavItem(item.type as 'locations' | 'meters' | 'readings')"
                    class="flex items-center p-3 cursor-pointer hover:bg-emerald-700 transition-colors">
                    <component :is="item.icon" class="w-6 h-6" />
                    <span class="ml-3 transition-all opacity-0 duration-500 ease-in-out"
                        :class="{ 'opacity-100': isExpanded }">
                        {{ item.label }}
                    </span>
                </div>
            </div>

            <div class="p-4 space-y-2">
                <button @click="addNew('location')"
                    class="w-full bg-emerald-600 hover:bg-emerald-700 text-white p-2 rounded flex items-center transition-all opacity-0 duration-500 ease-in-out"
                    :class="{ 'opacity-100': isNavbarFullyExpanded && isExpanded }">
                    <PlusIcon class="w-6 h-6 mr-2" />
                    <span v-if="isNavbarFullyExpanded">Add Location</span>
                </button>
                <button @click="addNew('meter')"
                    class="w-full bg-emerald-600 hover:bg-emerald-700 text-white p-2 rounded flex items-center transition-all opacity-0 duration-500 ease-in-out"
                    :class="{ 'opacity-100': isNavbarFullyExpanded && isExpanded }">
                    <PlusIcon class="w-6 h-6 mr-2" />
                    <span v-if="isNavbarFullyExpanded">Add Meter</span>
                </button>
                <button @click="addNew('reading')"
                    class="w-full bg-emerald-600 hover:bg-emerald-700 text-white p-2 rounded flex items-center transition-all opacity-0 duration-500 ease-in-out"
                    :class="{ 'opacity-100': isNavbarFullyExpanded && isExpanded }">
                    <PlusIcon class="w-6 h-6 mr-2" />
                    <span v-if="isNavbarFullyExpanded">Add Reading</span>
                </button>
            </div>
        </nav>

        <!-- Main Content Area with Search Panel -->
        <main class="flex flex-1">
            <div class="flex-grow bg-emerald-50 p-6 overflow-auto">
                <!-- Search Header with Reset Button -->
                <div class="mb-6 flex justify-between items-center">
                    <h2 class="text-emerald-800 font-bold text-2xl">
                        {{ getPageTitle(currentView) }}
                    </h2>
                    <div class="relative w-full max-w-md flex items-center">
                        <input type="text" v-model="searchQuery" :placeholder="`Search ${currentView}...`"
                            class="w-full p-3 pl-10 pr-16 rounded-lg bg-white border border-emerald-200 focus:ring-2 focus:ring-emerald-400 transition-all">
                        <SearchIcon class="absolute left-3 top-1/2 transform -translate-y-1/2 text-emerald-500" />
                        <button @click="resetContext"
                            class="absolute right-2 top-1/2 transform -translate-y-1/2 text-emerald-500 hover:text-emerald-700">
                            <XIcon class="w-5 h-5" />
                        </button>
                    </div>
                </div>

                <!-- Existing Grid View -->
                <div v-if="currentView === 'locations'" class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                    <div v-for="location in filteredLocations" :key="location.id"
                        class="bg-gradient-to-br from-emerald-100 to-emerald-200 rounded-lg shadow-md p-4 transform transition-all hover:scale-105">
                        <div class="flex justify-between items-center mb-2">
                            <h3 class="text-emerald-800 font-bold text-lg">
                                {{ location.street }} {{ location.number }}
                            </h3>
                            <span class="bg-emerald-600 text-white px-2 py-1 rounded text-xs">
                                {{ getMetersForLocation(location.id).length || 0 }} Meters
                            </span>
                        </div>
                        <p class="text-emerald-700 text-sm">
                            {{ location.city }}, {{ location.country }}
                        </p>
                        <div class="mt-4 flex justify-between items-center">
                            <span class="text-emerald-600 text-xs">
                                {{ location.postalCode }}
                            </span>
                            <button @click="selectLocation(location)"
                                class="bg-emerald-600 text-white px-3 py-1 rounded text-xs hover:bg-emerald-700">
                                View Meters
                            </button>
                        </div>
                    </div>
                </div>

                <div v-else-if="currentView === 'meters'" class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                    <div v-for="meter in filteredMeters" :key="meter.id"
                        class="bg-gradient-to-br from-blue-100 to-blue-200 rounded-lg shadow-md p-4 transform transition-all hover:scale-105">
                        <div class="flex justify-between items-center mb-2">
                            <h3 class="text-blue-800 font-bold text-lg">
                                Meter #{{ meter.id }}
                            </h3>
                            <span class="bg-blue-600 text-white px-2 py-1 rounded text-xs">
                                {{ getReadingsForMeter(meter.id).length || 0 }} Readings
                            </span>
                        </div>
                        <p v-if="activeLocation?.street" class="text-blue-700 text-sm">
                            Location: {{ activeLocation?.street }} {{ activeLocation?.number }}
                        </p>
                        <p v-else class="text-blue-700 text-sm">
                            Location: {{ getMeterLocation(meter) }}
                        </p>
                        <div class="mt-4 flex justify-between items-center">
                            <span class="text-blue-600 text-xs">
                                Registered: {{ formatDate(meter.date_of_registration) }}
                            </span>
                            <button @click="selectMeter(meter)"
                                class="bg-blue-600 text-white px-3 py-1 rounded text-xs hover:bg-blue-700">
                                View Readings
                            </button>
                        </div>
                    </div>
                </div>

                <div v-else-if="currentView === 'readings'"
                    class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                    <div v-for="reading in filteredReadings" :key="reading.id"
                        class="bg-gradient-to-br from-purple-100 to-purple-200 rounded-lg shadow-md p-4 transform transition-all hover:scale-105">
                        <div class="flex justify-between items-center mb-2">
                            <h3 class="text-purple-800 font-bold text-lg">
                                Reading #{{ reading.id }}
                            </h3>
                        </div>
                        <div class="text-purple-700 text-sm space-y-1">
                            <p>Date: {{ formatDate(reading.reading_date) }}</p>
                            <p>Lower: {{ reading.lower_reading }}</p>
                            <p>Upper: {{ reading.upper_reading }}</p>
                        </div>
                        <div class="mt-4 flex justify-between items-center">
                            <span class="text-purple-600 text-xs">
                                Meter #{{ reading.electricity_meter_id }}
                            </span>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Search Results and Current Context Panel -->
            <aside class="w-96 bg-white shadow-lg p-6 flex flex-col overflow-y-auto border-l border-emerald-100">
                <div class="flex-grow">
                    <div class="mb-6">
                        <h3 class="text-emerald-800 font-bold text-xl mb-4">Current Search</h3>
                        <div v-if="searchQuery" class="bg-emerald-50 p-4 rounded-lg">
                            <p class="text-emerald-700">
                                <span class="font-semibold">Query:</span> {{ searchQuery }}
                            </p>
                            <p class="text-emerald-600 text-sm mt-2">
                                <span class="font-semibold">Results:</span>
                                {{ filteredItemsCount }} items found
                            </p>
                        </div>
                        <div v-else class="text-emerald-500 text-center">
                            No active search
                        </div>
                    </div>

                    <div class="mb-6 h-45">
                        <h3 class="text-emerald-800 font-bold text-xl mb-4">Current Context</h3>
                        <div class="space-y-3">
                            <div v-if="activeLocation" class="bg-emerald-50 p-3 rounded-lg">
                                <p class="font-semibold text-emerald-700">
                                    Location: {{ activeLocation.street }} {{ activeLocation.number }}
                                </p>
                                <p class="text-emerald-600 text-sm">
                                    {{ activeLocation.city }}, {{ activeLocation.country }}
                                </p>
                            </div>
                            <div v-if="activeMeter" class="bg-blue-50 p-3 rounded-lg">
                                <p class="font-semibold text-blue-700">
                                    Meter #{{ activeMeter.id }}
                                </p>
                                <p class="text-blue-600 text-sm">
                                    Registered: {{ formatDate(activeMeter.date_of_registration) }}
                                </p>
                            </div>
                            <div v-if="!activeLocation && !activeMeter" class="text-emerald-500 text-center">
                                No active context
                            </div>
                        </div>
                    </div>

                    <div class="mb-6">
                        <h3 class="text-emerald-800 font-bold text-xl mb-4">Recent Searches</h3>
                        <div v-if="recentSearches.length" class="space-y-2">
                            <div v-for="(search, index) in recentSearches" :key="index"
                                class="bg-emerald-50 p-2 rounded-lg flex justify-between items-center hover:bg-emerald-100 transition-colors cursor-pointer"
                                @click="restoreSearch(search)">
                                <span class="text-emerald-700">{{ search }}</span>
                                <XIcon class="w-4 h-4 text-emerald-500 hover:text-emerald-700"
                                    @click.stop="removeSearch(index)" />
                            </div>
                        </div>
                        <p v-else class="text-emerald-500 text-center">
                            No recent searches
                        </p>
                    </div>
                </div>

                <!-- Button on the bottom -->
                <button v-if="searchQuery || activeLocation || activeMeter" @click="resetContext"
                    class="bg-emerald-600 hover:bg-emerald-700 text-white p-3 rounded-lg flex items-center justify-center    transition-colors">
                    <XIcon class="w-5 h-5 mr-2" />
                    Reset Search & Context
                </button>
            </aside>
        </main>
    </div>
</template>

<script setup lang="ts">
import { ref, computed, watch, onMounted } from 'vue';
import {
    HomeIcon,
    BoltIcon,
    ClipboardListIcon,
    PlusIcon,
    XIcon,
    SearchIcon
} from 'lucide-vue-next';

import { goToHome } from '@/app/routes';

import { DashboardService } from '@/app/services/backend/dashboard_service';
import { getAccessToken } from '@/app/services/backend/auth_service';

import type { Location } from '@/app/models/location';
import type { ElectricityMeter } from '@/app/models/electricity_meter';
import type { ElectricityReading } from '@/app/models/electricity_reading';

onMounted(async () => {
    const jwt: string | null = await getAccessToken();

    if (!jwt) {
        console.error('No JWT found in local storage');
        return;
    }

    locations.value = await DashboardService.getLocationsForUser(jwt);
    meters.value = await DashboardService.getMetersForUser(jwt);
    readings.value = await DashboardService.getReadingsForUser(jwt);

    console.log('Locations:', locations.value);
    console.log('Meters:', meters.value);
    console.log('Readings:', readings.value);
});

const locations = ref<Location[]>([]);
const meters = ref<ElectricityMeter[]>([]);
const readings = ref<ElectricityReading[]>([]);

const isExpanded = ref(false);
const isNavbarFullyExpanded = ref(false);
const currentView = ref<'locations' | 'meters' | 'readings'>('locations');
const activeLocation = ref<Location | null>(null);
const activeMeter = ref<ElectricityMeter | null>(null);

const searchQuery = ref('');
const recentSearches = ref<string[]>([]);

const navItems = [
    { type: 'locations', label: 'Locations', icon: HomeIcon },
    { type: 'meters', label: 'Meters', icon: BoltIcon },
    { type: 'readings', label: 'Readings', icon: ClipboardListIcon }
];

watch(isExpanded, (newVal) => {
    if (newVal) {
        setTimeout(() => {
            isNavbarFullyExpanded.value = true;
        }, 150);
    } else {
        setTimeout(() => {
            isNavbarFullyExpanded.value = false;
        }, 100);
    }
});

function selectNavItem(type: 'locations' | 'meters' | 'readings') {
    currentView.value = type;
}

function selectLocation(location: Location) {
    activeLocation.value = location;
    currentView.value = 'meters';
}

function selectMeter(meter: ElectricityMeter) {
    activeMeter.value = meter;
    currentView.value = 'readings';
}

function formatDate(dateString: string): string {
    return new Date(dateString).toLocaleDateString('en-US', {
        year: 'numeric',
        month: 'short',
        day: 'numeric'
    });
}

function addNew(type: 'location' | 'meter' | 'reading') {
    // Placeholder for future implementation
    console.log(`Adding new ${type}`);
    // You'll replace this with actual logic to add new items
}

function getReadingsForMeter(meterId: number): ElectricityReading[] {
    return readings.value.filter(r => r.electricity_meter_id === meterId);
}

function getMetersForLocation(locationId: number): ElectricityMeter[] {
    return meters.value.filter(m => m.location_id === locationId);
}

const filteredLocations = computed(() =>
    locations.value.filter(location =>
        searchQuery.value === '' ||
        Object.values(location).some(val =>
            val.toString().toLowerCase().includes(searchQuery.value.toLowerCase())
        )
    )
);

const filteredMeters = computed(() =>
    (activeLocation.value
        ? meters.value.filter(m => m.location_id === activeLocation.value?.id)
        : meters.value
    ).filter(meter =>
        searchQuery.value === '' ||
        Object.values(meter).some(val =>
            val.toString().toLowerCase().includes(searchQuery.value.toLowerCase())
        )
    )
);

const filteredReadings = computed(() =>
    (activeMeter.value
        ? readings.value.filter(r => r.electricity_meter_id === activeMeter.value?.id)
        : readings.value
    ).filter(reading =>
        searchQuery.value === '' ||
        Object.values(reading).some(val =>
            val.toString().toLowerCase().includes(searchQuery.value.toLowerCase())
        )
    )
);

// Additional computed properties for filtering
const filteredItemsCount = computed(() => {
    switch (currentView.value) {
        case 'locations':
            return locations.value.filter(location =>
                Object.values(location).some(val =>
                    val.toString().toLowerCase().includes(searchQuery.value.toLowerCase())
                )
            ).length;
        case 'meters':
            return filteredMeters.value.filter(meter =>
                Object.values(meter).some(val =>
                    val.toString().toLowerCase().includes(searchQuery.value.toLowerCase())
                )
            ).length;
        case 'readings':
            return filteredReadings.value.filter(reading =>
                Object.values(reading).some(val =>
                    val.toString().toLowerCase().includes(searchQuery.value.toLowerCase())
                )
            ).length;
        default:
            return 0;
    }
});

// Watch for search query changes and update recent searches
watch(searchQuery, (newQuery) => {
    if (newQuery && !recentSearches.value.includes(newQuery)) {
        if (recentSearches.value.length >= 5) {
            recentSearches.value.pop();
        }
        recentSearches.value.unshift(newQuery);
    }
});

function getPageTitle(view: 'locations' | 'meters' | 'readings') {
    const titles = {
        'locations': 'My Electricity Locations',
        'meters': 'Electricity Meters',
        'readings': 'Meter Readings'
    };
    return titles[view];
}

function restoreSearch(search: string) {
    searchQuery.value = search;
}

function removeSearch(index: number) {
    recentSearches.value.splice(index, 1);
}

// Reset context function
function resetContext() {
    searchQuery.value = '';
    activeLocation.value = null;
    activeMeter.value = null;
    currentView.value = 'locations';
}

function getMeterLocation(meter: ElectricityMeter): string {
    const location = locations.value.find(l => l.id === meter.location_id);
    return location ? `${location.street} ${location.number}` : 'Unknown Location';
}
</script>