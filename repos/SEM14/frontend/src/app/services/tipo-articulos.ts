import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TipoArticulo } from '../models/tipo-articulo.model';

@Injectable({
  providedIn: 'root',
})
export class TipoArticulosService {
  private apiUrl = 'http://localhost:8085/tipoarticulos';

  constructor(private http: HttpClient) {}

  listar(): Observable<TipoArticulo[]> {
    return this.http.get<TipoArticulo[]>(this.apiUrl);
  }

  obtener(id: number): Observable<TipoArticulo> {
    return this.http.get<TipoArticulo>(`${this.apiUrl}/${id}`);
  }

  crear(tipo: TipoArticulo): Observable<TipoArticulo> {
    return this.http.post<TipoArticulo>(this.apiUrl, tipo);
  }

  actualizar(id: number, tipo: TipoArticulo): Observable<TipoArticulo> {
    return this.http.put<TipoArticulo>(`${this.apiUrl}/${id}`, tipo);
  }

  eliminar(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
