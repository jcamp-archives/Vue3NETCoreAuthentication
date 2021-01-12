export interface BaseResult {
  isSuccessful: boolean
  message: string
  errors: { [key: string]: string }
}
