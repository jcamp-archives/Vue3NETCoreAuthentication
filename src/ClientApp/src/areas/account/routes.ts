import { RouteRecordRaw } from 'vue-router'
import { h } from 'vue'

const routes: Array<RouteRecordRaw> = [
  {
    name: 'account/login',
    path: '/account/login',
    component: () => import('./views/Login.vue')
  },
  {
    name: 'account/loginwith2fa',
    path: '/account/loginwith2fa',
    component: () => import('./views/LoginWith2fa.vue')
  },
  {
    name: 'account/loginwithrecoverycode',
    path: '/account/loginwithrecoverycode',
    component: () => import('./views/LoginWithRecoveryCode.vue')
  },
  {
    name: 'account/logout',
    path: '/account/logout',
    component: () => import('./views/Logout.vue')
  },
  {
    name: 'account/lockout',
    path: '/account/lockout',
    component: () => import('./views/Lockout.vue')
  },
  {
    name: 'account/register',
    path: '/account/register',
    component: () => import('./views/Register.vue')
  },
  {
    name: 'account/registerconfirmation',
    path: '/account/registerconfirmation',
    component: () => import('./views/RegisterConfirmation.vue')
  },
  {
    name: 'account/confirmemail',
    path: '/account/confirmemail',
    component: () => import('./views/ConfirmEmail.vue')
  },
  {
    name: 'account/confirmemailchange',
    path: '/account/confirmemailchange',
    component: () => import('./views/ConfirmEmailChange.vue')
  },
  {
    name: 'account/resetpassword',
    path: '/account/resetpassword',
    component: () => import('./views/ResetPassword.vue')
  },
  {
    name: 'account/resetpasswordconfirmation',
    path: '/account/resetpasswordconfirmation',
    component: () => import('./views/ResetPasswordConfirmation.vue')
  },
  {
    name: 'account/resendemailconfirmation',
    path: '/account/resendemailconfirmation',
    component: () => import('./views/ResendEmailConfirmation.vue')
  },
  {
    name: 'account/forgotpassword',
    path: '/account/forgotpassword',
    component: () => import('./views/ForgotPassword.vue')
  },
  {
    name: 'account/forgotpasswordconfirmation',
    path: '/account/forgotpasswordconfirmation',
    component: () => import('./views/ForgotPasswordConfirmation.vue')
  },
  {
    path: '/account/manage',
    component: () => import('./views/manage/ManageLayout.vue'),
    meta: { auth: true },
    children: [
      { path: '', component: () => import('./views/manage/Index.vue') },
      { path: 'email', component: () => import('./views/manage/Email.vue') },
      { path: 'changePassword', component: () => import('./views/manage/ChangePassword.vue') },
      {
        path: 'personalData',
        component: () => import('@/components/PassThrough.vue'),
        children: [
          { path: '', component: () => import('./views/manage/PersonalData.vue') },
          { path: 'download', component: () => import('./views/manage/PersonalDataDownload.vue') },
          { path: 'delete', component: () => import('./views/manage/PersonalDataDelete.vue') }
        ]
      },
      {
        path: 'twofactor',
        component: () => import('@/components/PassThrough.vue'),
        children: [
          { path: '', component: () => import('./views/manage/TwoFactorAuthentication.vue') },
          {
            path: 'recoverycodes',
            component: () => import('./views/manage/GenerateRecoveryCodes.vue')
          },
          { path: 'enable', component: () => import('./views/manage/EnableAuthenticator.vue') },
          { path: 'disable', component: () => import('./views/manage/DisableMfa.vue') },
          { path: 'reset', component: () => import('./views/manage/ResetAuthenticator.vue') }
        ]
      }
    ]
  }
]

export default routes
