import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ArticulosService } from '../../services/articulos';
import { Articulo } from '../../models/articulo.model';

@Component({
  selector: 'app-articulos-list',
  imports: [CommonModule],
  templateUrl: './articulos-list.html',
  styleUrl: './articulos-list.css',
})
export class ArticulosList implements OnInit {
  articulos: Articulo[] = [];
  loading = false;
  error = '';

  constructor(private articulosService: ArticulosService) {}

  ngOnInit(): void {
    this.cargarArticulos();
  }

  cargarArticulos(): void {
    this.loading = true;
    this.articulosService.listar().subscribe({
      next: (data) => {
        this.articulos = data;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Error al cargar artículos';
        this.loading = false;
        console.error(err);
      }
    });
  }

  eliminar(id: number | undefined): void {
    if (!id) return;
    if (confirm('¿Está seguro de eliminar este artículo?')) {
      this.articulosService.eliminar(id).subscribe({
        next: () => this.cargarArticulos(),
        error: (err) => {
          this.error = 'Error al eliminar artículo';
          console.error(err);
        }
      });
    }
  }
}
