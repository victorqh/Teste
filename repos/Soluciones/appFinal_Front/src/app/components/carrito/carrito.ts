import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { CarritoService } from '../../services/carrito.service';
import { Carrito as CarritoModel } from '../../models/user.model';

@Component({
  selector: 'app-carrito',
  imports: [CommonModule, RouterLink],
  templateUrl: './carrito.html',
  styleUrl: './carrito.css',
})
export class Carrito implements OnInit {
  carrito = signal<CarritoModel | null>(null);
  cargando = signal<boolean>(true);

  constructor(
    private carritoService: CarritoService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.cargarCarrito();
  }

  cargarCarrito(): void {
    this.cargando.set(true);
    this.carritoService.getCarrito().subscribe({
      next: (carrito) => {
        this.carrito.set(carrito);
        this.cargando.set(false);
      },
      error: (error) => {
        console.error('Error cargando carrito:', error);
        this.cargando.set(false);
      }
    });
  }

  actualizarCantidad(carritoItemId: number, cantidad: number): void {
    if (cantidad < 1) return;
    
    this.carritoService.actualizarCantidad(carritoItemId, cantidad).subscribe({
      next: (carrito) => this.carrito.set(carrito),
      error: (error) => {
        console.error('Error actualizando cantidad:', error);
        alert('Error al actualizar cantidad');
      }
    });
  }

  eliminarItem(carritoItemId: number): void {
    if (confirm('¿Eliminar este producto del carrito?')) {
      this.carritoService.eliminarItem(carritoItemId).subscribe({
        next: (carrito) => this.carrito.set(carrito),
        error: (error) => {
          console.error('Error eliminando item:', error);
          alert('Error al eliminar producto');
        }
      });
    }
  }

  vaciarCarrito(): void {
    if (confirm('¿Estás seguro de vaciar todo el carrito?')) {
      this.carritoService.vaciarCarrito().subscribe({
        next: () => {
          this.carrito.set(null);
          alert('Carrito vaciado');
        },
        error: (error) => {
          console.error('Error vaciando carrito:', error);
          alert('Error al vaciar carrito');
        }
      });
    }
  }

  procesarCompra(): void {
    alert('Funcionalidad de compra en desarrollo');
  }
}
