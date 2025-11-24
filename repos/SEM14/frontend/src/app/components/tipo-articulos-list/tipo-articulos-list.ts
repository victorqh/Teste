import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TipoArticulosService } from '../../services/tipo-articulos';
import { TipoArticulo } from '../../models/tipo-articulo.model';

@Component({
  selector: 'app-tipo-articulos-list',
  imports: [CommonModule],
  templateUrl: './tipo-articulos-list.html',
  styleUrl: './tipo-articulos-list.css',
})
export class TipoArticulosList implements OnInit {
  tipoArticulos: TipoArticulo[] = [];
  loading = false;
  error = '';

  constructor(private tipoArticulosService: TipoArticulosService) {}

  ngOnInit(): void {
    this.cargarTipos();
  }

  cargarTipos(): void {
    this.loading = true;
    this.tipoArticulosService.listar().subscribe({
      next: (data) => {
        this.tipoArticulos = data;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Error al cargar tipos de artículos';
        this.loading = false;
        console.error(err);
      }
    });
  }

  eliminar(id: number | undefined): void {
    if (!id) return;
    if (confirm('¿Está seguro de eliminar este tipo de artículo?')) {
      this.tipoArticulosService.eliminar(id).subscribe({
        next: () => this.cargarTipos(),
        error: (err) => {
          this.error = 'Error al eliminar tipo de artículo';
          console.error(err);
        }
      });
    }
  }
}
