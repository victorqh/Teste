import { Component, inject, OnInit, signal } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { ProductoService } from '../../services/producto.service';
import { CategoriaService } from '../../services/categoria.service';
import { Producto, Categoria } from '../../models/user.model';

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule, FormsModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {
  private authService = inject(AuthService);
  private productoService = inject(ProductoService);
  private categoriaService = inject(CategoriaService);
  private router = inject(Router);
  
  productos = signal<Producto[]>([]);
  categorias = signal<Categoria[]>([]);
  loading = signal(false);
  errorMessage = signal('');
  currentUser = signal(this.authService.getCurrentUser());

  // Para el panel de estadísticas
  totalProductos = signal(0);
  productosActivos = signal(0);
  productosEnOferta = signal(0);
  productosBajoStock = signal(0);

  ngOnInit(): void {
    this.authService.loadCurrentUser();
    const user = this.currentUser();
    
    if (user?.rol !== 'admin') {
      alert('Acceso denegado. Solo administradores');
      this.router.navigate(['/']);
      return;
    }

    this.cargarDatos();
  }

  cargarDatos(): void {
    this.loading.set(true);
    this.errorMessage.set('');
    
    this.categoriaService.getAllCategorias().subscribe({
      next: (categorias) => this.categorias.set(categorias),
      error: (error) => console.error('Error cargando categorías:', error)
    });

    this.productoService.getAllProductos().subscribe({
      next: (productos) => {
        this.productos.set(productos);
        this.calcularEstadisticas(productos);
        this.loading.set(false);
      },
      error: (error) => {
        this.errorMessage.set('Error al cargar productos');
        this.loading.set(false);
        console.error('Error loading productos:', error);
      }
    });
  }

  calcularEstadisticas(productos: Producto[]): void {
    this.totalProductos.set(productos.length);
    this.productosActivos.set(productos.filter(p => p.estaActivo).length);
    this.productosEnOferta.set(productos.filter(p => p.esOferta).length);
    this.productosBajoStock.set(productos.filter(p => p.stock < 10).length);
  }

  getCategoriaName(categoriaId: number): string {
    return this.categorias().find(c => c.categoriaId === categoriaId)?.nombre || 'Sin categoría';
  }

  formatDate(dateString: string): string {
    const date = new Date(dateString);
    return date.toLocaleDateString('es-ES', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    });
  }
}
