import { Module, VuexModule, Mutation, Action, getModule } from 'vuex-module-decorators'
import jwt_decode from 'jwt-decode'
import axios from 'axios'
import store from './index'

export interface IAuthInfo {
  accessToken: string
  accessTokenExpiration: Date | null
  userName: string
  userRoles: string[]
}

@Module({
  name: 'auth',
  store,
  dynamic: true
})
class AuthStore extends VuexModule {
  authInfo: IAuthInfo | null = null

  get isAuthenticated(): boolean {
    if (this.authInfo !== null && this.authInfo.accessToken) {
      // check exp
      if (new Date() <= (this.authInfo.accessTokenExpiration as Date)) return true
    }

    return false
  }

  get userName(): string {
    return this.authInfo?.userName ?? ''
  }

  get isUserInRole() {
    return (role: string): boolean => {
      if (this.authInfo === null) return false
      role = role.toLowerCase()
      return !!this.authInfo.userRoles.find((r) => r.toLowerCase() === role)
    }
  }

  get isUserInAnyRole() {
    return (roles: string[]): boolean => {
      if (this.authInfo === null) return false
      return this.authInfo.userRoles.some((v) => roles.find((r) => r.toLowerCase() === v.toLowerCase()))
    }
  }

  @Mutation
  updateToken(token: string): void {
    const decode = jwt_decode<any>(token)
    let role = decode['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
    if (!Array.isArray(role)) {
      role = [role]
    }
    this.authInfo = {
      accessToken: token,
      userName: decode[`http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name`],
      userRoles: role,
      accessTokenExpiration: new Date(decode.exp * 1000)
    }
    axios.defaults.headers.common.Authorization = `Bearer ${token}`
  }

  @Mutation
  clearToken(): void {
    this.authInfo = null
  }

  @Action
  login(token: string): void {
    this.updateToken(token)
    localStorage.setItem('token', token)
  }

  @Action
  logout(): void {
    this.clearToken()
    delete axios.defaults.headers.common.Authorization
    localStorage.removeItem('token')
  }
}

export default getModule(AuthStore)
