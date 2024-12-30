<script setup>
import { ref } from 'vue';

defineProps({
  modelValue: {
    type: String,
    default: ''
  }
});

const emit = defineEmits(['update:modelValue', 'search']);

const searchQuery = ref('');

function onSearch() {
  emit('search');
  emit('update:modelValue', searchQuery.value);
}

</script>

<template>
  <div class="flex w-full items-center justify-center">
    <input
    v-model="searchQuery"
    type="text"
    class="border rounded p-2 shadow-sm w-1/2"
    placeholder="Enter name of file"
  />
  <button @click="onSearch" class="button-primary">
    Search
  </button>
  </div>

</template>