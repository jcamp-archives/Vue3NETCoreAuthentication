<template>
  <h1>Register</h1>

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
        <div class="form-group">
          <label for="confirmpassword">Confirm Password</label>
          <Field
            v-model="model.confirmPassword"
            name="confirmPassword"
            type="password"
            class="form-control"
            :class="{ 'is-invalid': errors.confirmPassword }"
          />
          <ErrorMessage class="invalid-feedback" name="confirmPassword" />
        </div>
        <button type="submit" class="btn btn-primary">Submit</button>
      </Form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { Field, Form, ErrorMessage } from 'vee-validate'
import type { SubmissionContext } from 'vee-validate'
import { ref, reactive } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { PasswordSchema } from '../models'
import axios from 'axios'
import * as Yup from 'yup'

const router = useRouter()
const route = useRoute()
const message = ref('')
const error = ref('')
const model = reactive({ email: '', password: '', confirmPassword: '', returnUrl: '' })

const Schema = PasswordSchema.shape({
  email: Yup.string().label('Email').required().email()
})

const onSubmit = async (values: any, actions: SubmissionContext) => {
  message.value = ''
  error.value = ''
  model.returnUrl = route.query.returnUrl as string
  try {
    await axios.post('/api/account/register', model)
    router.push('/account/registerconfirmation')
  } catch (ex) {
    error.value = ex.response.message
    actions.setErrors(ex.response.data.errors)
    var x = document.getElementsByName(Object.keys(ex.response.data.errors)[0])[0]
    if (x) x.focus()
  }
}
</script>
