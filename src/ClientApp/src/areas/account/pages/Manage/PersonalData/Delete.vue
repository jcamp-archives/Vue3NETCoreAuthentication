<template>
  <h2>Delete Personal Data</h2>
  <TwAlertSuccess v-if="message">{{ message }}</TwAlertSuccess>
  <TwAlertDanger v-if="error">{{ error }}</TwAlertDanger>

  <div class="alert alert-warning" role="alert">
    <p>
      <strong>Deleting this data will permanently remove your account, and this cannot be recovered.</strong>
    </p>
  </div>

  <div class="row">
    <div class="col-md-6">
      <Form v-slot="{ errors }" :validation-schema="Schema" @submit="onSubmit">
        <TwFormGroup label="Password">
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
        <button class="mt-4 bg-red-400 btn" type="submit">Delete data and close my account</button>
      </Form>
    </div>
  </div>
</template>

<script setup lang="ts">
import axios from 'axios'
import * as Yup from 'yup'
import { ref, reactive } from 'vue'
import { useRouter } from 'vue-router'
import { Field, Form, ErrorMessage } from 'vee-validate'
import type { SubmissionContext } from 'vee-validate'

const router = useRouter()
const message = ref('')
const error = ref('')
const model = reactive({ password: '' })

const Schema = Yup.object().shape({
  password: Yup.string().label('Password').required().min(8)
})

const onSubmit = async (values: any, actions: SubmissionContext) => {
  message.value = ''
  error.value = ''
  try {
    await axios.post('/api/account/manage/deletepersonaldata', model)
    router.push('/account/logout')
  } catch (ex) {
    error.value = ex.response.message
    actions.setErrors(ex.response.data.errors)
    const x = document.getElementsByName(Object.keys(ex.response.data.errors)[0])[0]
    if (x) x.focus()
  }
}
</script>
