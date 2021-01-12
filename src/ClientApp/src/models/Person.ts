import * as Yup from 'yup'

export interface Person {
  name: string
  age: number | null
  emailAddress: string
}

export const PersonSchema = Yup.object().shape({
  name: Yup.string().label('Name').required(),
  age: Yup.number().label('Age').required().positive().max(150),
  emailAddress: Yup.string().label('Email').required().email()
})
