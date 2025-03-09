export type User = {
  name: string;
  email: string;
}

export type AuthResponse = {
  token: string;
  user?: User;
}

export type Register = {
  name: string;
  email: string;
  password: string;
}

export type Login = {
  email: string;
  password: string;
}
