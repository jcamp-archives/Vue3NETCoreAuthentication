<template>
  <div id="app">
    <component :is="route.meta.layout || DefaultLayout">
      <router-view />
    </component>
  </div>
</template>

<script setup lang="ts">
import { useRoute } from 'vue-router'
import DefaultLayout from '@/layouts/DefaultLayout.vue'
const route = useRoute()
</script>
