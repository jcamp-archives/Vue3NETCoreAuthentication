<template>
  <div>
    <h1>Weather forecast</h1>
    <p>This component demonstrates fetching data from the server.</p>

    <div v-if="loading">
      <p><em>Loading...</em></p>
    </div>
    <table v-else class="table">
      <thead>
        <tr>
          <th>Date</th>
          <th>Temp. (C)</th>
          <th>Temp. (F)</th>
          <th>Summary</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="(item, i) in forecasts" :key="i">
          <td>{{ date(item.date) }}</td>
          <td>
            <span :class="['p-2', 'rounded-pill', 'text-light', getColor(item.temperatureC)]">
              {{ item.temperatureC }}
            </span>
          </td>
          <td>{{ item.temperatureF }}</td>
          <td>{{ item.summary }}</td>
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
      <a href="http://localhost:8080/fetch-data">http://localhost:8080</a>) without devServer proxy
      settings in vue.config.js file.
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import axios from 'axios'
import { Forecast } from '../models/Forecast'
import { format } from 'date-fns'

const loading = ref(true)
const showError = ref(false)
const errorMessage = ref('')
const forecasts = ref([] as Forecast[])

const getColor = (temperature: number | undefined): string => {
  if (temperature! < 0) {
    return 'badge bg-primary'
  } else if (temperature! >= 0 && temperature! < 30) {
    return 'badge bg-success'
  } else {
    return 'badge bg-danger'
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
