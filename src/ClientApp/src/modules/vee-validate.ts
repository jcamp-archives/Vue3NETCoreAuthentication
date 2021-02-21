import { configure } from 'vee-validate'
import { UserModule } from '~/types'

export const install: UserModule = ({ isClient }) => {
  if (isClient) {
    // Default values
    configure({
      validateOnBlur: false
    })
  }
}
