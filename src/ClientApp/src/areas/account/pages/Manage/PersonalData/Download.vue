<template>
  <h2>Download Personal Data</h2>

  <TwCard>
    <textarea v-model="body" class="block w-full" style="height: 20rem"></textarea>
  </TwCard>
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
