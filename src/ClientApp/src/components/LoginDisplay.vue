<template>
  <template v-if="loggedIn">
    <router-link to="/account/manage/">Hello, {{ userName }}!</router-link>
    <button class="nav-link btn btn-link" @click.prevent="beginLogout">Log out</button>
  </template>
  <template v-else>
    <router-link to="/account/register">Register</router-link>
    <button class="nav-link btn btn-link" @click.prevent="beginLogin">Log in</button>
  </template>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import authStore from '@/store/authStore'

const router = useRouter()
const route = useRoute()

const loggedIn = computed(() => {
  return authStore.isAuthenticated
})

const userName = computed(() => {
  return authStore.userName
})

const beginLogin = () => {
  router.push({ path: '/account/login', query: { returnUrl: route.fullPath } })
}
const beginLogout = () => {
  router.push({ path: '/account/logout', query: { returnUrl: route.fullPath } })
}
</script>
