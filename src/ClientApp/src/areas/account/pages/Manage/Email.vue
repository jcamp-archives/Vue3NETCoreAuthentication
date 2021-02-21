<template>
  <h2>Manage Email</h2>
  <TwAlertSuccess v-if="message">{{ message }}</TwAlertSuccess>
  <TwAlertDanger v-if="error">{{ error }}</TwAlertDanger>

  <TwCard class="max-w-lg mt-8">
    <div class="grid grid-cols-1 gap-6">
      <Form v-slot="{ errors }" :validation-schema="Schema" @submit="onSubmit">
        <TwFormGroup label="Email">
          <div class="relative mt-1 rounded-md shadow-sm">
            <input v-model="emailAddress" disabled type="text" class="block w-full bg-gray-200 border-gray-300" />
            <div v-if="isEmailConfirmed" class="absolute inset-y-0 right-0 flex items-center pr-3 pointer-events-none">
              <oi-check class="w-5 h-5 text-green-600" />
            </div>
          </div>
          <div v-if="!isEmailConfirmed">
            <a class="block mt-1 mb-5 text-blue-500 cursor-pointer" @click.prevent="sendVerificationEmail"
              >Send verification email</a
            >
          </div>
        </TwFormGroup>

        <TwFormGroup label="New Email">
          <Field
            v-model="model.newEmail"
            name="newEmail"
            type="text"
            class="block w-full mt-1"
            :class="{ 'is-invalid': errors.newEmail }"
          />
          <ErrorMessage class="invalid-feedback" name="newEmail" />
        </TwFormGroup>
        <button type="submit" class="mt-4 btn">Change email</button>
      </Form>
    </div>
  </TwCard>
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
