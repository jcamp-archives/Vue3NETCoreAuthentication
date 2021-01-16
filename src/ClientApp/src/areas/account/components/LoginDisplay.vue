<template>
  <Authorized v-slot="{ userName }">
    <router-link to="/account/manage/">Hello, {{ userName }}!</router-link>
    <button class="nav-link btn btn-link" @click.prevent="beginLogout">Log out</button>
  </Authorized>
  <NotAuthorized>
    <router-link to="/account/register">Register</router-link>
    <button class="nav-link btn btn-link" @click.prevent="beginLogin">Log in</button>
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
