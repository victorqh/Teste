import { Component, inject, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { UserService } from '../../services/user.service';
import { CreateUserRequest } from '../../models/user.model';

@Component({
  selector: 'app-register',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  private userService = inject(UserService);
  private router = inject(Router);
  
  user: CreateUserRequest = {
    nombre: '',
    email: '',
    password: '',
    telefono: '',
    direccion: '',
    rol: 'Cliente'
  };
  
  loading = signal(false);
  errorMessage = signal('');
  successMessage = signal('');

  onSubmit(): void {
    this.loading.set(true);
    this.errorMessage.set('');
    this.successMessage.set('');
    
    this.userService.register(this.user).subscribe({
      next: (response) => {
        this.loading.set(false);
        this.successMessage.set('�Registro exitoso! Redirigiendo al login...');
        setTimeout(() => {
          this.router.navigate(['/login']);
        }, 2000);
      },
      error: (error) => {
        this.loading.set(false);
        this.errorMessage.set('Error al registrar. El email puede estar en uso.');
        console.error('Register error:', error);
      }
    });
  }
}
