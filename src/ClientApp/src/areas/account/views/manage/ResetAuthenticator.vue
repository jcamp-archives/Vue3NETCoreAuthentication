<template>
  <h1>Reset authenticator key</h1>

  <div class="alert alert-warning" role="alert">
    <p>
      <span class="glyphicon glyphicon-warning-sign"></span>
      <strong
        >If you reset your authenticator key your authenticator app will not work until you
        reconfigure it.</strong
      >
    </p>
    <p>
      This process disables 2FA until you verify your authenticator app. If you do not complete your
      authenticator app configuration you may lose access to your account.
    </p>
  </div>
  <div>
    <button @click.prevent="resetAuth" class="btn btn-danger">Reset authenticator key</button>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import axios from 'axios'
import { useRouter } from 'vue-router'

const router = useRouter()
const message = ref('')
const error = ref('')
const codes = ref([])

const resetAuth = async () => {
  message.value = ''
  error.value = ''
  try {
    await axios.post('/api/account/manage/mfaresetkey', {})
    router.push('/account/manage/twofactor/')
  } catch (ex) {
    error.value = ex.response.data.message
  }
}
</script>
