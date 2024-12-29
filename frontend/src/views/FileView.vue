<script setup>
import axios from 'axios';
import { reactive, onMounted } from 'vue';
import { useRoute, RouterLink } from 'vue-router';
import PulseLoader from 'vue-spinner/src/PulseLoader.vue';
import FileIcon from '@/components/FileIcon.vue';
import fileImage from "@/assets/File.svg"

const route = useRoute();
const fileId = route.params.id;
const backendUri = import.meta.env.VITE_BASE_BACKEND_URI
const state = reactive( {
  file: {},
  isLoading: true,
  imageUrl: null,
})


onMounted(async () => {
  try {
    const { data } = await axios.get(`${backendUri}/api/datafile/${fileId}`);
    state.file = data;

    if (['.jpeg', '.jpg', '.png', '.gif'].includes(state.file.extension.toLowerCase())) {
      state.imageUrl = URL.createObjectURL(data.data);
    }
  } catch (error) {
    console.error("Error fetching file. " + error);
  } finally {
    state.isLoading = false;
  }
});



</script>

<template>
  <div v-if="isLoading" class="absolute top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2 bg-transparent text-center">
    <PulseLoader color="#F97316" />
  </div>

  <div v-else class="md:mt-16">
    <div class="container m-auto">
      <RouterLink to="/" class="underline text-orange-500">
        <i class="pi pi-arrow-left"></i>
        Back
      </RouterLink>
      <div class="mt-6 grid grid-cols-4 gap-6">
        <div class="col-span-1">
          <FileIcon :image="fileImage" :file="state.file" />
        </div>
        <div class="col-span-3">
          <h2 class="text-3xl font-bold">{{ state.file.fileName }}</h2>
          <p class="text-lg">File size: {{ state.file.fileSize }} bytes</p>
          <p class="text-lg">File extension: {{ state.file.extension }}</p>
          <div v-if="state.file.extension === '.jpeg'">
            <img :src="imageUrl" class="object-cover" alt="file" />
          </div>
        </div>
      </div>
    </div>
  </div>

</template>