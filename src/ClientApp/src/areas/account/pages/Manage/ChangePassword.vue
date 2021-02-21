<template>
  <h2>Change Password</h2>
  <TwAlertSuccess v-if="message">{{ message }}</TwAlertSuccess>
  <TwAlertDanger v-if="error">{{ error }}</TwAlertDanger>

  <TwCard class="max-w-lg mt-8">
    <div class="grid grid-cols-1 gap-6">
      <Form v-slot="{ errors }" :validation-schema="Schema" @submit="onSubmit">
        <TwFormGroup label="Old Password">
          <Field
            v-model="model.oldPassword"
            v-focus
            name="oldPassword"
            type="password"
            class="block w-full mt-1"
            :class="{ 'is-invalid': errors.oldPassword }"
          />
          <ErrorMessage class="invalid-feedback" name="oldPassword" />
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

        <button type="submit" class="mt-4 btn">Update Password</button>
      </Form>
    </div>
  </TwCard>
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
