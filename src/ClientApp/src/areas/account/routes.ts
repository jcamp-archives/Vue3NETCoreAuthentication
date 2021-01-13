import { RouteRecordRaw } from 'vue-router'
import { h } from 'vue'

const routes: Array<RouteRecordRaw> = [
  {
    name: 'account/login',
    path: '/account/login',
    component: () => import('./Login.vue')
  },
  {
    name: 'account/loginwith2fa',
    path: '/account/loginwith2fa',
    component: () => import('./LoginWith2fa.vue')
  },
  {
    name: 'account/loginwithrecoverycode',
    path: '/account/loginwithrecoverycode',
    component: () => import('./LoginWithRecoveryCode.vue')
  },
  {
    name: 'account/logout',
    path: '/account/logout',
    component: () => import('./Logout.vue')
  },
  {
    name: 'account/lockout',
    path: '/account/lockout',
    component: () => import('./Lockout.vue')
  },
  {
    name: 'account/register',
    path: '/account/register',
    component: () => import('./Register.vue')
  },
  {
    name: 'account/registerconfirmation',
    path: '/account/registerconfirmation',
    component: () => import('./RegisterConfirmation.vue')
  },
  {
    name: 'account/confirmemail',
    path: '/account/confirmemail',
    component: () => import('./ConfirmEmail.vue')
  },
  {
    name: 'account/confirmemailchange',
    path: '/account/confirmemailchange',
    component: () => import('./ConfirmEmailChange.vue')
  },
  {
    name: 'account/resetpassword',
    path: '/account/resetpassword',
    component: () => import('./ResetPassword.vue')
  },
  {
    name: 'account/resetpasswordconfirmation',
    path: '/account/resetpasswordconfirmation',
    component: () => import('./ResetPasswordConfirmation.vue')
  },
  {
    name: 'account/resendemailconfirmation',
    path: '/account/resendemailconfirmation',
    component: () => import('./ResendEmailConfirmation.vue')
  },
  {
    name: 'account/forgotpassword',
    path: '/account/forgotpassword',
    component: () => import('./ForgotPassword.vue')
  },
  {
    name: 'account/forgotpasswordconfirmation',
    path: '/account/forgotpasswordconfirmation',
    component: () => import('./ForgotPasswordConfirmation.vue')
  },
  {
    path: '/account/manage',
    component: () => import('./Manage/ManageLayout.vue'),
    meta: { auth: true },
    children: [
      { path: '', component: () => import('./Manage/Index.vue') },
      { path: 'email', component: () => import('./Manage/Email.vue') },
      { path: 'changePassword', component: () => import('./Manage/ChangePassword.vue') },
      {
        path: 'personalData',
        component: () => import('@/components/PassThrough.vue'),
        children: [
          { path: '', component: () => import('./Manage/PersonalData.vue') },
          { path: 'download', component: () => import('./Manage/PersonalDataDownload.vue') },
          { path: 'delete', component: () => import('./Manage/PersonalDataDelete.vue') }
        ]
      },
      {
        path: 'twofactor',
        component: () => import('@/components/PassThrough.vue'),
        children: [
          { path: '', component: () => import('./Manage/TwoFactorAuthentication.vue') },
          { path: 'recoverycodes', component: () => import('./Manage/GenerateRecoveryCodes.vue') },
          { path: 'enable', component: () => import('./Manage/EnableAuthenticator.vue') },
          { path: 'disable', component: () => import('./Manage/DisableMfa.vue') },
          { path: 'reset', component: () => import('./Manage/ResetAuthenticator.vue') }
        ]
      }
    ]
  }
]

export default routes
