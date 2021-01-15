<template>
  <h4>Delete Personal Data</h4>
  <div v-if="message" class="alert alert-success" role="alert">{{ message }}</div>
  <div v-if="error" class="alert alert-danger" role="alert">{{ error }}</div>

  <div class="alert alert-warning" role="alert">
    <p>
      <strong
        >Deleting this data will permanently remove your account, and this cannot be
        recovered.</strong
      >
    </p>
  </div>

  <div class="row">
    <div class="col-md-6">
      <Form @submit="onSubmit" :validation-schema="Schema" v-slot="{ errors }">
        <div class="form-group">
          <label for="password">Password</label>
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
        <button class="btn btn-danger" type="submit">Delete data and close my account</button>
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
    var x = document.getElementsByName(Object.keys(ex.response.data.errors)[0])[0]
    if (x) x.focus()
  }
}
</script>
