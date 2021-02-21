<template>
  <ul class="flex flex-col p-4 space-y-2">
    <li v-for="(item, i) in items" :key="i" class="nav-item">
      <router-link :to="item.link">
        {{ item.title }}
      </router-link>
    </li>
  </ul>
</template>

<script setup lang="ts">
import { reactive } from 'vue'

const items = reactive([
  { title: 'Profile', link: '/account/manage', exact: true },
  { title: 'Email', link: '/Account/manage/Email', exact: true },
  { title: 'Password', link: '/Account/manage/ChangePassword', exact: true },
  {
    title: 'Two-factor authentication',
    link: '/Account/Manage/TwoFactor',
    exact: false
  },
  { title: 'Personal data', link: '/account/manage/personaldata', exact: false }
])
</script>

<style lang="postcss" scoped>
li a {
  @apply flex flex-row items-center h-12 px-4 py-2 text-sm text-blue-600 rounded hover:bg-gray-100;
}
li :deep(a.router-link-exact-active) {
  @apply text-white bg-blue-600;
}
</style>
