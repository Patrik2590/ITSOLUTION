import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
// Importamos nuestros servicios reales
import { AuthService } from '../../core/services/auth.service';
import { NotificationService } from '../../core/services/notification.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class LoginComponent {
  username: string = ''; // Actuará como el "Correo" para la API
  password: string = '';
  rememberMe: boolean = false;
  isPasswordFocused: boolean = false;
  showPassword: boolean = false;
  isDarkMode: boolean = true;
  
  // Nueva variable para bloquear el botón mientras carga
  isLoading: boolean = false; 

  // Inyectamos el AuthService y NotificationService
  constructor(
    private router: Router,
    private authService: AuthService,
    private notification: NotificationService
  ) {}

  togglePassword() {
    this.showPassword = !this.showPassword;
  }

  toggleDarkMode() {
    this.isDarkMode = !this.isDarkMode;
  }

  // Conexión real con la API en .NET
  // Conexión real con la API en .NET
  onLogin() {
    if (!this.username || !this.password) {
      this.notification.toast('warning', 'Ingrese su Admin ID y Security Key');
      return;
    }

    this.isLoading = true;
    
    // Llamamos a tu AuthController
    this.authService.login(this.username, this.password).subscribe({
      next: (res) => {
        // ✅ ÉXITO
        this.isLoading = false;
        this.notification.toast('success', `Acceso concedido, ${res.nombre}`);
        
        // Redirige al dashboard o al módulo de tickets
        this.router.navigate(['/tickets']); 
      },
      error: (err) => {
        // ❌ ERROR CAPTURADO (¡Evita que se quede pensando infinitamente!)
        this.isLoading = false; 
        
        // 🛡️ Filtramos el tipo de error usando TU servicio de notificaciones
        if (err.status === 0) {
          this.notification.toast('error', 'No hay conexión con el servidor. Verifica tu internet.');
        } else if (err.status === 401) {
          this.notification.toast('error', 'Correo o contraseña incorrectos.');
        } else if (err.status === 403) {
          this.notification.toast('warning', 'Tu cuenta no tiene permisos asignados. Contacta al administrador.');
        } else if (err.status === 500) {
          this.notification.toast('error', 'Error interno. Es posible que el usuario no tenga un rol válido en la BD.');
        } else {
          // Mensaje por defecto si el backend manda un texto específico
          const mensaje = err.error?.message || 'Ocurrió un error inesperado. Vuelve a intentar.';
          this.notification.toast('error', mensaje);
        }

        console.error('Error del sistema CORE:', err);
      }
    });
  }
}