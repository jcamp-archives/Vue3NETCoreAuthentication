<script setup lang="ts">
import { useRouter, useRoute } from 'vue-router'
import authStore from '@/store/authStore'

const router = useRouter()

authStore.logout()
router.push('/')
</script>
