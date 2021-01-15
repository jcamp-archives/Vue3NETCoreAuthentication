<template>
  <h1>Login</h1>

  <div v-if="message" class="alert alert-success" role="alert">{{ message }}</div>
  <div v-if="error" class="alert alert-danger" role="alert">{{ error }}</div>

  <div class="card">
    <div class="card-body">
      <h5 class="card-title">Please enter your details</h5>
      <Form @submit="onSubmit" :validation-schema="Schema" v-slot="{ errors }">
        <div class="form-group">
          <label for="email">Email address</label>
          <Field
            v-model="model.email"
            name="email"
            type="text"
            v-focus
            class="form-control"
            :class="{ 'is-invalid': errors.email }"
            :validateOnBlur="false"
          />
          <ErrorMessage class="invalid-feedback" name="email" />
        </div>
        <div class="form-group">
          <label for="password">Password</label>
          <Field
            v-model="model.password"
            name="password"
            type="password"
            class="form-control"
            :class="{ 'is-invalid': errors.password }"
          />
          <ErrorMessage class="invalid-feedback" name="password" />
        </div>
        <button type="submit" class="btn btn-primary">Submit</button>
      </Form>
    </div>
  </div>
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
import type { ILoginCommand, ILoginResult } from '../models'
import { Field, Form, ErrorMessage } from 'vee-validate'
import { ref, reactive, onMounted } from 'vue'
import axios from 'axios'
import { useRouter, useRoute } from 'vue-router'
import * as Yup from 'yup'
import authStore from '@/store/authStore'

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
      var x = document.getElementsByName(Object.keys(ex.response.data.errors)[0])[0]
      if (x) x.focus()
    }
  }
}
</script>
