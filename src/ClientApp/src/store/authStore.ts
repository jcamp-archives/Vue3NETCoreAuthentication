import { Module, VuexModule, Mutation, Action, getModule } from 'vuex-module-decorators'
import store from './index'
import jwt_decode from 'jwt-decode'
import axios from 'axios'

export interface IAuthInfo {
  accessToken: string
  accessTokenExpiration: Date | null
  userName: string
  userRole: string
}

@Module({
  name: 'auth',
  store: store,
  dynamic: true
})
class AuthStore extends VuexModule {
  authInfo: IAuthInfo | null = null

  get isAuthenticated() {
    //TODO: check expiration
    const jwt = localStorage.getItem('token')
    if (jwt) {
      const decode = jwt_decode(jwt)
      axios.defaults.headers.common['Authorization'] = `Bearer ${jwt}`
    }
    return jwt
    //return this.authInfo !== null && this.authInfo.accessToken
  }

  @Mutation
  login(token: string): void {
    this.authInfo = { accessToken: token, userName: '', userRole: '', accessTokenExpiration: null }
    localStorage.setItem('token', token)
  }

  @Mutation
  logout(): void {
    this.authInfo = null
    delete axios.defaults.headers.common['Authorization']
    localStorage.removeItem('token')
  }
}

export default getModule(AuthStore)
