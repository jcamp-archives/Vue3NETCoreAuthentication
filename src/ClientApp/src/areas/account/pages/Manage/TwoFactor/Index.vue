<template>
  <h2>Two Factor Authentication</h2>
  <TwAlertSuccess v-if="message">{{ message }}</TwAlertSuccess>
  <TwAlertDanger v-if="error">{{ error }}</TwAlertDanger>

  <div v-if="model.isMfaEnabled">
    <div v-if="model.recoveryCodesLeft == 0" class="alert alert-danger">
      <strong>You have no recovery codes left.</strong>
      <p>
        You must
        <router-link to="/Account/Manage/TwoFactor/RecoveryCodes">generate a new set of recovery codes</router-link>
        before you can log in with a recovery code.
      </p>
    </div>
    <div v-else-if="model.recoveryCodesLeft == 1" class="alert alert-danger">
      <strong>You have 1 recovery code left.</strong>
      <p>
        You can
        <router-link to="/Account/Manage/TwoFactor/RecoveryCodes">generate a new set of recovery codes</router-link>.
      </p>
    </div>
    <div v-else-if="model.recoveryCodesLeft <= 3" class="alert alert-warning">
      <strong>You have {{ model.recoveryCodesLeft }} recovery codes left.</strong>
      <p>
        You should
        <router-link to="/Account/Manage/TwoFactor/RecoveryCodes">generate a new set of recovery codes</router-link>.
      </p>
    </div>

    <button
      v-if="model.isMachineRemembered"
      style="margin-right: 4px"
      class="btn btn-primary"
      @click.prevent="forgetBrowser"
    >
      Forget this browser
    </button>

    <router-link to="/Account/Manage/TwoFactor/Disable" class="mr-2 btn btn-primary">Disable 2FA</router-link>
    <router-link to="/Account/Manage/TwoFactor/RecoveryCodes" class="btn btn-primary">Reset recovery codes</router-link>
  </div>

  <h5>Authenticator app</h5>
  <router-link v-if="!model.hasAuthenticator" to="/Account/Manage/TwoFactor/Enable" class="mr-2 btn btn-primary"
    >Add authenticator app</router-link
  >
  <router-link v-if="model.hasAuthenticator" to="/Account/Manage/TwoFactor/Enable" class="mr-2 btn btn-primary"
    >Setup authenticator app</router-link
  >
  <router-link v-if="model.hasAuthenticator" to="/Account/Manage/TwoFactor/Reset" class="btn btn-primary"
    >Reset authenticator app</router-link
  >
</template>

<script setup lang="ts">
import axios from 'axios'
import { ref, reactive, onBeforeMount } from 'vue'
import { useRouter } from 'vue-router'
import type { IMfaInfo } from '../../../models'

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
