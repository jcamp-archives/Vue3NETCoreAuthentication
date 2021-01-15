import * as Yup from 'yup'

export interface ILoginCommand {
  email: string
  password: string
  rememberMe: boolean
}
export interface ILoginResult extends IBaseResult {
  requiresTwoFactor: boolean
  isLockedOut: boolean
  requiresEmailConfirmation: boolean
  token: string
}
export interface IBaseResult {
  isSuccessful: boolean
  message: string
  errors: { [key: string]: string }
}
export interface IResetPasswordCommand {
  code: string
  email: string
  password: string
  confirmPassword: string
}

export const PasswordSchema = Yup.object().shape({
  email: Yup.string().label('Email').required().email(),
  password: Yup.string()
    .label('Password')
    .required()
    .min(8)
    .matches(
      /^.*(?=.{8,})((?=.*[!@#$%^&*()\-_=+{};:,<.>]){1})(?=.*\d)((?=.*[a-z]){1})((?=.*[A-Z]){1}).*$/,
      'Password must contain at least 8 characters, one uppercase, one number and one special case character'
    ),
  confirmPassword: Yup.string()
    .required('Please confirm your password')
    .when('password', {
      is: (password: any) => (password && password.length > 0 ? true : false),
      then: Yup.string().oneOf([Yup.ref('password')], "Password doesn't match")
    })
})

// manage models
export interface IUserProfileCommand {
  email: string
  phoneNumber: string
  isEmailConfirmed: boolean
}
export interface IMfaInfo extends IBaseResult {
  hasAuthenticator: boolean
  recoveryCodesLeft: number
  isMfaEnabled: boolean
  isMachineRemembered: boolean
}
export interface IMfaEnableResult extends IBaseResult {
  sharedKey: string
  authenticatorUri: string
  qrCodeBase64: string
}
