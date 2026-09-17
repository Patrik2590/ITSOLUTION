import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router'; // 👈 Importamos Router
import { Observable } from 'rxjs';
import { TabService } from '../core/services/tab.service';
import { AuthService } from '../core/services/auth.service'; // 👈 Importamos tu AuthService
import { Tab } from '../core/models/tab.model';
import { HasRoleDirective } from '../shared/directives/has-role';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [CommonModule, RouterModule, HasRoleDirective], 
  templateUrl: './layout.html',
  styleUrl: './layout.scss'
})
export class LayoutComponent implements OnInit {
  isDarkMode: boolean = true;
  isSidebarCollapsed: boolean = false;
  isProfileMenuOpen: boolean = false; // 👈 Controla si el menú flotante está abierto

  // Datos dinámicos del usuario
  userName: string = 'Usuario';
  userRole: string = 'Rol';
  userInitial: string = 'U';

  tabs$!: Observable<Tab[]>;

  // 🌟 Inyección de dependencias moderna
  private tabService = inject(TabService);
  private authService = inject(AuthService);
  private router = inject(Router);

  menuItems = [
    { section: 'Principal', items: [
      { id: '/dashboard', icon: 'pi-home', label: 'Dashboard', route: '/dashboard', roles: ['Administrador TI', 'Técnico Soporte'] }
    ]},
    { section: 'Gestión TI', items: [
      // Dentro de tu array menuItems, en la sección 'Gestión TI':
      { id: '/inventarios', icon: 'pi-box', label: 'Inventarios', route: '/inventarios', roles: ['Administrador TI', 'Técnico Soporte'] },
      { id: '/tickets', icon: 'pi-ticket', label: 'Tickets / Incidencias', route: '/tickets', roles: ['Administrador TI', 'Técnico Soporte'] },
      { id: '/licenses', icon: 'pi-key', label: 'Licencias de Software', route: '/licenses', roles: ['Administrador TI'] }
    ]},
    { section: 'Análisis', items: [
      { id: '/reports', icon: 'pi-chart-bar', label: 'Reportes Básicos', route: '/reports', roles: ['Administrador TI', 'Técnico Soporte'] },
      { id: '/pivot-tables', icon: 'pi-table', label: 'Tablas Dinámicas', route: '/pivot-tables', roles: ['Administrador TI'] }
    ]},
    { section: 'Administración', items: [
      { id: '/empresas', icon: 'pi-building', label: 'Empresas y Sucursales', route: '/empresas', roles: ['Super Admin', 'Administrador TI'] },
      { id: '/users', icon: 'pi-users', label: 'Usuarios y Permisos', route: '/users', roles: ['Administrador TI'] },
      { id: '/n8n-config', icon: 'pi-sitemap', label: 'Integración n8n', route: '/n8n-config', roles: ['Administrador TI'] },
      { id: '/settings', icon: 'pi-cog', label: 'Ajustes del Sistema', route: '/settings', roles: ['Administrador TI'] }
    ]}
  ];

  ngOnInit(): void {
    this.tabs$ = this.tabService.tabs$;
    this.cargarDatosUsuario();
    
    // 🚀 Le avisamos a Tailwind el estado inicial apenas carga la app
    if (this.isDarkMode) {
      document.documentElement.classList.add('dark');
    }
  }

  cargarDatosUsuario() {
    // 1. Obtenemos los datos completos del usuario logueado desde tu AuthService
    const currentUser = this.authService.currentUserValue;

    if (currentUser) {
      // 2. Extraemos el nombre real que viene desde tu backend (API)
      this.userName = currentUser.nombre || 'Usuario';
      
      // 3. Extraemos el rol principal
      this.userRole = currentUser.roles && currentUser.roles.length > 0 ? currentUser.roles[0] : 'Invitado';
    } else {
      // Valores por defecto por si ocurre un error de sesión
      this.userName = 'Invitado';
      this.userRole = 'Sin Rol';
    }

    // 4. Tomamos la primera letra del nombre real para el círculo celeste
    this.userInitial = this.userName.charAt(0).toUpperCase();
  }

  toggleDarkMode() { 
  this.isDarkMode = !this.isDarkMode; 
  
  // 🚀 Aplicamos o quitamos la clase 'dark' a la raíz de la página (toda la pantalla)
  if (this.isDarkMode) {
    document.documentElement.classList.add('dark');
  } else {
    document.documentElement.classList.remove('dark');
  }
}
  toggleSidebar() { this.isSidebarCollapsed = !this.isSidebarCollapsed; }
  
  // 👈 Alterna la visibilidad del menú de perfil
  toggleProfileMenu() { 
    this.isProfileMenuOpen = !this.isProfileMenuOpen; 
  }

  // 👈 Lógica para cerrar sesión
  logout() {
    this.isProfileMenuOpen = false;
    // Si tu authService tiene un método logout, llámalo aquí. Por defecto limpiamos localStorage:
    localStorage.removeItem('token'); 
    // Opcional: limpiar otros datos de sesión
    this.router.navigate(['/auth/login']);
  }

  onMenuClick(item: any) {
    this.tabService.openTab({ id: item.id, label: item.label, icon: item.icon, route: item.route });
  }

  selectTab(tabId: string) { this.tabService.setActiveTab(tabId); }
  closeTab(tabId: string, event: Event) {
    event.stopPropagation();
    this.tabService.closeTab(tabId);
  }
}