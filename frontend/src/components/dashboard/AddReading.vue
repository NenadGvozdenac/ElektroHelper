<template>
    <Transition name="modal">
        <div v-if="modelValue" class="fixed inset-0 z-50 flex items-center justify-center">
            <!-- Backdrop -->
            <div class="absolute inset-0 bg-black bg-opacity-50" @click="$emit('update:modelValue', false)"></div>

            <!-- Modal -->
            <div class="relative bg-white rounded-lg shadow-xl w-full max-w-md mx-4 transform transition-all">
                <!-- Header -->
                <div class="bg-purple-800 text-white px-6 py-4 rounded-t-lg flex justify-between items-center">
                    <h3 class="text-white text-xl font-bold mt-2">Add New Reading</h3>
                    <button @click="$emit('update:modelValue', false)" class="text-white hover:text-purple-200">
                        <XIcon class="w-6 h-6" />
                    </button>
                </div>

                <!-- Form -->
                <form @submit.prevent="handleSubmit" class="p-6 space-y-4">
                    <!-- Meter Selection -->
                    <div class="space-y-2">
                        <label class="text-sm font-medium text-purple-800">Electricity Meter</label>
                        <select v-model="formData.electricity_meter_id" required
                            class="w-full p-3 rounded-lg border border-purple-200 focus:ring-2 focus:ring-purple-400 focus:border-purple-400 transition-all">
                            <option value="" disabled>Select a meter</option>
                            <option v-for="meter in meters" :key="meter.ID" :value="meter.ID">
                                Meter #{{ meter.ID }} - {{ getMeterLocation(meter) }}
                            </option>
                        </select>
                    </div>

                    <!-- Lower Reading -->
                    <div class="space-y-2">
                        <label class="text-sm font-medium text-purple-800">Lower Reading</label>
                        <input v-model="formData.lower_reading" type="text" required
                            class="w-full p-3 rounded-lg border border-purple-200 focus:ring-2 focus:ring-purple-400 focus:border-purple-400 transition-all"
                            placeholder="Enter lower reading value" />
                    </div>

                    <!-- Upper Reading -->
                    <div class="space-y-2">
                        <label class="text-sm font-medium text-purple-800">Upper Reading</label>
                        <input v-model="formData.upper_reading" type="text" required
                            class="w-full p-3 rounded-lg border border-purple-200 focus:ring-2 focus:ring-purple-400 focus:border-purple-400 transition-all"
                            placeholder="Enter upper reading value" />
                    </div>

                    <!-- Reading Date Options -->
                    <div class="space-y-2">
                        <label class="text-sm font-medium text-purple-800">Reading Date</label>
                        <div class="space-y-3">
                            <!-- Date Option Selection -->
                            <div class="flex items-center space-x-4">
                                <label class="flex items-center">
                                    <input type="radio" v-model="dateOption" value="current"
                                        class="w-4 h-4 text-purple-600 border-purple-300 focus:ring-purple-500">
                                    <span class="ml-2 text-sm text-purple-800">Use current time</span>
                                </label>
                                <label class="flex items-center">
                                    <input type="radio" v-model="dateOption" value="custom"
                                        class="w-4 h-4 text-purple-600 border-purple-300 focus:ring-purple-500">
                                    <span class="ml-2 text-sm text-purple-800">Select date & time</span>
                                </label>
                            </div>

                            <!-- Custom Date Input -->
                            <input v-if="dateOption === 'custom'" v-model="formData.reading_date" type="datetime-local"
                                required
                                class="w-full p-3 rounded-lg border border-purple-200 focus:ring-2 focus:ring-purple-400 focus:border-purple-400 transition-all" />
                        </div>
                    </div>

                    <!-- Buttons -->
                    <div class="flex space-x-3 pt-4">
                        <button type="button" @click="$emit('update:modelValue', false)"
                            class="flex-1 p-3 rounded-lg border border-purple-200 text-purple-800 hover:bg-purple-50 transition-colors">
                            Cancel
                        </button>
                        <button type="submit"
                            class="flex-1 p-3 rounded-lg bg-purple-600 text-white hover:bg-purple-700 transition-colors">
                            Add Reading
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
import type { CreateElectricityReading, CreateElectricityReadingWithDate } from "@/app/models/backend/electricity_reading";
import type { ElectricityMeter } from "@/app/models/backend/electricity_meter";
import type { Location } from "@/app/models/backend/location";

const props = defineProps<{
    modelValue: boolean;
    meters: ElectricityMeter[];
    locations: Location[];
}>();

const emit = defineEmits<{
    (e: "update:modelValue", value: boolean): void;
    (e: "submit", value: CreateElectricityReadingWithDate): void;
}>();

const dateOption = ref<'current' | 'custom'>('current');

const formData = ref<CreateElectricityReadingWithDate>({
    electricity_meter_id: 0,
    lower_reading: '',
    upper_reading: '',
    reading_date: '',
});

const getMeterLocation = (meter: ElectricityMeter): string => {
    const location = props.locations.find(l => l.ID === meter.location_id);
    return location ? `${location.street} ${location.number}` : 'Unknown Location';
};

const handleSubmit = () => {
    const submitData: CreateElectricityReadingWithDate = {
        ...formData.value,
        reading_date: dateOption.value === 'current'
            ? new Date().toISOString()
            : new Date(formData.value.reading_date).toISOString()
    };

    emit("submit", submitData);
    formData.value = {
        electricity_meter_id: 0,
        lower_reading: '',
        upper_reading: '',
        reading_date: '',
    };
    dateOption.value = 'current';
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