import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { Home } from './components/home/home';
import { Productos } from './components/productos/productos';
import { ProductoDetalle } from './components/producto-detalle/producto-detalle';
import { Carrito } from './components/carrito/carrito';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: '', component: Home },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'productos', component: Productos },
  { path: 'productos/categoria/:categoriaId', component: Productos },
  { path: 'productos/:id', component: ProductoDetalle },
  { path: 'carrito', component: Carrito, canActivate: [authGuard] },
  { path: 'dashboard', component: DashboardComponent, canActivate: [authGuard] },
  { path: '**', redirectTo: '' }
];
