<script setup>
defineProps({
  statusCode: {
    type: Number,
    default: 404
  },
  message: {
    type: String,
    default: 'Page does not exist'
  },
})
</script>

<template>
  <div class="flex justify-center items-center flex-col min-h-screen">
    <h1 class="text-6xl font-bold">404 Not Found</h1>
    <h2 class="text-3xl mt-2">Page does not exist</h2>
    <a href="/" class="mt-6 underline tracking-tight text-orange-500 hover:text-orange-600">Return to homepage</a>
  </div>
</template>