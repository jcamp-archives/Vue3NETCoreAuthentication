<template>
  <h2>Enable multi-factor authentication (MFA)</h2>

  <TwAlertSuccess v-if="message">{{ message }}</TwAlertSuccess>
  <TwAlertDanger v-if="error">{{ error }}</TwAlertDanger>

  <div v-if="model.qrCodeBase64">
    <p>To use an authenticator app go through the following steps:</p>
    <ol class="list">
      <li>
        <p>
          Download a two-factor authenticator app like Microsoft Authenticator for
          <a href="https://go.microsoft.com/fwlink/?Linkid=825072">Android</a> and
          <a href="https://go.microsoft.com/fwlink/?Linkid=825073">iOS</a> or Google Authenticator for
          <a href="https://play.google.com/store/apps/details?id=com.google.android.apps.authenticator2&amp;hl=en"
            >Android</a
          >
          and
          <a href="https://itunes.apple.com/us/app/google-authenticator/id388497605?mt=8">iOS</a>.
        </p>
      </li>
      <li>
        <p>
          Scan the QR Code or enter this key <kbd>{{ model.sharedKey }}</kbd> into your two factor authenticator app.
          Spaces and casing do not matter.
        </p>
        <img :src="'data:image/png;base64,' + model.qrCodeBase64" />
      </li>
      <li>
        <p>
          Once you have scanned the QR code or input the key above, your two factor authentication app will provide you
          with a unique code. Enter the code in the confirmation box below.
        </p>
        <div class="row">
          <div class="col-md-6">
            <Form v-slot="{ errors }" :validation-schema="Schema" @submit="onSubmit">
              <TwFormGroup label="Verification Code">
                <Field
                  v-model="verificationCode"
                  v-focus
                  name="verificationCode"
                  type="text"
                  class="block w-full mt-1"
                  :class="{ 'is-invalid': errors.verificationCode }"
                />
                <ErrorMessage class="invalid-feedback" name="verificationCode" />
              </TwFormGroup>

              <button type="submit" class="mt-4 btn">Verify</button>
            </Form>
          </div>
        </div>
      </li>
    </ol>
  </div>
</template>

<script setup lang="ts">
import { Field, Form, ErrorMessage } from 'vee-validate'
import { ref, reactive, onBeforeMount } from 'vue'
import axios from 'axios'
import * as Yup from 'yup'
import type { IMfaEnableResult } from '../../../models'

const message = ref('')
const error = ref('')
const model = reactive({} as IMfaEnableResult)
const verificationCode = ref('')

onBeforeMount(async () => {
  try {
    const response = await axios.get<IMfaEnableResult>('/api/account/manage/mfaenable')
    Object.assign(model, response.data)
  } catch (ex) {
    error.value = ex.message
  }
})

const Schema = Yup.object().shape({
  verificationCode: Yup.string().label('Verification Code').required().min(6).max(7)
})

const onSubmit = async (values: any, actions: any) => {
  message.value = ''
  error.value = ''
  try {
    const response = await axios.post('/api/account/manage/mfaenable', {
      verificationCode: verificationCode.value
    })
    message.value = response.data.message
  } catch (ex) {
    error.value = ex.response.data.message
    actions.setErrors(ex.response.data.errors)
    const x = document.getElementsByName(Object.keys(ex.response.data.errors)[0])[0]
    if (x) x.focus()
  }
}
</script>
