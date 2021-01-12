<template>
  <div>
    <h4>Validation Sample</h4>
    <hr class="mb-5" />
    <div v-if="message" class="alert alert-success" role="alert">{{ message }}</div>
    <VeeForm @submit="onSubmit" :validation-schema="PersonSchema" v-slot="{ errors }">
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
          v-model="model.EmailAddress"
          name="EmailAddress"
          type="email"
          class="form-control"
          :class="{ 'is-invalid': errors.EmailAddress }"
        />
        <ErrorMessage class="invalid-feedback" name="EmailAddress" />
      </P>

      <button type="submit" class="btn btn-primary mr-1">Save</button>
    </VeeForm>
  </div>
</template>

<script lang="ts">
import { Options, Vue } from 'vue-class-component'
import { Field, Form as VeeForm, ErrorMessage } from 'vee-validate'
import { Person, PersonSchema } from '@/models/Person'
import axios from 'axios'

export default class Validation extends Vue {
  message = ''
  model = { name: 'Isadora Jarr', age: 39, EmailAddress: 'im@nonymous.com' }

  onSubmit(values: any, actions: any) {
    axios
      .post('api/person', this.model)
      .then((response) => {
        this.message = response.data
      })
      .catch((ex) => {
        this.message = ex.response.data
        actions.setErrors(ex.response.data.errors)
      })
  }
}
</script>
