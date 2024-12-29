<script setup>
import ControlButton from './ControlButton.vue'
import File from './File.vue'

defineProps({
  files: {
    type: Array,
    default: () => []
  }
})
</script>

<template>
  <div class="mt-6 w-full flex justify-end gap-3">
    <ControlButton text="Select" />
    <ControlButton text="Upload" />
    <ControlButton text="Download" />
  </div>

  <div class="flex justify-center items-center w-full">
    <div class="bg-white flex flex-wrap md:justify-start justify-center items-center gap-6 py-4 px-6 border-orange-50 w-full">
      <!-- <File name="Test.png" size="100" extension=".png" /> -->
      <div v-for="file in files" :key="file.id">
        <File :id="file.id" :name="file.fileName" :size="file.fileSize" :extension="file.extension" />
      </div>
    </div>
  </div>
</template>