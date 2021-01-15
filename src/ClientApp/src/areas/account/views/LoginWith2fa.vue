<template>
  <h1>Two-factor authentication</h1>

  <p>Your login is protected with an authenticator app. Enter your authenticator code below.</p>

  <div v-if="message" class="alert alert-success" role="alert">{{ message }}</div>
  <div v-if="error" class="alert alert-danger" role="alert">{{ error }}</div>

  <div class="card">
    <div class="card-body">
      <h5 class="card-title">Please enter your details</h5>
      <Form @submit="onSubmit" :validation-schema="Schema" v-slot="{ errors }">
        <div class="form-group">
          <label for="twoFactorCode">Verification Code</label>
          <Field
            v-model="model.twoFactorCode"
            name="twoFactorCode"
            type="text"
            v-focus
            class="form-control"
            :class="{ 'is-invalid': errors.twoFactorCode }"
          />
          <ErrorMessage class="invalid-feedback" name="twoFactorCode" />
        </div>
        <div class="form-check">
          <input
            type="checkbox"
            Id="rememberMachine"
            class="form-check-input"
            v-model="model.rememberMachine"
          />
          <label class="form-check-label" for="rememberMachine">Remember this machine</label>
        </div>

        <button type="submit" class="btn btn-primary mt-2">Submit</button>
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
import type { ILoginCommand, ILoginResult } from '../models'
import { Field, Form, ErrorMessage } from 'vee-validate'
import { ref, reactive, onBeforeMount } from 'vue'
import axios from 'axios'
import { useRouter, useRoute } from 'vue-router'
import * as Yup from 'yup'
import authStore from '@/store/authStore'

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
    var x = document.getElementsByName(Object.keys(ex.response.data.errors)[0])[0]
    if (x) x.focus()
  }
}
</script>
