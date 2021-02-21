<template>
  <div>
    <p class="my-3">
      Current count (Vuex):
      <strong>{{ currentCount }}</strong>
    </p>
    <button class="mr-2 btn btn-primary" @click.prevent="increment">Increment</button>
    <button class="btn btn-secondary" @click.prevent="reset">Reset</button>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import counterStore from '~/store/counterStore'

const currentCount = computed(() => counterStore.count)

const increment = (): void => {
  counterStore.incr()
}

const reset = (): void => {
  counterStore.reset()
}
</script>
