<template>
  <h2>Disable multi-factor authentication (MFA)</h2>

  <div class="alert alert-warning" role="alert">
    <p>
      <strong>This action only disables MFA.</strong>
    </p>
    <p>
      Disabling MFA does not change the keys used in authenticator apps. If you wish to change the key used in an
      authenticator app you should
      <router-link to="/account/manage/twofactor/reset">reset your authenticator keys.</router-link>
    </p>
  </div>

  <div>
    <button class="mt-4 bg-red-400 btn" @click.prevent="disableMfa">Disable MFA</button>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import axios from 'axios'
import { useRouter } from 'vue-router'

const router = useRouter()
const message = ref('')
const error = ref('')

const disableMfa = async () => {
  message.value = ''
  error.value = ''
  try {
    await axios.post('/api/account/manage/mfadisable', {})
    router.push('/account/manage/twofactor/')
  } catch (ex) {
    error.value = ex.response.data.message
  }
}
</script>
