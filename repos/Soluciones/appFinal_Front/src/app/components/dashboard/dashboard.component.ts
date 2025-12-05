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

  // Para el formulario
  mostrandoFormulario = signal(false);
  editandoProducto = signal(false);
  productoForm: any = {
    productoId: null,
    nombre: '',
    mensaje: '',
    descripcion: '',
    precio: 0,
    stock: 0,
    categoriaId: '',
    imagenUrl: '',
    esOferta: false,
    estaActivo: true
  };

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

  mostrarFormulario(): void {
    this.resetFormulario();
    this.editandoProducto.set(false);
    this.mostrandoFormulario.set(true);
  }

  cancelarFormulario(): void {
    this.mostrandoFormulario.set(false);
    this.resetFormulario();
  }

  resetFormulario(): void {
    this.productoForm = {
      productoId: null,
      nombre: '',
      mensaje: '',
      descripcion: '',
      precio: 0,
      stock: 0,
      categoriaId: '',
      imagenUrl: '',
      esOferta: false,
      estaActivo: true
    };
  }

  editarProducto(producto: Producto): void {
    this.productoForm = {
      productoId: producto.productoId,
      nombre: producto.nombre,
      mensaje: producto.mensaje,
      descripcion: producto.descripcion,
      precio: producto.precio,
      stock: producto.stock,
      categoriaId: producto.categoriaId,
      imagenUrl: producto.imagenUrl,
      esOferta: producto.esOferta,
      estaActivo: producto.estaActivo
    };
    this.editandoProducto.set(true);
    this.mostrandoFormulario.set(true);
  }

  guardarProducto(event: Event): void {
    event.preventDefault();
    
    const productoData = {
      ...this.productoForm,
      categoriaId: Number(this.productoForm.categoriaId),
      precio: Number(this.productoForm.precio),
      stock: Number(this.productoForm.stock)
    };

    if (this.editandoProducto()) {
      this.productoService.updateProducto(productoData.productoId, productoData).subscribe({
        next: () => {
          alert('Producto actualizado exitosamente');
          this.cargarDatos();
          this.cancelarFormulario();
        },
        error: (error) => {
          console.error('Error actualizando producto:', error);
          alert('Error al actualizar el producto');
        }
      });
    } else {
      this.productoService.createProducto(productoData).subscribe({
        next: () => {
          alert('Producto creado exitosamente');
          this.cargarDatos();
          this.cancelarFormulario();
        },
        error: (error) => {
          console.error('Error creando producto:', error);
          alert('Error al crear el producto');
        }
      });
    }
  }

  eliminarProducto(productoId: number): void {
    if (confirm('¿Estás seguro de que deseas eliminar este producto?')) {
      this.productoService.deleteProducto(productoId).subscribe({
        next: () => {
          alert('Producto eliminado exitosamente');
          this.cargarDatos();
        },
        error: (error) => {
          console.error('Error eliminando producto:', error);
          alert('Error al eliminar el producto');
        }
      });
    }
  }
}
