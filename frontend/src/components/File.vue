<script setup>
import { defineEmits } from 'vue';
import fileImage from "@/assets/File.svg";
import axios from 'axios';
import { formatBytes } from '@/utility/utils';

defineProps({
  id: {
    type: String,
    required: true
  },
  name: {
    type: String,
    required: true
  },
  size: {
    type: Number,
    default: 0
  },
  extension: {
    type: String,
    default: ''
  },
});

const backendUri = import.meta.env.VITE_BASE_BACKEND_URI;
const emit = defineEmits(['fileDeleted']);

async function deleteFile(fileId) {
  try {
    await axios.delete(`${backendUri}/api/datafile/${fileId}`);
    emit('fileDeleted', fileId);
  } catch (error) {
    console.error("Error deleting file: " + error);
  }
}
</script>

<template>
  <div class="flex flex-col justify-between gap-3 lg:w-[175px] w-[150px] h-[240px] text-sm border border-orange-200 bg-orange-50 p-3 overflow-hidden">
    <div>
      <div class="flex items-center justify-center">
        <img :src="fileImage" class="object-cover" alt="file" />
      </div>
      <div class="flex justify-between">
        <div class="flex flex-col mt-1 max-h-16">
          <div>Name: {{ name }}</div>
          <div>Size: {{ formatBytes(size) }}</div>
        </div>
      </div>
    </div>
    <div class="flex justify-start items-center gap-1.5 underline text-orange-500 ">
      <RouterLink :to="`/files/${id}`" class="hover:text-orange-700">View</RouterLink>
      <button @click="() => deleteFile(id)" class="hover:text-orange-700">Delete</button>
    </div>
  </div>

</template>

