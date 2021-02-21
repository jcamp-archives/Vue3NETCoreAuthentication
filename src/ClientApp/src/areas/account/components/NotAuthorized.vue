<template>
  <template v-if="!isAuthorized">
    <slot></slot>
  </template>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import authStore from '~/store/authStore'

const isAuthorized = computed(() => authStore.isAuthenticated)
</script>
