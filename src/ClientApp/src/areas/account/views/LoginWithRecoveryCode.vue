<template>
  <h1>Recovery code verification</h1>

  <p>
    You have requested to log in with a recovery code. This login will not be remembered until you
    provide an authenticator app code at log in or disable 2FA and log in again.
  </p>

  <div v-if="message" class="alert alert-success" role="alert">{{ message }}</div>
  <div v-if="error" class="alert alert-danger" role="alert">{{ error }}</div>

  <div class="card">
    <div class="card-body">
      <Form @submit="onSubmit" :validation-schema="Schema" v-slot="{ errors }">
        <div class="form-group">
          <label for="twoFactorCode">Recovery Code</label>
          <Field
            v-model="model.recoveryCode"
            name="recoveryCode"
            type="text"
            v-focus
            class="form-control"
            :class="{ 'is-invalid': errors.recoveryCode }"
          />
          <ErrorMessage class="invalid-feedback" name="recoveryCode" />
        </div>

        <button type="submit" class="btn btn-primary">Submit</button>
      </Form>
    </div>
  </div>
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
const model = reactive({ recoveryCode: '' })

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
    const response = await axios.post<ILoginResult>('/api/account/loginrecovery', model)
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
