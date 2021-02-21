<template>
  <h1>Reset Password</h1>
  <TwAlertSuccess v-if="message">{{ message }}</TwAlertSuccess>
  <TwAlertDanger v-if="error">{{ error }}</TwAlertDanger>

  <div class="card">
    <div class="card-body">
      <h5 class="card-title">Please enter your details</h5>
      <Form v-slot="{ errors }" :validation-schema="PasswordSchema" @submit="onSubmit">
        <div class="form-group">
          <label for="password">New Password</label>
          <Field
            v-model="model.password"
            v-focus
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
import axios from 'axios'
import { ref, reactive, onMounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { Field, Form, ErrorMessage } from 'vee-validate'
import type { SubmissionContext } from 'vee-validate'
import type { IResetPasswordCommand } from '../models'
import { PasswordSchema } from '../models'

const router = useRouter()
const route = useRoute()
const message = ref('')
const error = ref('')
const model: IResetPasswordCommand = reactive({} as IResetPasswordCommand)

onMounted(() => {
  const email = route.query.email
  const code = route.query.code

  if (!email || !code) router.push('/')

  model.email = email as string
  model.code = code as string
})

const onSubmit = async (values: any, actions: SubmissionContext) => {
  message.value = ''
  error.value = ''
  try {
    await axios.post('/api/account/resetpassword', model)
    router.push('/Account/ResetPasswordConfirmation')
  } catch (ex) {
    error.value = ex.response.data.message
    actions.setErrors(ex.response.data.errors)
    const x = document.getElementsByName(Object.keys(ex.response.data.errors)[0])[0]
    if (x) x.focus()
  }
}
</script>
