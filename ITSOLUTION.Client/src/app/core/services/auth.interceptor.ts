import { HttpInterceptorFn } from '@angular/common/http';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  // 1. Buscamos el token que el AuthService guardó en el navegador
  const token = localStorage.getItem('jwt_token');

  // 2. Si existe un token, clonamos la petición original y le inyectamos la cabecera de seguridad
  if (token) {
    const authReq = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
    // Enviamos la petición modificada al backend
    return next(authReq);
  }

  // 3. Si no hay token (ej. está en la pantalla de login), la petición pasa tal cual
  return next(req);
};