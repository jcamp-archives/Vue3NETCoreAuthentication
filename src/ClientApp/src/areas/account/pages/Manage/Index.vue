<template>
  <h2>Profile</h2>
  <TwAlertSuccess v-if="message">{{ message }}</TwAlertSuccess>
  <TwAlertDanger v-if="error">{{ error }}</TwAlertDanger>

  <TwCard class="max-w-lg mt-8">
    <div class="grid grid-cols-1 gap-6">
      <Form v-slot="{ errors }" :validation-schema="Schema" @submit="onSubmit">
        <TwFormGroup label="User Name">
          <Field
            v-model="model.email"
            name="email"
            type="text"
            class="block w-full mt-1 bg-gray-200 border-gray-300"
            :class="{ 'is-invalid': errors.email }"
            disabled
          />
          <ErrorMessage class="invalid-feedback" name="email" />
        </TwFormGroup>
        <TwFormGroup label="Phone Number">
          <Field
            v-model="model.phoneNumber"
            name="phoneNumber"
            type="text"
            class="block w-full mt-1"
            :class="{ 'is-invalid': errors.phoneNumber }"
          />
          <ErrorMessage class="invalid-feedback" name="phoneNumber" />
        </TwFormGroup>
        <button type="submit" class="mt-4 btn">Save</button>
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
const message = ref('')
const error = ref('')
const model = reactive({} as IUserProfileCommand)

const Schema = Yup.object().shape({
  email: Yup.string().label('Email').required().email()
})

onBeforeMount(async () => {
  try {
    const response = await axios.get<IUserProfileCommand>('/api/account/manage/userprofile')
    Object.assign(model, response.data)
  } catch (ex) {
    router.push('/account/login')
  }
})

const onSubmit = async (values: any, actions: SubmissionContext) => {
  message.value = ''
  error.value = ''
  try {
    const response = await axios.post('/api/account/manage/userprofile', model)
    message.value = response.data.message
  } catch (ex) {
    error.value = ex.response.data.message
    actions.setErrors(ex.response.data.errors)
    const x = document.getElementsByName(Object.keys(ex.response.data.errors)[0])[0]
    if (x) x.focus()
  }
}
</script>
