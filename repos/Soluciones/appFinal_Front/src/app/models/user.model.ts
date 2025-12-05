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

export interface Categoria {
  categoriaId: number;
  nombre: string;
  descripcion: string;
}

export interface Producto {
  productoId: number;
  nombre: string;
  descripcion: string;
  precio: number;
  precioAnterior?: number;
  stock: number;
  imagenUrl: string;
  categoriaId: number;
  categoriaNombre?: string;
  estaActivo: boolean;
  esOferta: boolean;
  fechaCreacion: string;
  mensaje?: string; //objeto mensaje
}

export interface CarritoItem {
  carritoItemId: number;
  productoId: number;
  productoNombre: string;
  productoImagen: string;
  precioUnitario: number;
  cantidad: number;
  subtotal: number;
}

export interface Carrito {
  carritoId: number;
  userId: number;
  fechaCreacion: string;
  items: CarritoItem[];
  total: number;
  cantidadItems: number;
}

export interface AgregarAlCarritoRequest {
  productoId: number;
  cantidad: number;
}
