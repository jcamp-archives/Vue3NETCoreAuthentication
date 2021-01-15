<script setup lang="ts">
import { useRouter, useRoute } from 'vue-router'
import authStore from '@/store/authStore'

const router = useRouter()
const route = useRoute()

authStore.logout()

const returnUrl = route.query.returnUrl as string
if (returnUrl) router.push(returnUrl)
else router.push('/')
</script>
