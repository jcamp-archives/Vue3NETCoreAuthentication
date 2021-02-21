<template>
  <div>
    <h1 class="text-lg">Weather forecast</h1>
    <p class="my-3">This component demonstrates fetching data from the server.</p>

    <div v-if="loading">
      <p><em>Loading...</em></p>
    </div>
    <table v-else class="w-full">
      <thead>
        <tr class="text-left border-t border-b-2">
          <th class="w-1/4 p-3">Date</th>
          <th class="w-1/4 p-3">Temp. (C)</th>
          <th class="w-1/4 p-3">Temp. (F)</th>
          <th class="w-1/4 p-3">Summary</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="(item, i) in forecasts" :key="i" class="border-t">
          <td class="p-3">{{ date(item.date) }}</td>
          <td class="p-3">
            <span :class="['p-2', 'rounded-pill', 'text-white', getColor(item.temperatureC)]">
              {{ item.temperatureC }}
            </span>
          </td>
          <td class="p-3">{{ item.temperatureF }}</td>
          <td class="p-3">{{ item.summary }}</td>
        </tr>
      </tbody>
    </table>

    <div v-if="showError" class="alert alert-danger" role="alert">
      {{ errorMessage }}
    </div>

    <div v-if="showError" class="alert alert-warning" role="alert">
      Are you sure you're using ASP.NET Core endpoint? (default at
      <a href="http://localhost:5000/fetch-data">http://localhost:5000</a>)
      <br />
      API call would fail with status code 404 when calling from Vue app (default at
      <a href="http://localhost:8080/fetch-data">http://localhost:8080</a>) without devServer proxy settings in
      vue.config.js file.
    </div>
  </div>
</template>

<script setup lang="ts">
import { format } from 'date-fns'
import { ref } from 'vue'
import axios from 'axios'
import { Forecast } from '../models/Forecast'

const loading = ref(true)
const showError = ref(false)
const errorMessage = ref('')
const forecasts = ref([] as Forecast[])

const getColor = (temperature: number | undefined): string => {
  if (temperature! < 0) {
    return 'rounded bg-blue-500'
  } else if (temperature! >= 0 && temperature! < 20) {
    return 'rounded bg-green-600'
  } else {
    return 'rounded bg-red-600'
  }
}

const date = (date: Date | undefined): string => {
  return format(new Date(date!), 'eeee, dd MMMM')
}

const fetchWeatherForecasts = (): void => {
  try {
    axios.get<Forecast[]>('api/WeatherForecast').then((res) => {
      forecasts.value = res.data
      loading.value = false
    })
  } catch (e) {
    showError.value = true
    errorMessage.value = `Error while loading weather forecast: ${e.message}.`
  }
  loading.value = false
}

fetchWeatherForecasts()
</script>
