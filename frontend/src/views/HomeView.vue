<script setup>
import { ref, onMounted } from 'vue';
import SearchBar from '@/components/SearchBar.vue';
import FileRegion from '@/components/FileRegion.vue';
import Header from '@/components/Header.vue';
import axios from 'axios';
import PulseLoader from "vue-spinner/src/PulseLoader.vue";

const files = ref([]);
const isLoading = ref(true);

const backendUri = import.meta.env.VITE_BASE_BACKEND_URI

onMounted(async () => {
  try {
    const response = await axios.get(`${backendUri}/api/datafile`);
    files.value = response.data;
  } catch (error) {
    console.error('Error fetching files. ' + error);
  } finally {
    isLoading.value = false;
  }
})

function removeFile(deletedFileId) {
  files.value = files.value.filter(file => file.id !== deletedFileId);
}
</script>

<template>
  <div class="container m-auto ">
    <Header title="File Manager" />
    <div class="bg-primary-lighter flex flex-col items-center gap-3 justify-center mt-12 text-xl p-3">
      <!-- Loading -->
      <div v-if="isLoading" class="absolute top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2 bg-transparent text-center">
        <PulseLoader color="#F97316" />
      </div>
      <div v-else class="w-full flex flex-col gap-3">
        <SearchBar />
        <FileRegion :files="files" @fileDeleted="removeFile" />
      </div>
    </div>
  </div>
</template>