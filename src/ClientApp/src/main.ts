import { createApp } from 'vue'
import App from './App.vue'
import './registerServiceWorker'
import router from './router'
import store from './store'
import '@/css/main.scss'

import focusDiretive from '@/plugins/focusDirective'
import { configure } from 'vee-validate'

// Default values
configure({
  validateOnBlur: false
})

// prettier-ignore
createApp(App)
    .use(focusDiretive)
    .use(store)
    .use(router)
    .mount('#app')
