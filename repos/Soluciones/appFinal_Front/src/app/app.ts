import { Component, signal, OnInit } from '@angular/core';
import { RouterOutlet, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from './services/auth.service';
import { CarritoService } from './services/carrito.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, RouterLink, CommonModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  protected readonly title = signal('TechShop');
  estaAutenticado = signal<boolean>(false);
  nombreUsuario = signal<string>('');
  cantidadCarrito = signal<number>(0);

  constructor(
    private authService: AuthService,
    private carritoService: CarritoService
  ) {}

  ngOnInit(): void {
    this.authService.currentUser$.subscribe(user => {
      this.estaAutenticado.set(!!user);
      this.nombreUsuario.set(user?.nombre || '');
      
      if (user) {
        this.carritoService.getCarrito().subscribe();
      }
    });

    this.carritoService.carrito$.subscribe(carrito => {
      this.cantidadCarrito.set(carrito?.cantidadItems || 0);
    });
  }

  logout(): void {
    this.authService.logout();
  }
}
