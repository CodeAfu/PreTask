
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
    onclick=onclick
    class="button-menus"
  >
  {{ text }}
  </button>
</template>