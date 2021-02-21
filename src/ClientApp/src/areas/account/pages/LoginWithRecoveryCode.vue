<template>
  <h1>Recovery code verification</h1>

  <p>
    You have requested to log in with a recovery code. This login will not be remembered until you provide an
    authenticator app code at log in or disable 2FA and log in again.
  </p>

  <TwAlertSuccess v-if="message">{{ message }}</TwAlertSuccess>
  <TwAlertDanger v-if="error">{{ error }}</TwAlertDanger>

  <TwCard title="Please enter your details" class="max-w-lg mt-8">
    <div class="grid grid-cols-1 gap-6">
      <Form v-slot="{ errors }" :validation-schema="Schema" @submit="onSubmit">
        <TwFormGroup label="Recovery Code">
          <Field
            v-model="model.recoveryCode"
            v-focus
            name="recoveryCode"
            type="text"
            class="block w-full mt-1"
            :class="{ 'is-invalid': errors.recoveryCode }"
          />
          <ErrorMessage class="invalid-feedback" name="recoveryCode" />
        </TwFormGroup>
        <button type="submit" class="mt-4 btn">Submit</button>
      </Form>
    </div>
  </TwCard>
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
    const x = document.getElementsByName(Object.keys(ex.response.data.errors)[0])[0]
    if (x) x.focus()
  }
}
</script>
