import { Routes } from '@angular/router';

// Aquí debes asegurarte de importar correctamente tus componentes según tu estructura
import { LoginComponent } from './auth/login/login';
import { LayoutComponent } from './layout/layout';
import { DashboardComponent } from './features/dashboard/dashboard'; 
import { TicketsComponent } from './features/tickets/tickets'; // <-- 1. Importamos la nueva pantalla

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
      { path: 'dashboard', component: DashboardComponent },
      { path: 'tickets', component: TicketsComponent }, // <-- 2. Registramos la ruta
      // Aquí iremos agregando: 'inventory', 'users', etc.
    ]
  },
  
  // Ruta comodín para errores 404 (Opcional, redirige al dashboard si está logueado o al login)
  { path: '**', redirectTo: 'login' }
];