<template>
  <h1>Confirm Email Change</h1>

  <div v-if="success" class="alert alert-success" role="alert">
    {{ success }}
  </div>
  <div v-if="error" class="alert alert-danger" role="alert">
    {{ error }}
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import axios from 'axios'
import authStore from '@/store/authStore'

const success = ref('')
const error = ref('')
const router = useRouter()
const route = useRoute()

const mounted = onMounted(async () => {
  let userId = route.query.userId
  let code = route.query.code
  let email = route.query.email

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
