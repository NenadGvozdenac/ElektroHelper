<template>
    <Transition name="modal">
        <div v-if="modelValue" class="fixed inset-0 z-50 flex items-center justify-center">
            <!-- Backdrop -->
            <div class="absolute inset-0 bg-black bg-opacity-50" @click="$emit('update:modelValue', false)"></div>

            <!-- Modal -->
            <div class="relative bg-white rounded-lg shadow-xl w-full max-w-md mx-4 transform transition-all">
                <!-- Header -->
                <div class="bg-blue-800 text-white px-6 py-4 rounded-t-lg flex justify-between items-center">
                    <h3 class="text-white text-xl font-bold mt-2">Add New Meter</h3>
                    <button @click="$emit('update:modelValue', false)" class="text-white hover:text-blue-200">
                        <XIcon class="w-6 h-6" />
                    </button>
                </div>

                <!-- Form -->
                <form @submit.prevent="handleSubmit" class="p-6 space-y-4">
                    <!-- Location Selection -->
                    <div class="space-y-2">
                        <label class="text-sm font-medium text-blue-800">Location</label>
                        <div>
                            <select v-if="filteredLocation.length > 0" v-model="formData.location_id" required
                                class="w-full p-3 rounded-lg border border-blue-200 focus:ring-2 focus:ring-blue-400 focus:border-blue-400 transition-all">
                                <option value="" disabled>Select a location</option>
                                <option v-for="location in filteredLocation" :key="location.ID" :value="location.ID">
                                    {{ location.street }} {{ location.number }}, {{ location.city }}
                                </option>
                            </select>
                            <div v-else class="p-4 bg-yellow-50 rounded-lg border border-yellow-200">
                                <span class="text-sm text-yellow-700">
                                    No available locations found. <br>All existing locations already have meters assigned.
                                </span>
                            </div>
                        </div>
                    </div>

                    <!-- Meter Code -->
                    <div class="space-y-2">
                        <label class="text-sm font-medium text-blue-800">Meter Code</label>
                        <input v-model="formData.meter_code" type="text" required
                            :disabled="filteredLocation.length === 0"
                            class="w-full p-3 rounded-lg border border-blue-200 focus:ring-2 focus:ring-blue-400 focus:border-blue-400 transition-all disabled:bg-gray-100 disabled:cursor-not-allowed"
                            placeholder="Enter meter code" />
                    </div>

                    <!-- Selected Location Details -->
                    <div v-if="selectedLocation" class="mt-4 p-4 bg-blue-50 rounded-lg">
                        <h4 class="text-sm font-medium text-blue-800 mb-2">Selected Location Details:</h4>
                        <p class="text-sm text-blue-600">
                            {{ selectedLocation.street }} {{ selectedLocation.number }}
                        </p>
                        <p class="text-sm text-blue-600">
                            {{ selectedLocation.city }}, {{ selectedLocation.country }}
                        </p>
                        <p class="text-sm text-blue-600">
                            Postal Code: {{ selectedLocation.postal_code }}
                        </p>
                    </div>

                    <!-- Buttons -->
                    <div class="flex space-x-3 pt-4">
                        <button type="button" @click="$emit('update:modelValue', false)"
                            class="flex-1 p-3 rounded-lg border border-blue-200 text-blue-800 hover:bg-blue-50 transition-colors">
                            Cancel
                        </button>
                        <button type="submit" :disabled="filteredLocation.length === 0"
                            class="flex-1 p-3 rounded-lg bg-blue-600 text-white hover:bg-blue-700 transition-colors disabled:bg-blue-300 disabled:cursor-not-allowed">
                            Add Meter
                        </button>
                    </div>
                </form>
            </div>
        </div>
    </Transition>
</template>

<script setup lang="ts">
import { ref, computed } from "vue";
import { XIcon } from "lucide-vue-next";
import type { CreateElectricityMeter, ElectricityMeter } from "@/app/models/backend/electricity_meter";
import type { Location } from "@/app/models/backend/location";

const filteredLocation = computed(() => {
    return props.locations.filter(location => !props.meters.some(meter => meter.location_id === location.ID));
});

const props = defineProps<{
    modelValue: boolean;
    locations: Location[];
    meters: ElectricityMeter[];
}>();

const emit = defineEmits<{
    (e: "update:modelValue", value: boolean): void;
    (e: "submit", value: CreateElectricityMeter): void;
}>();

const formData = ref<CreateElectricityMeter>({
    location_id: 0,
    meter_code: "",
});

const selectedLocation = computed(() => {
    return props.locations.find(location => location.ID === formData.value.location_id);
});

const handleSubmit = () => {
    emit("submit", formData.value);
    formData.value = {
        location_id: 0,
        meter_code: "",
    };
    emit("update:modelValue", false);
};
</script>

<style scoped>
.modal-enter-active,
.modal-leave-active {
    transition: opacity 0.3s ease;
}

.modal-enter-from,
.modal-leave-to {
    opacity: 0;
}
</style>