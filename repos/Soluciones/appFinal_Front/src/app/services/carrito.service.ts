import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { Carrito, AgregarAlCarritoRequest } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class CarritoService {
  private apiUrl = 'http://localhost:8080/api/carrito';
  private carritoSubject = new BehaviorSubject<Carrito | null>(null);
  public carrito$ = this.carritoSubject.asObservable();

  constructor(private http: HttpClient) { }

  getCarrito(): Observable<Carrito> {
    return this.http.get<Carrito>(this.apiUrl).pipe(
      tap(carrito => this.carritoSubject.next(carrito))
    );
  }

  agregarAlCarrito(request: AgregarAlCarritoRequest): Observable<Carrito> {
    return this.http.post<Carrito>(`${this.apiUrl}/agregar`, request).pipe(
      tap(carrito => this.carritoSubject.next(carrito))
    );
  }

  actualizarCantidad(carritoItemId: number, cantidad: number): Observable<Carrito> {
    return this.http.put<Carrito>(`${this.apiUrl}/item/${carritoItemId}`, { cantidad }).pipe(
      tap(carrito => this.carritoSubject.next(carrito))
    );
  }

  eliminarItem(carritoItemId: number): Observable<Carrito> {
    return this.http.delete<Carrito>(`${this.apiUrl}/item/${carritoItemId}`).pipe(
      tap(carrito => this.carritoSubject.next(carrito))
    );
  }

  vaciarCarrito(): Observable<void> {
    return this.http.delete<void>(this.apiUrl).pipe(
      tap(() => this.carritoSubject.next(null))
    );
  }

  getCantidadItems(): number {
    const carrito = this.carritoSubject.value;
    return carrito?.cantidadItems || 0;
  }
}
