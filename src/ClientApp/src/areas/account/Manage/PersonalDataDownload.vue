<template>
  <h4>Download Personal Data</h4>

  <div class="row">
    <div class="col-md-6">
      <textarea class="form-control" v-model="body" style="height: 20rem"></textarea>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onBeforeMount } from 'vue'
import { useRouter } from 'vue-router'
import axios from 'axios'
const body = ref('')
const router = useRouter()

onBeforeMount(async () => {
  try {
    const response = await axios.post('/api/account/manage/downloadpersonaldata', {})
    body.value = response.data.jsonData
  } catch (ex) {
    router.push('/account/login')
  }
})
</script>
