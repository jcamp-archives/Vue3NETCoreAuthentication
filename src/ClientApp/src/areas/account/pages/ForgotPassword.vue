<template>
  <h1>Forgot Password</h1>

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
        <button type="submit" class="mt-4 btn">Submit</button>
      </Form>
    </div>
  </TwCard>
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
