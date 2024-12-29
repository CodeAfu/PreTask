
<script setup>
defineProps({
  title: {
    type: String,
    default: ''
  }
})
</script>

<template>
  <div class="flex justify-center items-center mt-6">
    <h1 class="text-5xl font-semibold tracking-wider">
      {{ title }}
    </h1>
  </div>
</template>