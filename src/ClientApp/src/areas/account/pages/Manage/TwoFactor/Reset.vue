<template>
  <h2>Reset authenticator key</h2>

  <div class="alert alert-warning" role="alert">
    <p>
      <span class="glyphicon glyphicon-warning-sign"></span>
      <strong>
        If you reset your authenticator key your authenticator app will not work until you reconfigure it.
      </strong>
    </p>
    <p>
      This process disables 2FA until you verify your authenticator app. If you do not complete your authenticator app
      configuration you may lose access to your account.
    </p>
  </div>
  <div>
    <button class="mt-4 bg-red-400 btn" @click.prevent="resetAuth">Reset authenticator key</button>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import axios from 'axios'
import { useRouter } from 'vue-router'

const router = useRouter()
const message = ref('')
const error = ref('')

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
