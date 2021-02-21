<template>
  <h1>Change Password</h1>
  <TwAlertSuccess v-if="message">{{ message }}</TwAlertSuccess>
  <TwAlertDanger v-if="error">{{ error }}</TwAlertDanger>

  <div class="card">
    <div class="card-body">
      <Form v-slot="{ errors }" :validation-schema="Schema" @submit="onSubmit">
        <div class="form-group">
          <label for="oldPassword">Old Password</label>
          <Field
            v-model="model.oldPassword"
            v-focus
            name="oldPassword"
            type="password"
            class="form-control"
            :class="{ 'is-invalid': errors.oldPassword }"
          />
          <ErrorMessage class="invalid-feedback" name="oldPassword" />
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

        <button type="submit" class="btn btn-primary">Update Password</button>
      </Form>
    </div>
  </div>
</template>

<script setup lang="ts">
import axios from 'axios'
import * as Yup from 'yup'
import { Field, Form, ErrorMessage } from 'vee-validate'
import type { SubmissionContext } from 'vee-validate'
import { ref, reactive } from 'vue'
import { PasswordSchema } from '../../models'

const message = ref('')
const error = ref('')
const model = reactive({ oldPassword: '', password: '', confirmPassword: '' })

const Schema = PasswordSchema.shape({
  oldPassword: Yup.string().label('Old Password').required().min(8)
})

const onSubmit = async (values: any, actions: SubmissionContext) => {
  message.value = ''
  error.value = ''
  try {
    const response = await axios.post('/api/account/manage/changepassword', model)
    message.value = response.data.message
  } catch (ex) {
    error.value = ex.response.data.message
    actions.setErrors(ex.response.data.errors)
    const x = document.getElementsByName(Object.keys(ex.response.data.errors)[0])[0]
    if (x) x.focus()
  }
}
</script>
