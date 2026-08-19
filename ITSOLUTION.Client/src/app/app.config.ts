import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';
// 1. Importamos withInterceptors
import { provideHttpClient, withInterceptors } from '@angular/common/http'; 
// 2. Importamos el interceptor que creaste (ajusta la ruta si es necesario)
import { authInterceptor } from './core/services/auth.interceptor'; 

import { routes } from './app.routes';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    // 3. ¡AQUÍ ESTÁ LA MAGIA! Le inyectamos el interceptor a todas las peticiones
    provideHttpClient(withInterceptors([authInterceptor])) 
  ]
};