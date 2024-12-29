<script setup>
defineProps({
  image: {
    type: String,
    required: true,
  },
  file: {
    type: Object,
    required: true,
  },
})
</script>

<template>
  <div class="flex justify-center items-center flex-col border-2 border-orange-500">
    <img :src="image" class="object-cover" alt="file" />
    <div className="flex flex-col w-full p-3 justify-start items-start">
      <p>File Name: {{ file.fileName }}</p>
      <p>Size: {{ file.fileSize }} bytes</p>
      <p>Extension: {{ file.extension }}</p>
    </div>
  </div>
</template>