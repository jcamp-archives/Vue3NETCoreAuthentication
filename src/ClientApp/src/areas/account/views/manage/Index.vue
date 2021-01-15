<template>
  <h1>Profile</h1>
  <div v-if="message" class="alert alert-success" role="alert">{{ message }}</div>
  <div v-if="error" class="alert alert-danger" role="alert">{{ error }}</div>

  <div class="card">
    <div class="card-body">
      <Form @submit="onSubmit" :validation-schema="Schema" v-slot="{ errors }">
        <div class="form-group">
          <label for="email">User Name</label>
          <Field
            v-model="model.email"
            name="email"
            type="text"
            class="form-control"
            :class="{ 'is-invalid': errors.email }"
            disabled
          />
          <ErrorMessage class="invalid-feedback" name="email" />
        </div>
        <div class="form-group">
          <label for="phoneNumber">Phone Number</label>
          <Field
            v-model="model.phoneNumber"
            name="phoneNumber"
            type="text"
            class="form-control"
            :class="{ 'is-invalid': errors.phoneNumber }"
          />
          <ErrorMessage class="invalid-feedback" name="phoneNumber" />
        </div>
        <button type="submit" class="btn btn-primary">Save</button>
      </Form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { Field, Form, ErrorMessage } from 'vee-validate'
import type { SubmissionContext } from 'vee-validate'
import { ref, reactive, onMounted, onBeforeMount } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import type { IUserProfileCommand } from '../../models'
import axios from 'axios'
import * as Yup from 'yup'

const router = useRouter()
const route = useRoute()
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
    var x = document.getElementsByName(Object.keys(ex.response.data.errors)[0])[0]
    if (x) x.focus()
  }
}
</script>
