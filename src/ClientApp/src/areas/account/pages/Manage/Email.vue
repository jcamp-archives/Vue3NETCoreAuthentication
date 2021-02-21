<template>
  <h1>Manage Email</h1>
  <TwAlertSuccess v-if="message">{{ message }}</TwAlertSuccess>
  <TwAlertDanger v-if="error">{{ error }}</TwAlertDanger>

  <div class="card">
    <div class="card-body">
      <Form v-slot="{ errors }" :validation-schema="Schema" @submit="onSubmit">
        <div class="form-group">
          <label for="email">Email</label>

          <div v-if="isEmailConfirmed" class="input-group">
            <input v-model="emailAddress" type="text" name="email" class="form-control" disabled />
            <div class="input-group-append">
              <span class="input-group-text text-success font-weight-bold">✓</span>
            </div>
          </div>
          <div v-else>
            <input v-model="emailAddress" type="text" name="email" class="form-control" disabled />
            <button class="btn btn-link" @click.prevent="sendVerificationEmail">Send verification email</button>
          </div>
        </div>

        <div class="form-group">
          <label for="newEmail">New Email</label>
          <Field
            v-model="model.newEmail"
            name="newEmail"
            type="text"
            class="form-control"
            :class="{ 'is-invalid': errors.newEmail }"
          />
          <ErrorMessage class="invalid-feedback" name="newEmail" />
        </div>
        <button type="submit" class="btn btn-primary">Change email</button>
      </Form>
    </div>
  </div>
</template>

<script setup lang="ts">
import axios from 'axios'
import * as Yup from 'yup'
import { Field, Form, ErrorMessage } from 'vee-validate'
import type { SubmissionContext } from 'vee-validate'
import { ref, reactive, onBeforeMount } from 'vue'
import { useRouter } from 'vue-router'
import type { IUserProfileCommand } from '../../models'

const router = useRouter()
const isEmailConfirmed = ref(false)
const emailAddress = ref('')
const message = ref('')
const error = ref('')
const model = reactive({ newEmail: '' })

const Schema = Yup.object().shape({
  newEmail: Yup.string().label('Email').required().email()
})

onBeforeMount(async () => {
  try {
    const response = await axios.get<IUserProfileCommand>('/api/account/manage/userprofile')
    emailAddress.value = response.data.email
    isEmailConfirmed.value = response.data.isEmailConfirmed
  } catch (ex) {
    router.push('/account/login')
  }
})

const onSubmit = async (values: any, actions: SubmissionContext) => {
  message.value = ''
  error.value = ''
  try {
    const response = await axios.post('/api/account/manage/changeemail', model)
    message.value = response.data.message
  } catch (ex) {
    error.value = ex.response.data.message
    actions.setErrors(ex.response.data.errors)
    const x = document.getElementsByName(Object.keys(ex.response.data.errors)[0])[0]
    if (x) x.focus()
  }
}

const sendVerificationEmail = async () => {
  message.value = ''
  error.value = ''
  try {
    const response = await axios.post('/api/account/manage/SendEmailConfirmation')
    message.value = response.data.message
  } catch (ex) {
    error.value = ex.response.data.message
  }
}
</script>
