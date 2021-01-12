<template>
  <div>
    <h4>Validation Sample</h4>
    <hr class="mb-5" />
    <div v-if="message" class="alert alert-success" role="alert">{{ message }}</div>
    <div v-if="error" class="alert alert-danger" role="alert">{{ error }}</div>
    <Form @submit="onSubmit" :validation-schema="PersonSchema" v-slot="{ errors }">
      <P>
        <label>Name: </label>
        <Field
          v-model="model.name"
          name="name"
          type="text"
          class="form-control"
          :class="{ 'is-invalid': errors.name }"
        />
        <ErrorMessage class="invalid-feedback" name="name" />
      </P>
      <P>
        <label>Age: </label>
        <Field
          v-model="model.age"
          name="age"
          type="number"
          class="form-control"
          :class="{ 'is-invalid': errors.age }"
        />
        <ErrorMessage class="invalid-feedback" name="age" />
      </P>
      <P>
        <label>Email: </label>
        <Field
          v-model="model.emailAddress"
          name="emailAddress"
          type="email"
          class="form-control"
          :class="{ 'is-invalid': errors.emailAddress }"
        />
        <ErrorMessage class="invalid-feedback" name="emailAddress" />
      </P>

      <button type="submit" class="btn btn-primary mr-1">Save</button>
    </Form>
  </div>
</template>

<script setup>
import { Field, Form, ErrorMessage } from 'vee-validate'
import { ref, reactive } from 'vue'
import { Person, PersonSchema } from '@/models/Person'
import axios from 'axios'

const message = ref('')
const error = ref('')
const model = reactive({ name: 'Isadora Jarr', age: 39, emailAddress: 'im@nonymous.com' })

const onSubmit = (values, actions) => {
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
      var x = document.getElementsByName(Object.keys(ex.response.data.errors)[0])[0]
      x.focus()
    })
}
</script>
