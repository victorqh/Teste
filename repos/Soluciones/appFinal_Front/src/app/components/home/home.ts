import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ProductoService } from '../../services/producto.service';
import { CategoriaService } from '../../services/categoria.service';
import { Producto, Categoria } from '../../models/user.model';

@Component({
  selector: 'app-home',
  imports: [CommonModule, RouterLink],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home implements OnInit {
  productosDestacados = signal<Producto[]>([]);
  productosEnOferta = signal<Producto[]>([]);
  categorias = signal<Categoria[]>([]);
  cargando = signal<boolean>(true);

  constructor(
    private productoService: ProductoService,
    private categoriaService: CategoriaService
  ) {}

  ngOnInit(): void {
    this.cargarDatos();
  }

  cargarDatos(): void {
    this.cargando.set(true);

    this.productoService.getAllProductos().subscribe({
      next: (productos) => {
        this.productosDestacados.set(productos.slice(0, 8));
      },
      error: (error) => console.error('Error cargando productos:', error)
    });

    this.productoService.getProductosEnOferta().subscribe({
      next: (ofertas) => {
        this.productosEnOferta.set(ofertas.slice(0, 4));
      },
      error: (error) => console.error('Error cargando ofertas:', error)
    });

    this.categoriaService.getAllCategorias().subscribe({
      next: (categorias) => {
        this.categorias.set(categorias);
        this.cargando.set(false);
      },
      error: (error) => {
        console.error('Error cargando categorías:', error);
        this.cargando.set(false);
      }
    });
  }
}
