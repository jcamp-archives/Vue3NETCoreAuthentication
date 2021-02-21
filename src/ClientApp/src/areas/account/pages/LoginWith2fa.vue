<template>
  <h1>Two-factor authentication</h1>

  <p>Your login is protected with an authenticator app. Enter your authenticator code below.</p>

  <TwAlertSuccess v-if="message">{{ message }}</TwAlertSuccess>
  <TwAlertDanger v-if="error">{{ error }}</TwAlertDanger>

  <div class="card">
    <div class="card-body">
      <h5 class="card-title">Please enter your details</h5>
      <Form v-slot="{ errors }" :validation-schema="Schema" @submit="onSubmit">
        <div class="form-group">
          <label for="twoFactorCode">Verification Code</label>
          <Field
            v-model="model.twoFactorCode"
            v-focus
            name="twoFactorCode"
            type="text"
            class="form-control"
            :class="{ 'is-invalid': errors.twoFactorCode }"
          />
          <ErrorMessage class="invalid-feedback" name="twoFactorCode" />
        </div>
        <div class="form-check">
          <input id="rememberMachine" v-model="model.rememberMachine" type="checkbox" class="form-check-input" />
          <label class="form-check-label" for="rememberMachine">Remember this machine</label>
        </div>

        <button type="submit" class="mt-2 btn btn-primary">Submit</button>
      </Form>
    </div>
  </div>
  <p>
    Don't have access to your authenticator device? You can
    <router-link :to="{ path: '/account/LoginWithRecoveryCode', query: { returnUrl: returnUrl } }"
      >log in with a recovery code</router-link
    >.
  </p>
</template>

<script setup lang="ts">
import axios from 'axios'
import * as Yup from 'yup'
import { ref, reactive, onBeforeMount } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { Field, Form, ErrorMessage } from 'vee-validate'
import type { ILoginResult } from '../models'
import authStore from '~/store/authStore'

const router = useRouter()
const route = useRoute()
const message = ref('')
const error = ref('')
const returnUrl = ref('')
const model = reactive({ twoFactorCode: '', rememberMachine: false })

onBeforeMount(async () => {
  returnUrl.value = route.query.returnUrl as string
  try {
    await axios.post('/api/account/checkmfa', {})
  } catch (ex) {
    router.push({ path: '/account/login', query: { returnUrl: returnUrl.value } })
  }
})

const Schema = Yup.object().shape({
  twoFactorCode: Yup.string().label('Two Factor Code').required().min(6).max(7)
})

const onSubmit = async (values: any, actions: any) => {
  message.value = ''
  error.value = ''
  try {
    const response = await axios.post<ILoginResult>('/api/account/loginmfa', model)
    const result = response.data

    if (result.isSuccessful) {
      authStore.login(result.token)
      if (returnUrl.value) router.push(returnUrl.value)
      else router.push('/')
    }
  } catch (ex) {
    error.value = ex.response.data.message
    actions.setErrors(ex.response.data.errors)
    const x = document.getElementsByName(Object.keys(ex.response.data.errors)[0])[0]
    if (x) x.focus()
  }
}
</script>
