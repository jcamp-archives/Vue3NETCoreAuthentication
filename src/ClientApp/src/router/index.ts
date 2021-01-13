import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router'
import Home from '../views/Home.vue'
import NoPermission from '../views/NoPermission.vue'
import Admin from '../views/Admin.vue'
import Validation from '../views/Validation-Class.vue'
import AccountRoutes from '@/areas/account/routes'
import authStore from '@/store/authStore'
import axios from 'axios'

const token = localStorage.getItem('token')
if (token) authStore.updateToken(token)

const baseRoutes: Array<RouteRecordRaw> = [
  {
    path: '/',
    name: 'Home',
    component: Home
  },
  {
    path: '/NoPermission',
    name: 'NoPermission',
    component: NoPermission
  },
  {
    path: '/Admin',
    name: 'Admin',
    meta: { auth: true, role: 'admin' },
    component: Admin
  },
  {
    path: '/counter',
    name: 'counter',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "counter" */ '../views/Counter.vue')
  },
  {
    path: '/fetch-data',
    name: 'fetch-data',
    meta: { auth: true, roles: ['user', 'admin'] },
    component: () => import(/* webpackChunkName: "fetchdata" */ '../views/FetchData.vue')
  },
  {
    path: '/validation',
    name: 'validation',
    component: Validation
    //component: () => import(/* webpackChunkName: "counter" */ '../views/Validation-Setup.vue')
  },
  {
    path: '/about',
    name: 'About',
    component: () => import(/* webpackChunkName: "about" */ '../views/About.vue')
  }
]

const routes = baseRoutes.concat(AccountRoutes)

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

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
    const role = to.meta.role
    if (role && !authStore.isUserInRole(role)) {
      return {
        path: '/nopermission'
      }
    }
    const roles = to.meta.roles
    if (roles && !authStore.isUserInAnyRole(roles)) {
      return {
        path: '/nopermission'
      }
    }
  }
})

export default router
