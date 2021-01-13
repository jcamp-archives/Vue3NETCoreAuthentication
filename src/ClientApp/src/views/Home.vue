<template>
  <div v-if="loggedIn">Authenticated User Info</div>
  <div class="text-center">
    <img src="@/assets/logo.png" alt="Vuetify.js" class="mb-5" />
    <HelloWorld
      quote="Welcome to Your ASP.NET Core + Vue.js + TypeScript App"
      author="brought to you by Software Ateliers"
    />
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import authStore from '@/store/authStore'
import HelloWorld from '@/components/HelloWorld.vue' // @ is an alias to /src

const loggedIn = computed(() => {
  return authStore.isAuthenticated
})
</script>
