// https://github.com/simplesmiler/vue-focus
// undefined allows for not includig ="true" everywhere
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
        if (binding.modifiers.lazy) {
          if (Boolean(binding.value) === Boolean(binding.oldValue)) {
            return
          }
        }

        if (binding.value || binding.value === undefined) el.focus()
        else el.blur()
      }
    })
  }
}
