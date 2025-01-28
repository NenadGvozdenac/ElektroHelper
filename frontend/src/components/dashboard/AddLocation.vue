<template>
  <Transition name="modal">
    <div v-if="modelValue" class="fixed inset-0 z-50 flex items-center justify-center">
      <!-- Backdrop -->
      <div class="absolute inset-0 bg-black bg-opacity-50" @click="$emit('update:modelValue', false)"></div>

      <!-- Modal -->
      <div class="relative bg-white rounded-lg shadow-xl w-full max-w-md mx-4 transform transition-all">
        <!-- Header -->
        <div class="bg-emerald-800 text-white px-6 py-4 rounded-t-lg flex justify-between items-center">
          <h3 class="text-white text-xl font-bold mt-2">Add New Location</h3>
          <button @click="$emit('update:modelValue', false)" class="text-white hover:text-emerald-200">
            <XIcon class="w-6 h-6" />
          </button>
        </div>

        <!-- Form -->
        <form @submit.prevent="handleSubmit" class="p-6 space-y-4">
          <!-- Street -->
          <div class="space-y-2">
            <label class="text-sm font-medium text-emerald-800">Street</label>
            <input v-model="formData.street" type="text" required
              class="w-full p-3 rounded-lg border border-emerald-200 focus:ring-2 focus:ring-emerald-400 focus:border-emerald-400 transition-all"
              placeholder="Enter street name" />
          </div>

          <!-- Number -->
          <div class="space-y-2">
            <label class="text-sm font-medium text-emerald-800">Number</label>
            <input v-model="formData.number" type="text" required
              class="w-full p-3 rounded-lg border border-emerald-200 focus:ring-2 focus:ring-emerald-400 focus:border-emerald-400 transition-all"
              placeholder="Enter street number" />
          </div>

          <!-- City -->
          <div class="space-y-2">
            <label class="text-sm font-medium text-emerald-800">City</label>
            <input v-model="formData.city" type="text" required
              class="w-full p-3 rounded-lg border border-emerald-200 focus:ring-2 focus:ring-emerald-400 focus:border-emerald-400 transition-all"
              placeholder="Enter city name" />
          </div>

          <!-- Country -->
          <div class="space-y-2">
            <label class="text-sm font-medium text-emerald-800">Country</label>
            <select v-model="formData.country" required
              class="w-full p-3 rounded-lg border border-emerald-200 focus:ring-2 focus:ring-emerald-400 focus:border-emerald-400 transition-all">
              <option value="" disabled>Select a country</option>
              <option v-for="country in countries" :key="country" :value="country">
                {{ country }}
              </option>
            </select>
          </div>

          <!-- Postal Code -->
          <div class="space-y-2">
            <label class="text-sm font-medium text-emerald-800">Postal Code</label>
            <input v-model="formData.postal_code" required
              class="w-full p-3 rounded-lg border border-emerald-200 focus:ring-2 focus:ring-emerald-400 focus:border-emerald-400 transition-all"
              placeholder="Enter postal code" />
          </div>

          <!-- Has Electricity Meter Checkbox -->
          <div class="flex items-center space-x-2 pt-2">
            <input type="checkbox" id="hasElectricityMeter" v-model="formData.hasElectricityMeter"
              class="h-4 w-4 rounded border-emerald-300 text-emerald-600 focus:ring-emerald-500" />
            <label for="hasElectricityMeter" class="text-sm font-medium text-emerald-800">
              Has electricity meter?
            </label>
          </div>

          <!-- Buttons -->
          <div class="flex space-x-3 pt-4">
            <button type="button" @click="$emit('update:modelValue', false)"
              class="flex-1 p-3 rounded-lg border border-emerald-200 text-emerald-800 hover:bg-emerald-50 transition-colors">
              Cancel
            </button>
            <button type="submit"
              class="flex-1 p-3 rounded-lg bg-emerald-600 text-white hover:bg-emerald-700 transition-colors">
              Add Location
            </button>
          </div>
        </form>
      </div>
    </div>
  </Transition>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { XIcon } from "lucide-vue-next";
import type { CreateLocation } from "@/app/models/location";
import type { CreateLocationWithMeter } from "@/app/models/electricity_meter";

const props = defineProps<{
  modelValue: boolean;
}>();

const emit = defineEmits<{
  (e: "update:modelValue", value: boolean): void;
  (e: "submit", value: CreateLocationWithMeter): void;
}>();

const formData = ref<CreateLocationWithMeter>({
  street: "",
  number: "",
  city: "",
  country: "",
  postal_code: "",
  hasElectricityMeter: false,
});

const countries = [
  "United States",
  "Canada",
  "United Kingdom",
  "Germany",
  "France",
  "Spain",
  "Italy",
  "Netherlands",
  "Belgium",
  "Switzerland",
  "Serbia"
];

const handleSubmit = () => {
  emit("submit", { ...formData.value });
  formData.value = {
    street: "",
    number: "",
    city: "",
    country: "",
    postal_code: "",
    hasElectricityMeter: false,
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