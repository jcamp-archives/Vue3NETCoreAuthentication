<template>
  <label class="block">
    <span v-if="props.label" class="text-gray-700">{{ props.label }}</span>
    <slot></slot>
  </label>
</template>

<script setup lang="ts">
import { defineProps } from 'vue'

const props = defineProps({
  label: {
    type: String,
    required: false
  }
})
</script>
