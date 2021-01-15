<template>
  <h1>Two Factor Authentication</h1>
  <div v-if="message" class="alert alert-success" role="alert">{{ message }}</div>
  <div v-if="error" class="alert alert-danger" role="alert">{{ error }}</div>

  <div v-if="model.isMfaEnabled">
    <div v-if="model.recoveryCodesLeft == 0" class="alert alert-danger">
      <strong>You have no recovery codes left.</strong>
      <p>
        You must
        <router-link to="/Account/Manage/TwoFactor/RecoveryCodes"
          >generate a new set of recovery codes</router-link
        >
        before you can log in with a recovery code.
      </p>
    </div>
    <div v-else-if="model.recoveryCodesLeft == 1" class="alert alert-danger">
      <strong>You have 1 recovery code left.</strong>
      <p>
        You can
        <router-link to="/Account/Manage/TwoFactor/RecoveryCodes"
          >generate a new set of recovery codes</router-link
        >.
      </p>
    </div>
    <div v-else-if="model.recoveryCodesLeft <= 3" class="alert alert-warning">
      <strong>You have {{ model.recoveryCodesLeft }} recovery codes left.</strong>
      <p>
        You should
        <router-link to="/Account/Manage/TwoFactor/RecoveryCodes"
          >generate a new set of recovery codes</router-link
        >.
      </p>
    </div>

    <button
      v-if="model.isMachineRemembered"
      style="margin-right: 4px"
      @click.prevent="forgetBrowser"
      class="btn btn-primary"
    >
      Forget this browser
    </button>

    <router-link to="/Account/Manage/TwoFactor/Disable" class="btn btn-primary mr-2"
      >Disable 2FA</router-link
    >
    <router-link to="/Account/Manage/TwoFactor/RecoveryCodes" class="btn btn-primary"
      >Reset recovery codes</router-link
    >
  </div>

  <h5>Authenticator app</h5>
  <router-link
    v-if="!model.hasAuthenticator"
    to="/Account/Manage/TwoFactor/Enable"
    class="btn btn-primary mr-2"
    >Add authenticator app</router-link
  >
  <router-link
    v-if="model.hasAuthenticator"
    to="/Account/Manage/TwoFactor/Enable"
    class="btn btn-primary mr-2"
    >Setup authenticator app</router-link
  >
  <router-link
    v-if="model.hasAuthenticator"
    to="/Account/Manage/TwoFactor/Reset"
    class="btn btn-primary"
    >Reset authenticator app</router-link
  >
</template>

<script setup lang="ts">
import { ref, reactive, onBeforeMount } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import type { IMfaInfo } from '../../models'
import axios from 'axios'

const router = useRouter()
const message = ref('')
const error = ref('')
const model = reactive({} as IMfaInfo)

onBeforeMount(async () => {
  try {
    const response = await axios.get<IMfaInfo>('/api/account/manage/mfainfo')
    Object.assign(model, response.data)
  } catch (ex) {
    router.push('/account/login')
  }
})

const forgetBrowser = async () => {
  message.value = ''
  error.value = ''
  try {
    const response = await axios.post('/api/account/manage/mfaforgetbrowser')
    message.value = response.data.message
  } catch (ex) {
    error.value = ex.response.data.message
  }
}
</script>
