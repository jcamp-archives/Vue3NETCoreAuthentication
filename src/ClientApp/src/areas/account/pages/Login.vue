<template>
  <h1>Login</h1>

  <TwAlertSuccess v-if="message">{{ message }}</TwAlertSuccess>
  <TwAlertDanger v-if="error">{{ error }}</TwAlertDanger>

  <TwCard title="Please enter your details" class="max-w-lg mt-8">
    <div class="grid grid-cols-1 gap-6">
      <Form v-slot="{ errors }" :validation-schema="Schema" @submit="onSubmit">
        <TwFormGroup label="Email address">
          <Field
            v-model="model.email"
            v-focus
            name="email"
            type="text"
            class="block w-full mt-1"
            :class="{ 'is-invalid': errors.email }"
            :validate-on-blur="false"
          />
          <ErrorMessage class="invalid-feedback" name="email" />
        </TwFormGroup>
        <TwFormGroup label="Password">
          <Field
            v-model="model.password"
            name="password"
            type="password"
            class="block w-full mt-1"
            :class="{ 'is-invalid': errors.password }"
          />
          <ErrorMessage class="error-message" name="password" />
        </TwFormGroup>
        <button type="submit" class="mt-4 btn">Submit</button>
      </Form>
    </div>
  </TwCard>

  <div class="form-group">
    <p>
      <router-link to="/Account/ForgotPassword">Forgot your password?</router-link>
    </p>
    <p>
      <router-link to="/Account/Register">Register as a new user</router-link>
    </p>
    <p>
      <router-link to="/Account/ResendEmailConfirmation">Resend email confirmation</router-link>
    </p>
  </div>
</template>

<script setup lang="ts">
import { Field, Form, ErrorMessage } from 'vee-validate'
import { ref, reactive, onMounted } from 'vue'
import axios from 'axios'
import { useRouter, useRoute } from 'vue-router'
import * as Yup from 'yup'
import type { ILoginCommand, ILoginResult } from '../models'
import authStore from '~/store/authStore'

const router = useRouter()
const route = useRoute()
const message = ref('')
const error = ref('')
const returnUrl = ref('')
const model: ILoginCommand = reactive({} as ILoginCommand)

onMounted(() => {
  returnUrl.value = route.query.returnUrl as string
})

const Schema = Yup.object().shape({
  email: Yup.string().label('Email').required().email(),
  password: Yup.string().label('Password').required().min(8)
})

const onSubmit = async (values: any, actions: any) => {
  message.value = ''
  error.value = ''
  try {
    const response = await axios.post<ILoginResult>('/api/account/login', model)
    const result = response.data

    authStore.login(result.token)
    if (returnUrl.value) router.push(returnUrl.value)
    else router.push('/')
  } catch (ex) {
    const result = ex.response.data
    if (result.requiresTwoFactor) {
      router.push({ path: '/account/loginwith2fa', query: { returnUrl: returnUrl.value } })
    } else if (result.requiresEmailConfirmation) {
      router.push({
        path: '/account/ResendEmailConfirmation',
        query: { returnUrl: returnUrl.value }
      })
    } else if (result.isLockedOut) {
      router.push('/Account/Lockout')
    } else {
      error.value = ex.response.data.message
      actions.setErrors(ex.response.data.errors)
      const x = document.getElementsByName(Object.keys(ex.response.data.errors)[0])[0]
      if (x) x.focus()
    }
  }
}
</script>
