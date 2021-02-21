import authStore from '~/store/authStore'
import { UserModule } from '~/types'

export const install: UserModule = ({ isClient, router }) => {
  const token = localStorage.getItem('token')
  if (token) authStore.updateToken(token)

  router.beforeEach((to, from) => {
    // instead of having to check every route record with
    // to.matched.some(record => record.meta.requiresAuth)
    if (to.meta.auth) {
      if (!authStore.isAuthenticated) {
        // this route requires auth, check if logged in
        // if not, redirect to login page.
        return {
          path: '/account/login',
          // save the location we were at to come back later
          query: { returnUrl: to.fullPath }
        }
      }
      const role = to.meta.role as string
      if (role && !authStore.isUserInRole(role)) {
        return {
          path: '/account/nopermission'
        }
      }
      const roles = to.meta.roles as string[]
      if (roles && !authStore.isUserInAnyRole(roles)) {
        return {
          path: '/account/nopermission'
        }
      }
    }
  })
}
