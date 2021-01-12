<template>
  <div>
    <p>
      Current count (Vuex):
      <strong>{{ currentCount }}</strong>
    </p>
    <button class="btn btn-primary mr-2" @click.prevent="increment">Increment</button>
    <button class="btn btn-secondary" @click.prevent="reset">Reset</button>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import counterStore from '@/store/counterStore'

const currentCount = computed(() => counterStore.count)

const increment = (): void => {
  counterStore.incr()
}

const reset = (): void => {
  counterStore.reset()
}
</script>
