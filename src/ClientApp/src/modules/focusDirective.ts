// https://github.com/simplesmiler/vue-focus
// undefined allows for not includig ="true"
import { UserModule } from '~/types'

export const install: UserModule = ({ app }) => {
  app.directive('focus', {
    mounted(el, binding) {
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
      // }
      if (binding.value || binding.value === undefined) el.focus()
      else el.blur()
    }
  })
}
