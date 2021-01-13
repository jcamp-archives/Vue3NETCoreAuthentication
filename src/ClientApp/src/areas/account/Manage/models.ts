import { IBaseResult } from '../models'

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
