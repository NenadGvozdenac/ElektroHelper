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
          <span class="ml-3 transition-all opacity-0 duration-500 ease-in-out" :class="{ 'opacity-100': isExpanded }">
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
              class="w-full p-3 pl-10 pr-16 rounded-lg bg-white border border-emerald-200 focus:ring-2 focus:ring-emerald-400 transition-all" />
            <SearchIcon class="absolute left-3 top-1/2 transform -translate-y-1/2 text-emerald-500" />
            <button @click="resetContext"
              class="absolute right-2 top-1/2 transform -translate-y-1/2 text-emerald-500 hover:text-emerald-700">
              <XIcon class="w-5 h-5" />
            </button>
          </div>
        </div>

        <!-- Existing Grid View -->
        <!-- Location Cards -->
        <div v-if="currentView === 'locations'" class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          <DashboardCard v-for="location in filteredLocations" :key="location.ID" type="location"
            :title="`${location.street} ${location.number}`" :subtitle="`${location.city}, ${location.country}`"
            :metrics="[
              { label: 'Postal Code', value: location.postal_code },
              { label: 'Active Meters', value: getMetersForLocation(location.ID).length }
            ]" :status-text="`${getMetersForLocation(location.ID).length} Meters`" :footer-text="location.postal_code"
            action-label="View Meters" @action="selectLocation(location)" />
        </div>

        <!-- Meter Cards -->
        <div v-if="currentView === 'meters'" class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          <DashboardCard v-for="meter in filteredMeters" :key="meter.ID" type="meter" :title="`Meter #${meter.ID}`"
            :subtitle="getMeterLocation(meter)" :metrics="[
              { label: 'Total Readings', value: getReadingsForMeter(meter.ID).length },
              { label: 'Registration Date', value: formatDate(meter.date_of_registration) }
            ]" :status-text="`${getReadingsForMeter(meter.ID).length} Readings`"
            :footer-text="`Registered: ${formatDate(meter.date_of_registration)}`" action-label="View Readings"
            @action="selectMeter(meter)" />
        </div>

        <!-- Reading Cards -->
        <div v-if="currentView === 'readings'" class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          <DashboardCard v-for="reading in filteredReadings" :key="reading.id" type="reading"
            :title="`Reading #${reading.id}`" :subtitle="`Meter #${reading.electricity_meter_id}`" :metrics="[
              { label: 'Lower Reading', value: reading.lower_reading },
              { label: 'Upper Reading', value: reading.upper_reading }
            ]" :status-text="formatDate(reading.reading_date)" :footer-text="`Meter #${reading.electricity_meter_id}`"
            disabled />
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
            <div v-else class="text-emerald-500 text-center">No active search</div>
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
                <p class="font-semibold text-blue-700">Meter #{{ activeMeter.ID }}</p>
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
                <XIcon class="w-4 h-4 text-emerald-500 hover:text-emerald-700" @click.stop="removeSearch(index)" />
              </div>
            </div>
            <p v-else class="text-emerald-500 text-center">No recent searches</p>
          </div>
        </div>

        <!-- Button on the bottom -->
        <button v-if="searchQuery || activeLocation || activeMeter" @click="resetContext"
          class="bg-emerald-600 hover:bg-emerald-700 text-white p-3 rounded-lg flex items-center justify-center transition-colors">
          <XIcon class="w-5 h-5 mr-2" />
          Reset Search & Context
        </button>
      </aside>
    </main>
  </div>

  <AddLocation v-model="isAddLocationDialogOpen" @submit="handleAddLocation" />
  <AddMeter v-model="isAddMeterDialogOpen" :locations="locations" :meters="meters" @submit="handleAddMeter" />
  <AddReading v-model="isAddReadingDialogOpen" :meters="meters" :locations="locations" @submit="handleAddReading" />
  <SuccessModal v-model="isSuccessModalOpen" :title="successModalTitle" :message="successModalMessage" />
</template>

<script setup lang="ts">
import { ref, computed, watch, onMounted } from "vue";
import {
  HomeIcon,
  BoltIcon,
  ClipboardListIcon,
  PlusIcon,
  XIcon,
  SearchIcon,
} from "lucide-vue-next";

import { goToHome } from "@/app/routes";

import { DashboardService } from "@/app/services/backend/dashboard_service";
import { getAccessToken } from "@/app/services/backend/auth_service";

import type { CreateLocation, Location } from "@/app/models/location";
import type { CreateElectricityMeter, CreateLocationWithMeter, ElectricityMeter } from "@/app/models/electricity_meter";
import type { CreateElectricityReadingWithDate, ElectricityReading } from "@/app/models/electricity_reading";

import AddLocation from "@/components/dashboard/AddLocation.vue";
import AddReading from "@/components/dashboard/AddReading.vue";
import AddMeter from "@/components/dashboard/AddMeter.vue";
import SuccessModal from "@/components/dashboard/SuccessModal.vue";

import DashboardCard from "@/components/dashboard/Card.vue";

const isAddLocationDialogOpen = ref(false);
const isAddReadingDialogOpen = ref(false);
const isAddMeterDialogOpen = ref(false);

const isSuccessModalOpen = ref(false);
const successModalMessage = ref('');
const successModalTitle = ref('Success!');

onMounted(async () => {
  const jwt: string | null = await getAccessToken();

  if (!jwt) {
    console.error("No JWT found in local storage");
    goToHome();
    return;
  }

  locations.value = await DashboardService.getLocationsForUser(jwt);
  meters.value = await DashboardService.getMetersForUser(jwt);
  readings.value = await DashboardService.getReadingsForUser(jwt);
});

const locations = ref<Location[]>([]);
const meters = ref<ElectricityMeter[]>([]);
const readings = ref<ElectricityReading[]>([]);

const isExpanded = ref(false);
const isNavbarFullyExpanded = ref(false);
const currentView = ref<"locations" | "meters" | "readings">("locations");
const activeLocation = ref<Location | null>(null);
const activeMeter = ref<ElectricityMeter | null>(null);

const searchQuery = ref("");
const recentSearches = ref<string[]>([]);

const navItems = [
  { type: "locations", label: "Locations", icon: HomeIcon },
  { type: "meters", label: "Meters", icon: BoltIcon },
  { type: "readings", label: "Readings", icon: ClipboardListIcon },
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

function selectNavItem(type: "locations" | "meters" | "readings") {
  currentView.value = type;
}

function selectLocation(location: Location) {
  activeLocation.value = location;
  currentView.value = "meters";
}

function selectMeter(meter: ElectricityMeter) {
  activeMeter.value = meter;
  currentView.value = "readings";
}

function formatDate(dateString: string): string {
  return new Date(dateString).toLocaleDateString("en-US", {
    year: "numeric",
    month: "short",
    day: "numeric",
  });
}

function addNew(type: "location" | "meter" | "reading") {
  if (type === "location") {
    isAddLocationDialogOpen.value = true;
  }

  if (type === "reading") {
    isAddReadingDialogOpen.value = true;
  }

  if (type === "meter") {
    isAddMeterDialogOpen.value = true;
  }
}

function getReadingsForMeter(meterId: number): ElectricityReading[] {
  return readings.value.filter((r) => r.electricity_meter_id === meterId);
}

function getMetersForLocation(locationId: number): ElectricityMeter[] {
  return meters.value.filter((m) => m.location_id === locationId);
}

const filteredLocations = computed(() =>
  locations.value.filter(
    (location) =>
      searchQuery.value === "" ||
      Object.values(location).some((val) =>
        val.toString().toLowerCase().includes(searchQuery.value.toLowerCase())
      )
  )
);

const filteredMeters = computed(() =>
  (activeLocation.value
    ? meters.value.filter((m) => m.location_id === activeLocation.value?.ID)
    : meters.value
  ).filter(
    (meter) =>
      searchQuery.value === "" ||
      Object.values(meter).some((val) =>
        val.toString().toLowerCase().includes(searchQuery.value.toLowerCase())
      )
  )
);

const filteredReadings = computed(() =>
  (activeMeter.value
    ? readings.value.filter((r) => r.electricity_meter_id === activeMeter.value?.ID)
    : readings.value
  ).filter(
    (reading) =>
      searchQuery.value === "" ||
      Object.values(reading).some((val) =>
        val.toString().toLowerCase().includes(searchQuery.value.toLowerCase())
      )
  )
);

// Additional computed properties for filtering
const filteredItemsCount = computed(() => {
  switch (currentView.value) {
    case "locations":
      return locations.value.filter((location) =>
        Object.values(location).some((val) =>
          val.toString().toLowerCase().includes(searchQuery.value.toLowerCase())
        )
      ).length;
    case "meters":
      return filteredMeters.value.filter((meter) =>
        Object.values(meter).some((val) =>
          val.toString().toLowerCase().includes(searchQuery.value.toLowerCase())
        )
      ).length;
    case "readings":
      return filteredReadings.value.filter((reading) =>
        Object.values(reading).some((val) =>
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

function getPageTitle(view: "locations" | "meters" | "readings") {
  const titles = {
    locations: "My Electricity Locations",
    meters: "Electricity Meters",
    readings: "Meter Readings",
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
  searchQuery.value = "";
  activeLocation.value = null;
  activeMeter.value = null;
  currentView.value = "locations";
}

function getMeterLocation(meter: ElectricityMeter): string {
  const location = locations.value.find((l) => l.ID === meter.location_id);
  return location ? `${location.street} ${location.number}` : "Unknown Location";
}

const handleAddLocation = async (locationData: CreateLocationWithMeter) => {
  // Handle the new location data here
  try {
    const jwt = await getAccessToken();
    if (!jwt) {
      console.error("No JWT found");
      return;
    }

    const locationToCreate: CreateLocation = {
      street: locationData.street,
      number: locationData.number,
      city: locationData.city,
      country: locationData.country,
      postal_code: locationData.postal_code,
    };

    let createdLocation = await DashboardService.addLocationForUser(jwt, locationToCreate);

    if (locationData.hasElectricityMeter && locationData.meter_code) {
      // Create a new meter for the location

      const createElectricityMeter: CreateElectricityMeter = {
        location_id: createdLocation.ID,
        meter_code: locationData.meter_code,
      }

      await DashboardService.addMeterForUser(jwt, createElectricityMeter);

      // Refresh meters list
      meters.value = await DashboardService.getMetersForUser(jwt);
    }

    // Show success modal
    successModalTitle.value = 'Location Added!';
    successModalMessage.value = `Successfully added location at ${locationData.street} ${locationData.number}`;
    isSuccessModalOpen.value = true;

    // Refresh locations list
    locations.value = await DashboardService.getLocationsForUser(jwt);
  } catch (error) {
    console.error("Error creating location:", error);
  }
};

const handleAddReading = async (readingData: CreateElectricityReadingWithDate) => {
  try {
    const jwt = await getAccessToken();
    if (!jwt) {
      console.error('No JWT found');
      return;
    }

    await DashboardService.addReadingForUser(jwt, readingData);

    // Show success modal
    successModalTitle.value = 'Reading Added!';
    successModalMessage.value = `Successfully added new reading for meter #${readingData.electricity_meter_id}`;
    isSuccessModalOpen.value = true;

    // Refresh readings list
    readings.value = await DashboardService.getReadingsForUser(jwt);
  } catch (error) {
    console.error('Error creating reading:', error);
  }
};

const handleAddMeter = async (meterData: CreateElectricityMeter) => {
  try {
    const jwt = await getAccessToken();
    if (!jwt) {
      console.error('No JWT found');
      return;
    }

    await DashboardService.addMeterForUser(jwt, meterData);

    // Show success modal
    successModalTitle.value = 'Meter Added!';
    successModalMessage.value = `Successfully added new meter with code ${meterData.meter_code}`;
    isSuccessModalOpen.value = true;

    // Refresh meters list
    meters.value = await DashboardService.getMetersForUser(jwt);
  } catch (error) {
    console.error('Error creating meter:', error);
  }
};

</script>
