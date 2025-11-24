import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductoService } from '../../services/producto.service';
import { CarritoService } from '../../services/carrito.service';
import { AuthService } from '../../services/auth.service';
import { Producto } from '../../models/user.model';

@Component({
  selector: 'app-producto-detalle',
  imports: [CommonModule],
  templateUrl: './producto-detalle.html',
  styleUrl: './producto-detalle.css',
})
export class ProductoDetalle implements OnInit {
  producto = signal<Producto | null>(null);
  cantidad = signal<number>(1);
  cargando = signal<boolean>(true);
  agregandoCarrito = signal<boolean>(false);
  estaAutenticado = signal<boolean>(false);

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private productoService: ProductoService,
    private carritoService: CarritoService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.estaAutenticado.set(this.authService.isAuthenticated());
    
    this.route.params.subscribe(params => {
      const productoId = +params['id'];
      this.cargarProducto(productoId);
    });
  }

  cargarProducto(id: number): void {
    this.cargando.set(true);
    this.productoService.getProductoById(id).subscribe({
      next: (producto) => {
        this.producto.set(producto);
        this.cargando.set(false);
      },
      error: (error) => {
        console.error('Error cargando producto:', error);
        this.cargando.set(false);
      }
    });
  }

  aumentarCantidad(): void {
    const prod = this.producto();
    if (prod && this.cantidad() < prod.stock) {
      this.cantidad.update(c => c + 1);
    }
  }

  disminuirCantidad(): void {
    if (this.cantidad() > 1) {
      this.cantidad.update(c => c - 1);
    }
  }

  agregarAlCarrito(): void {
    const prod = this.producto();
    if (!prod) return;

    if (!this.estaAutenticado()) {
      alert('Debes iniciar sesión para agregar productos al carrito');
      this.router.navigate(['/login']);
      return;
    }

    if (prod.stock === 0) {
      alert('Producto sin stock');
      return;
    }

    this.agregandoCarrito.set(true);
    this.carritoService.agregarAlCarrito({
      productoId: prod.productoId,
      cantidad: this.cantidad()
    }).subscribe({
      next: () => {
        this.agregandoCarrito.set(false);
        alert('Producto agregado al carrito');
        this.cantidad.set(1);
      },
      error: (error) => {
        console.error('Error agregando al carrito:', error);
        this.agregandoCarrito.set(false);
        alert('Error al agregar al carrito');
      }
    });
  }

  volver(): void {
    this.router.navigate(['/productos']);
  }
}
