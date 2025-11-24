export interface User {
  userId: number;
  nombre: string;
  email: string;
  telefono?: string;
  direccion?: string;
  rol: string;
  fechaRegistro: string;
}

export interface CreateUserRequest {
  nombre: string;
  email: string;
  password: string;
  telefono?: string;
  direccion?: string;
  rol?: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  user: {
    userId: number;
    nombre: string;
    email: string;
    rol: string;
  };
}
