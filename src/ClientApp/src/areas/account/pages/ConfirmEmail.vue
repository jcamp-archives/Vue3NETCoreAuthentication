<template>
  <h1>Confirm Email</h1>
  <TwAlertSuccess v-if="success">{{ success }}</TwAlertSuccess>
  <TwAlertDanger v-if="error">{{ error }}</TwAlertDanger>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import axios from 'axios'

const success = ref('')
const error = ref('')
const router = useRouter()
const route = useRoute()

onMounted(async () => {
  const userId = route.query.userId
  const code = route.query.code

  if (!userId || !code) {
    router.push('/')
  }

  try {
    const response = await axios.post('/api/account/confirmemail', { userId, code })
    success.value = response.data.message
  } catch (ex) {
    error.value = ex.response.data.message
  }
})
</script>
