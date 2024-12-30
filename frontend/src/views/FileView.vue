<script setup>
import axios from 'axios';
import { ref, reactive, onMounted, watchEffect } from 'vue';
import { useRoute, useRouter, RouterLink } from 'vue-router';
import PulseLoader from 'vue-spinner/src/PulseLoader.vue';
import FileIcon from '@/components/FileIcon.vue';
import fileImage from "@/assets/File.svg";
import ControlButton from '@/components/ControlButton.vue';

const router = useRouter();
const route = useRoute();
const decodeText = ref('');
const fileId = route.params.id;
const imageExtensions = ['.jpeg', '.jpg', '.png', '.gif'];
const backendUri = import.meta.env.VITE_BASE_BACKEND_URI;

const isEditing = ref(false);
const newFileName = ref('');
const editError = ref('');
const isRefreshing = ref(false);

const state = reactive({
  file: {},
  isLoading: true,
  imageUrl: null,
});

watchEffect(() => {
  if (state.file.extension && state.file.extension.toLowerCase() === '.txt') {
    decodeText.value = atob(state.file.data);
  }
});

onMounted(async () => {
  try {
    const { data } = await axios.get(`${backendUri}/api/datafile/${fileId}`);
    state.file = data;

    if (imageExtensions.includes(state.file.extension.toLowerCase())) {
      state.imageUrl = `data:image/${state.file.extension.slice(1)};base64,${data.data}`;
    }
  } catch (error) {
    console.error("Error fetching file: " + error);
  } finally {
    state.isLoading = false;
  }
}); 

async function deleteFile(fileId) {
  try {
    await axios.delete(`${backendUri}/api/datafile/${fileId}`);
    router.push({ path: '/' });
  } catch (error) {
    console.error("Error deleting file: " + error);
  }
}

async function triggerEdit() {
  isEditing.value = !isEditing.value;
  if (isEditing.value) {
    newFileName.value = state.file.fileName;
  } else {
    editError.value = '';
  }
}

async function editFile() {
  try {
    editError.value = '';

    if (!newFileName.value.trim()) {
      editError.value = "Filename cannot be empty.";
      return;
    }

    const extension = state.file.extension;
    const nameWithoutExt = newFileName.value.replace(extension, '');
    const finalFileName = nameWithoutExt + extension;
    
    isRefreshing.value = true;
    const response = await axios.put(`${backendUri}/api/datafile/${fileId}`, {
      fileName: finalFileName
    });

    state.file = response.data;
    isEditing.value = false;

    const { data } = await axios.get(`${backendUri}/api/datafile/${fileId}`);
    state.file = data;

    if (imageExtensions.includes(state.file.extension.toLowerCase())) {
      state.imageUrl = `data:image/${state.file.extension.slice(1)};base64,${data.data}`;
    }
    
    if (state.file.extension.toLowerCase() === '.txt') {
      decodeText.value = atob(state.file.data);
    }

  } catch (error) {
    console.error("Error updating filename: ", error);
    editError.value = "Failed to update filename. Please try again.";
  } finally {
    isRefreshing.value = false;
  }
}

async function cancelEdit() {
  isEditing.value = false;
  editError.value = '';
  newFileName.value = state.file.fileName;
}

async function downloadFile() {
  try {
    const response = await axios.get(`${backendUri}/api/datafile/download/${fileId}`, {
      responseType: 'blob',
    });

    const blob = new Blob([response.data], { type: response.headers['content-type'] });
    const url = URL.createObjectURL(blob);

    const link = document.createElement('a');
    link.href = url;
    link.download = state.file.fileName;
    link.click();

    URL.revokeObjectURL(url);
  } catch (error) {
    console.error("Error downloading file. " + error);
  }
}
</script>


<template>
  <div v-if="state.isLoading" class="absolute top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2 bg-transparent text-center">
    <PulseLoader color="#F97316" />
  </div>

  <div v-else class="md:mt-16">
    <div class="container m-auto">
      <RouterLink to="/" class="underline text-orange-500">
        <i class="pi pi-arrow-left"></i>
        Back
      </RouterLink>
      <div class="mt-6 grid grid-cols-4 gap-6">
        <div class="col-span-1 flex flex-col gap-2">
          <FileIcon :image="fileImage" :file="state.file" />
          <h4 class="text-xl font-semibold">Controls</h4>
          <div class="flex items-center flex-wrap gap-2">
            <ControlButton text="Download" :onclick="downloadFile" />
            <ControlButton text="Delete" :onclick="() => deleteFile(state.file.id)" />
            <ControlButton text="Edit" :onclick="triggerEdit" />
          </div>
          <div v-if="isEditing" class="mt-4">
            <hr class="border-t-1 border-gray-500 mb-2">
            <div class="flex flex-col gap-2">
              <p class="text-base">Filename:</p>
              <input 
                type="text"
                v-model="newFileName"
                class="border rounded px-2 py-1"
                :class="{ 'border-red-500': editError }"
                :disabled="isRefreshing"
              />
              <p v-if="editError" class="text-red-500 text-sm">{{ editError }}</p>
              <div class="flex items-center flex-wrap gap-2">
                <div v-if="isRefreshing" class="text-orange-500">
                  <PulseLoader :loading="true" :color="'#F97316'" :size="'8px'" />
                </div>
                <template v-else>
                  <ControlButton text="Confirm" :onclick="editFile" />
                  <ControlButton text="Cancel" :onclick="cancelEdit" />
                </template>
              </div>
            </div>
          </div>
        </div>
        <div class="col-span-3">
          <div class="bg-white shadow-md flex flex-wrap md:justify-start justify-center items-center gap-6 py-4 px-6 border-orange-50">
            <div v-if="state.file.extension && imageExtensions.includes(state.file.extension.toLowerCase())" 
              class="w-full"
            >
              <img :src="state.imageUrl" class="mx-auto object-contain" alt="Image" />
            </div>
            <div v-else-if="state.file.extension && state.file.extension.toLowerCase() === '.pdf'" class="w-full">
              <iframe :src="`data:application/pdf;base64,${state.file.data}`" width="100%" height="800"></iframe>
            </div>
            <div v-else-if="state.file.extension && state.file.extension.toLowerCase() === '.txt'" class="w-full">
              <div>{{ decodeText }}</div>
            </div>
            <div v-else class="w-full">
              <p>File Type is unreadable</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
