import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink, ActivatedRoute } from '@angular/router';
import { ProductoService } from '../../services/producto.service';
import { CategoriaService } from '../../services/categoria.service';
import { Producto, Categoria } from '../../models/user.model';

@Component({
  selector: 'app-productos',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './productos.html',
  styleUrl: './productos.css',
})
export class Productos implements OnInit {
  productos = signal<Producto[]>([]);
  productosFiltrados = signal<Producto[]>([]);
  categorias = signal<Categoria[]>([]);
  categoriaSeleccionada = signal<number | null>(null);
  busqueda = signal<string>('');
  cargando = signal<boolean>(true);

  constructor(
    private productoService: ProductoService,
    private categoriaService: CategoriaService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.cargarCategorias();
    
    this.route.params.subscribe(params => {
      const categoriaId = params['categoriaId'];
      if (categoriaId) {
        this.categoriaSeleccionada.set(+categoriaId);
        this.cargarProductosPorCategoria(+categoriaId);
      } else {
        this.cargarTodosProductos();
      }
    });
  }

  cargarCategorias(): void {
    this.categoriaService.getAllCategorias().subscribe({
      next: (categorias) => this.categorias.set(categorias),
      error: (error) => console.error('Error cargando categorías:', error)
    });
  }

  cargarTodosProductos(): void {
    this.cargando.set(true);
    this.productoService.getAllProductos().subscribe({
      next: (productos) => {
        this.productos.set(productos);
        this.productosFiltrados.set(productos);
        this.cargando.set(false);
      },
      error: (error) => {
        console.error('Error cargando productos:', error);
        this.cargando.set(false);
      }
    });
  }

  cargarProductosPorCategoria(categoriaId: number): void {
    this.cargando.set(true);
    this.productoService.getProductosByCategoria(categoriaId).subscribe({
      next: (productos) => {
        this.productos.set(productos);
        this.productosFiltrados.set(productos);
        this.cargando.set(false);
      },
      error: (error) => {
        console.error('Error cargando productos:', error);
        this.cargando.set(false);
      }
    });
  }

  filtrarPorCategoria(categoriaId: number | null): void {
    this.categoriaSeleccionada.set(categoriaId);
    if (categoriaId === null) {
      this.cargarTodosProductos();
    } else {
      this.cargarProductosPorCategoria(categoriaId);
    }
  }

  buscar(): void {
    const query = this.busqueda();
    if (query.trim() === '') {
      if (this.categoriaSeleccionada() === null) {
        this.cargarTodosProductos();
      } else {
        this.cargarProductosPorCategoria(this.categoriaSeleccionada()!);
      }
    } else {
      this.productoService.buscarProductos(query).subscribe({
        next: (productos) => this.productosFiltrados.set(productos),
        error: (error) => console.error('Error buscando productos:', error)
      });
    }
  }
}
