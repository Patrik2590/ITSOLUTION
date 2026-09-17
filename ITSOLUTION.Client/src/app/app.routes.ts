import { Routes } from '@angular/router';

import { LoginComponent } from './auth/login/login';
import { LayoutComponent } from './layout/layout';
import { DashboardComponent } from './features/dashboard/dashboard'; 
import { TicketsComponent } from './features/tickets/tickets'; 

// 🛡️ 1. Importamos el Guardián de Roles
import { roleGuard } from './core/guards/role.guard';

export const routes: Routes = [
  // Ruta por defecto: Redirige al Login
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  
  // Ruta pública
  { path: 'login', component: LoginComponent },
  
  // Rutas Privadas (Protegidas dentro del Layout)
  {
    path: '',
    component: LayoutComponent,
    children: [
      { 
        path: 'dashboard', 
        component: DashboardComponent,
        // 🛡️ 2. Protegemos y definimos quién puede entrar
        canActivate: [roleGuard],
        data: { roles: ['Administrador TI', 'Técnico Soporte'] }
      },
      { 
        path: 'tickets', 
        component: TicketsComponent,
        // 🛡️ 3. Aplicamos lo mismo para tickets
        canActivate: [roleGuard],
        data: { roles: ['Administrador TI', 'Técnico Soporte'] }
      },
      { 
         path: 'inventarios', 
         loadComponent: () => import('./features/activos/activos.component').then(m => m.ActivosComponent) 
      },

      { path: 'empresas', loadComponent: () => import('./features/empresas/empresas.component').then(m => m.EmpresasComponent) }

      // 💡 Ejemplo de cómo quedará la ruta de usuarios cuando la crees:
      // { 
      //   path: 'users', 
      //   component: UsersComponent, // (Aún no importado)
      //   canActivate: [roleGuard],
      //   data: { roles: ['Administrador TI'] } // <-- ¡BLOQUEO ESTRICTO! Solo el Admin pasará.
      // }
    ]
  },
  
  // Ruta comodín para errores 404
  { path: '**', redirectTo: 'login' }
];