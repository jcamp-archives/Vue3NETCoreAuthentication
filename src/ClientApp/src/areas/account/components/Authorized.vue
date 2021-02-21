<template>
  <template v-if="isAuthorized">
    <slot :userName="userName"></slot>
  </template>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import authStore from '~/store/authStore'

const userName = computed(() => authStore.userName)
const isAuthorized = computed(() => authStore.isAuthenticated)
</script>
