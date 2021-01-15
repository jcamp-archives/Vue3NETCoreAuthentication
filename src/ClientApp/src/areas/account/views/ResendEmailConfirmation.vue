<template>
  <h1>Resend Email Confirmation</h1>

  <div class="card">
    <div class="card-body">
      <Form @submit="onSubmit" :validation-schema="Schema" v-slot="{ errors }">
        <div class="form-group">
          <label for="email">Email address</label>
          <Field
            v-model="model.email"
            name="email"
            type="text"
            v-focus
            class="form-control"
            :class="{ 'is-invalid': errors.email }"
          />
          <ErrorMessage class="invalid-feedback" name="email" />
        </div>
        <button type="submit" class="btn btn-primary">Resend</button>
      </Form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { Field, Form, ErrorMessage } from 'vee-validate'
import type { SubmissionContext } from 'vee-validate'
import { ref, reactive } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import axios from 'axios'
import * as Yup from 'yup'

const router = useRouter()
const route = useRoute()
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
    var x = document.getElementsByName(Object.keys(ex.response.data.errors)[0])[0]
    if (x) x.focus()
  }
}
</script>
