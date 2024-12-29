<script setup>
import { ref } from 'vue';
const searchQuery = ref('');
</script>

<template>
  <div class="flex w-full items-center justify-center">
    <input
    v-model="searchQuery"
    type="text"
    class="border rounded p-2 w-1/2"
    placeholder="Enter name of file"
  />

  <button
    onclick="alert('Remember to implement search!')"
    class="button-primary"
  >
    Search
  </button>
  </div>

</template>