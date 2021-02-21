/* eslint-disable import/no-duplicates */
import { defineComponent } from 'vue'

declare interface Window {
  // extend the window
}

declare module '/@vite-icons/oi/home' {
  const Component: ReturnType<typeof defineComponent>
  export default Component
}
