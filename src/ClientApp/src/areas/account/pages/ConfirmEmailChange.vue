<template>
  <h1>Confirm Email Change</h1>
  <TwAlertSuccess v-if="success">{{ success }}</TwAlertSuccess>
  <TwAlertDanger v-if="error">{{ error }}</TwAlertDanger>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import axios from 'axios'
import authStore from '~/store/authStore'

const success = ref('')
const error = ref('')
const router = useRouter()
const route = useRoute()

onMounted(async () => {
  const userId = route.query.userId
  const code = route.query.code
  const email = route.query.email

  if (!userId || !code || !email) {
    router.push('/')
  }

  let url = '/api/account/confirmemailchange'

  if (authStore.isAuthenticated) url = '/api/account/manage/confirmemailchange'

  try {
    const response = await axios.post(url, { userId, code, email })
    if (authStore.isAuthenticated && response.data.token) authStore.login(response.data.token)
    success.value = response.data.message
  } catch {
    router.push('/')
  }
})
</script>
