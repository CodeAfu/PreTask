<script setup>
import { formatBytes } from '@/utility/utils';
defineProps({
  image: {
    type: String,
    required: true,
  },
  file: {
    type: Object,
    required: true,
  },
});
</script>

<template>
  <div class="flex flex-col items-center justify-center bg-orange-50 rounded-md shadow-md p-4">
    <img :src="image" class="object-cover" alt="file" />
    <div className="flex flex-col w-full p-3 justify-start items-start">
      <p>File Name: {{ file.fileName }}</p>
      <p>Size: {{ formatBytes(file.fileSize) }}</p>
      <p>Extension: {{ file.extension }}</p>
    </div>
  </div>
</template>