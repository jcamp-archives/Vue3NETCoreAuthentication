<template>
  <h1>Reset Password</h1>
  <div v-if="message" class="alert alert-success" role="alert">{{ message }}</div>
  <div v-if="error" class="alert alert-danger" role="alert">{{ error }}</div>

  <div class="card">
    <div class="card-body">
      <h5 class="card-title">Please enter your details</h5>
      <Form @submit="onSubmit" :validation-schema="PasswordSchema" v-slot="{ errors }">
        <div class="form-group">
          <label for="password">New Password</label>
          <Field
            v-model="model.password"
            name="password"
            type="password"
            v-focus
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
import { ref, reactive, onMounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import type { IResetPasswordCommand } from '../models'
import { PasswordSchema } from '../models'
import axios from 'axios'

const router = useRouter()
const route = useRoute()
const message = ref('')
const error = ref('')
const model: IResetPasswordCommand = reactive({} as IResetPasswordCommand)

onMounted(() => {
  let email = route.query.email
  let code = route.query.code

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
    var x = document.getElementsByName(Object.keys(ex.response.data.errors)[0])[0]
    if (x) x.focus()
  }
}
</script>
