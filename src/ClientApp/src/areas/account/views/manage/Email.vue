<template>
  <h1>Manage Email</h1>
  <div v-if="message" class="alert alert-success" role="alert">{{ message }}</div>
  <div v-if="error" class="alert alert-danger" role="alert">{{ error }}</div>

  <div class="card">
    <div class="card-body">
      <Form @submit="onSubmit" :validation-schema="Schema" v-slot="{ errors }">
        <div class="form-group">
          <label for="email">Email</label>

          <div class="input-group" v-if="isEmailConfirmed">
            <input type="text" name="email" class="form-control" v-model="emailAddress" disabled />
            <div class="input-group-append">
              <span class="input-group-text text-success font-weight-bold">✓</span>
            </div>
          </div>
          <div v-else>
            <input type="text" name="email" class="form-control" v-model="emailAddress" disabled />
            <button @click.prevent="sendVerificationEmail" class="btn btn-link">
              Send verification email
            </button>
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
import { Field, Form, ErrorMessage } from 'vee-validate'
import type { SubmissionContext } from 'vee-validate'
import { ref, reactive, onBeforeMount } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import type { IUserProfileCommand } from '../../models'
import axios from 'axios'
import * as Yup from 'yup'

const router = useRouter()
const route = useRoute()
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
    var x = document.getElementsByName(Object.keys(ex.response.data.errors)[0])[0]
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
