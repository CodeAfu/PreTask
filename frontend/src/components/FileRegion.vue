<script setup>
import File from '@/components/File.vue';
import axios from 'axios';

defineProps({
  files: {
    type: Array,
    default: () => []
  }
});

const backendUri = import.meta.env.VITE_BASE_BACKEND_URI;
const emit = defineEmits(['fileDeleted']);

async function uploadFile(event) {
  const file = event.target.files[0];

  if (!file) {
    console.error("No file selected.");
    return;
  }

  try {
    const formData = new FormData();
    formData.append('file', file);
    const response = await axios.post(`${backendUri}/api/datafile/upload`, formData, {
      headers: {
        'Content-Type': 'multipart/form-data'
      }
    });

    console.log("File uploaded successfully: ", response.data)
    emit('fileUploaded', response.data);
  } catch (error) {
    console.error("Error uploading file: " + error);
  }
}

function removeFile(deletedFileId) {
  emit('fileDeleted', deletedFileId);
}
</script>

<template>
  <div class="mt-6 w-full flex justify-end gap-3">
    <input 
      type="file"
      id="fileInput" 
      @change="uploadFile"
      style="display: none;"
    />
    <label 
      for="fileInput"
      class="button-menu"
    >
      Upload
    </label>
  </div>

  <div class="flex justify-center items-center w-full">
    <div class="bg-white shadow-md flex flex-wrap md:justify-start justify-center items-center gap-6 py-4 px-6 border-orange-50 w-full">
      <File v-for="file in files" 
        :key="file.id" 
        :id="file.id" 
        :name="file.fileName" 
        :size="file.fileSize" 
        :extension="file.extension" 
        @fileDeleted="removeFile" />
    </div>
  </div>
</template>

<style scoped>
</style>