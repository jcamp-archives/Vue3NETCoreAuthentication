import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router'
import Home from '../views/Home.vue'
import Validation from '../views/Validation-Class.vue'
import AccountRoutes from '@/areas/account/routes'

const baseRoutes: Array<RouteRecordRaw> = [
  {
    path: '/',
    name: 'Home',
    component: Home
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

export default router
