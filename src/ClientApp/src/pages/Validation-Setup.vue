<template>
  <div>
    <h4>Validation Sample</h4>
    <hr class="mb-5" />
    <div v-if="message" class="alert alert-success" role="alert">{{ message }}</div>
    <div v-if="error" class="alert alert-danger" role="alert">{{ error }}</div>
    <Form v-slot="{ errors }" :validation-schema="PersonSchema" @submit="onSubmit">
      <p>
        <label>Name: </label>
        <Field
          v-model="model.name"
          name="name"
          type="text"
          class="form-control"
          :class="{ 'is-invalid': errors.name }"
        />
        <ErrorMessage class="invalid-feedback" name="name" />
      </p>
      <p>
        <label>Age: </label>
        <Field
          v-model="model.age"
          name="age"
          type="number"
          class="form-control"
          :class="{ 'is-invalid': errors.age }"
        />
        <ErrorMessage class="invalid-feedback" name="age" />
      </p>
      <p>
        <label>Email: </label>
        <Field
          v-model="model.emailAddress"
          name="emailAddress"
          type="email"
          class="form-control"
          :class="{ 'is-invalid': errors.emailAddress }"
        />
        <ErrorMessage class="invalid-feedback" name="emailAddress" />
      </p>

      <button type="submit" class="mr-1 btn btn-primary">Save</button>
    </Form>
  </div>
</template>

<script setup lang="ts">
import axios from 'axios'
import { Field, Form, ErrorMessage } from 'vee-validate'
import type { SubmissionContext } from 'vee-validate'
import { ref, reactive } from 'vue'
import { PersonSchema } from '~/models/Person'
import type { Person } from '~/models/Person'

const message = ref('')
const error = ref('')
const model = reactive({ name: 'Isadora Jarr', age: 39, emailAddress: 'im@nonymous.com' } as Person)

const onSubmit = (values: any, actions: SubmissionContext) => {
  message.value = ''
  error.value = ''
  axios
    .post('api/personmediatr', model)
    .then((response) => {
      message.value = response.data
    })
    .catch((ex) => {
      error.value = ex.response.message

      actions.setErrors(ex.response.data.errors)
      const x = document.getElementsByName(Object.keys(ex.response.data.errors)[0])[0]
      x.focus()
    })
}
</script>

<style scoped>
.is-invalid {
  @apply border-red-300;
}
</style>
