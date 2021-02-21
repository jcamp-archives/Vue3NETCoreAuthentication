<template>
  <h1>Forgot Password</h1>

  <div class="card">
    <div class="card-body">
      <Form v-slot="{ errors }" :validation-schema="Schema" @submit="onSubmit">
        <div class="form-group">
          <label for="email">Email address</label>
          <Field
            v-model="model.email"
            v-focus
            name="email"
            type="text"
            class="form-control"
            :class="{ 'is-invalid': errors.email }"
          />
          <ErrorMessage class="invalid-feedback" name="email" />
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
import { useRouter } from 'vue-router'
import axios from 'axios'
import * as Yup from 'yup'

const router = useRouter()
const message = ref('')
const error = ref('')
const model = reactive({ email: '' })

const Schema = Yup.object().shape({
  email: Yup.string().label('Email').required().email()
})

const onSubmit = async (values: any, actions: SubmissionContext) => {
  message.value = ''
  error.value = ''
  try {
    await axios.post('/api/account/forgotpassword', model)
    router.push('/Account/ForgotPasswordConfirmation')
  } catch (ex) {
    error.value = ex.response.data.message
    actions.setErrors(ex.response.data.errors)
    const x = document.getElementsByName(Object.keys(ex.response.data.errors)[0])[0]
    if (x) x.focus()
  }
}
</script>
