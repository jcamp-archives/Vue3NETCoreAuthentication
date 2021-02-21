import store from '../store'
import { UserModule } from '~/types'

export const install: UserModule = ({ app }) => {
  app.use(store)
}
