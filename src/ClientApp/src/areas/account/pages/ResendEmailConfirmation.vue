<template>
  <h1>Resend Email Confirmation</h1>
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
        <button type="submit" class="mt-4 btn">Resend</button>
      </Form>
    </div>
  </TwCard>
</template>

<script setup lang="ts">
import axios from 'axios'
import * as Yup from 'yup'
import { ref, reactive } from 'vue'
import { Field, Form, ErrorMessage } from 'vee-validate'
import type { SubmissionContext } from 'vee-validate'

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
    const response = await axios.post('/api/account/resendemailconfirmation', model)
    message.value = response.data.message
  } catch (ex) {
    error.value = ex.response.data.message
    actions.setErrors(ex.response.data.errors)
    const x = document.getElementsByName(Object.keys(ex.response.data.errors)[0])[0]
    if (x) x.focus()
  }
}
</script>
