<template>
  <Authorized v-slot="{ userName }">
    <div>
      <router-link to="/account/manage/">Hello, {{ userName }}!</router-link>
      <button class="px-4 focus:outline-none" @click.prevent="beginLogout">Log out</button>
    </div>
  </Authorized>
  <NotAuthorized>
    <div>
      <router-link to="/account/register">Register</router-link>
      <button class="px-4 focus:outline-none" @click.prevent="beginLogin">Log in</button>
    </div>
  </NotAuthorized>
</template>

<script setup lang="ts">
import { useRouter, useRoute } from 'vue-router'
import { Authorized, NotAuthorized } from './AuthorizeViews'

const router = useRouter()
const route = useRoute()

const beginLogin = () => {
  router.push({ path: '/account/login', query: { returnUrl: route.fullPath } })
}
const beginLogout = () => {
  router.push({ path: '/account/logout', query: { returnUrl: route.fullPath } })
}
</script>
