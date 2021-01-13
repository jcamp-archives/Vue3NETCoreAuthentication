// https://github.com/simplesmiler/vue-focus
// undefined allows for not includig ="true"
import { App } from 'vue'

export default {
  install: (app: App) => {
    app.directive('focus', {
      mounted(el, binding) {
        console.log('binding: ' + binding.value)
        if (binding.value || binding.value === undefined) el.focus()
        else el.blur()
      },

      updated(el, binding) {
        // Had to default to only calling focus again if the value changed, otherwise constantly regrabbed the focus in vue 3
        // see docs for vue 1 vs 2 change in focus, its the same issue
        // if (binding.modifiers.lazy) {
        if (Boolean(binding.value) === Boolean(binding.oldValue)) {
          return
        }
        //}
        if (binding.value || binding.value === undefined) el.focus()
        else el.blur()
      }
    })
  }
}
