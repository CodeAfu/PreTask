
<script setup>
defineProps({
  text: {
    type: String,
    default: ''
  },
  onclick: {
    type: Function,
    default: () => {}
  },
})
</script>

<template>
  <button
    type="button"
    @click=onclick
    class="button-menu"
  >
  {{ text }}
  </button>
</template>