<template>
  <h1>Confirm Email</h1>

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

const success = ref('')
const error = ref('')
const router = useRouter()
const route = useRoute()

onMounted(async () => {
  let userId = route.query.userId
  let code = route.query.code

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
