import { Component, inject, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';
import { LoginRequest } from '../../models/user.model';

@Component({
  selector: 'app-login',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  private authService = inject(AuthService);
  private router = inject(Router);
  
  credentials: LoginRequest = {
    email: '',
    password: ''
  };
  
  loading = signal(false);
  errorMessage = signal('');

  onSubmit(): void {
    this.loading.set(true);
    this.errorMessage.set('');
    
    this.authService.login(this.credentials).subscribe({
      next: (response) => {
        this.loading.set(false);
        this.router.navigate(['/dashboard']);
      },
      error: (error) => {
        this.loading.set(false);
        this.errorMessage.set('Credenciales inv�lidas. Por favor, intenta de nuevo.');
        console.error('Login error:', error);
      }
    });
  }
}
