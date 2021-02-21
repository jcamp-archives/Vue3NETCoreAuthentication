<template>
  <ul class="nav nav-pills flex-column">
    <li v-for="(item, i) in items" :key="i" class="nav-item">
      <router-link v-if="item.exact" class="nav-link" :to="item.link" exact-active-class="active">
        {{ item.title }}
      </router-link>
      <router-link v-if="!item.exact" class="nav-link" :to="item.link" exact active-class="active">
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
