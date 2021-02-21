<template>
  <h1>Reset Password</h1>
  <TwAlertSuccess v-if="message">{{ message }}</TwAlertSuccess>
  <TwAlertDanger v-if="error">{{ error }}</TwAlertDanger>

  <TwCard title="Please enter your details" class="max-w-lg mt-8">
    <div class="grid grid-cols-1 gap-6">
      <Form v-slot="{ errors }" :validation-schema="PasswordSchema" @submit="onSubmit">
        <TwFormGroup label="New Password">
          <Field
            v-model="model.password"
            v-focus
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
