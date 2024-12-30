<script setup>
import { formatBytes, formatDate } from '@/utility/utils';
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
    <img :src="image" class="object-cover mb-4" alt="file" />
    <div class="w-full max-w-xl">
      <div class="grid grid-cols-[auto_1fr] gap-x-3">
        <p class="font-bold text-right whitespace-nowrap">Name:</p>
        <p class="break-words">{{ file.fileName }}</p>
        
        <p class="font-bold text-right whitespace-nowrap">Size:</p>
        <p class="break-words">{{ formatBytes(file.fileSize) }}</p>
        
        <p class="font-bold text-right whitespace-nowrap">Ext:</p>
        <p class="break-words">{{ file.extension }}</p>
        
        <p class="font-bold text-right whitespace-nowrap">Date:</p>
        <p class="break-words">{{ formatDate(file.uploadDate) }}</p>
      </div>
    </div>
  </div>
</template>