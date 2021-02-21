<template>
  <h1>Register</h1>

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
          <ErrorMessage class="invalid-feedback" name="password" />
        </TwFormGroup>
        <TwFormGroup label="Confirm Password">
          <Field
            v-model="model.confirmPassword"
            name="confirmPassword"
            type="password"
            class="block w-full mt-1"
            :class="{ 'is-invalid': errors.confirmPassword }"
          />
          <ErrorMessage class="invalid-feedback" name="confirmPassword" />
        </TwFormGroup>
        <button type="submit" class="mt-4 btn">Submit</button>
      </Form>
    </div>
  </TwCard>
</template>

<script setup lang="ts">
import axios from 'axios'
import * as Yup from 'yup'
import { ref, reactive } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { Field, Form, ErrorMessage } from 'vee-validate'
import type { SubmissionContext } from 'vee-validate'
import { PasswordSchema } from '../models'

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
    const x = document.getElementsByName(Object.keys(ex.response.data.errors)[0])[0]
    if (x) x.focus()
  }
}
</script>
