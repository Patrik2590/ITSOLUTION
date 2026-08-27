import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service'; // Asegúrate de que la ruta sea correcta

export const roleGuard: CanActivateFn = (route, state) => {
  // Inyectamos nuestros servicios
  const authService = inject(AuthService);
  const router = inject(Router);

  // Leemos qué roles exige esta ruta específica (lo configuraremos en el siguiente paso)
  const rolesRequeridos = route.data['roles'] as Array<string>;
  
  // Obtenemos los roles del usuario que está intentando entrar
  const rolesDelUsuario = authService.getRolesUsuario();

  // Si la ruta no exige ningún rol, lo dejamos pasar
  if (!rolesRequeridos || rolesRequeridos.length === 0) {
    return true;
  }

  // Verificamos si tiene permiso
  const tieneAcceso = rolesRequeridos.some(rol => rolesDelUsuario.includes(rol));

  if (tieneAcceso) {
    return true; // ¡Acceso concedido!
  } else {
    // 🚫 Acceso denegado: Lo pateamos de vuelta al dashboard (o a una página de "No Autorizado")
    router.navigate(['/dashboard']);
    return false;
  }
};