import { createApp } from 'vue'
import App from './App.vue'
import './registerServiceWorker'
import router from './router'
import store from './store'
import '@/css/main.scss'

// prettier-ignore
createApp(App)
    .use(store)
    .use(router)
    .mount('#app')
