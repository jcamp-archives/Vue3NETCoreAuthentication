<template>
  <h1>Two-factor authentication</h1>

  <p>Your login is protected with an authenticator app. Enter your authenticator code below.</p>

  <TwAlertSuccess v-if="message">{{ message }}</TwAlertSuccess>
  <TwAlertDanger v-if="error">{{ error }}</TwAlertDanger>

  <TwCard title="Please enter your details" class="max-w-lg mt-8">
    <div class="grid grid-cols-1 gap-6">
      <Form v-slot="{ errors }" :validation-schema="Schema" @submit="onSubmit">
        <TwFormGroup label="Verification Code">
          <Field
            v-model="model.twoFactorCode"
            v-focus
            name="twoFactorCode"
            type="text"
            class="block w-full mt-1"
            :class="{ 'is-invalid': errors.twoFactorCode }"
          />
          <ErrorMessage class="invalid-feedback" name="twoFactorCode" />
        </TwFormGroup>
        <div class="block">
          <div class="mt-2">
            <div>
              <label class="inline-flex items-center">
                <input id="rememberMachine" v-model="model.rememberMachine" type="checkbox" />
                <span class="ml-2">Remember this machine</span>
              </label>
            </div>
          </div>
        </div>

        <button type="submit" class="mt-4 btn">Submit</button>
      </Form>
    </div>
  </TwCard>
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
