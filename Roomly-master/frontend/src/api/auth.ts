import { AuthResponse, Login, Register } from "../types/Auth";
import { client } from "../utils/fetchClient";

export const login = (data: Login) => {
  return client.post<AuthResponse>("/login", data);
};

export const register = (data: Register) => {
  return client.post<AuthResponse>("/register", data);
};

export const logout = () => {
  return client.post("/logout");
};