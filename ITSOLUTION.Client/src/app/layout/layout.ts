import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router'; // IMPORTANTE para el router-outlet
import { Observable } from 'rxjs';
import { TabService } from '../core/services/tab.service';
import { Tab } from '../core/models/tab.model';

@Component({
  selector: 'app-layout',
  standalone: true,
  // Asegúrate de importar RouterModule para que funcione la navegación interna
  imports: [CommonModule, RouterModule], 
  templateUrl: './layout.html',
  styleUrl: './layout.scss'
})
export class LayoutComponent implements OnInit {
  isDarkMode: boolean = true;
  isSidebarCollapsed: boolean = false;
  
  // Observador de pestañas gestionado por el servicio
  tabs$: Observable<Tab[]>;

  menuItems = [
    { section: 'Principal', items: [
      { id: '/dashboard', icon: 'pi-home', label: 'Dashboard', route: '/dashboard' }
    ]},
    { section: 'Gestión TI', items: [
      { id: '/inventory', icon: 'pi-box', label: 'Inventarios', route: '/inventory' },
      { id: '/tickets', icon: 'pi-ticket', label: 'Tickets / Incidencias', route: '/tickets' },
      { id: '/licenses', icon: 'pi-key', label: 'Licencias de Software', route: '/licenses' }
    ]},
    { section: 'Análisis', items: [
      { id: '/reports', icon: 'pi-chart-bar', label: 'Reportes Básicos', route: '/reports' },
      { id: '/pivot-tables', icon: 'pi-table', label: 'Tablas Dinámicas', route: '/pivot-tables' }
    ]},
    { section: 'Administración', items: [
      { id: '/users', icon: 'pi-users', label: 'Usuarios y Permisos', route: '/users' },
      { id: '/n8n-config', icon: 'pi-sitemap', label: 'Integración n8n', route: '/n8n-config' },
      { id: '/settings', icon: 'pi-cog', label: 'Ajustes del Sistema', route: '/settings' }
    ]}
  ];

  constructor(private tabService: TabService) {
    // Conectamos nuestra variable observable al servicio
    this.tabs$ = this.tabService.tabs$;
  }

  ngOnInit(): void {}

  toggleDarkMode() {
    this.isDarkMode = !this.isDarkMode;
  }

  toggleSidebar() {
    this.isSidebarCollapsed = !this.isSidebarCollapsed;
  }

  // Método que dispara el clic del menú lateral
  onMenuClick(item: any) {
    this.tabService.openTab({
      id: item.id,
      label: item.label,
      icon: item.icon,
      route: item.route
    });
  }

  // Métodos que disparan los clics de la barra de pestañas
  selectTab(tabId: string) {
    this.tabService.setActiveTab(tabId);
  }

  closeTab(tabId: string, event: Event) {
    event.stopPropagation(); // Evita que se dispare selectTab() al mismo tiempo
    this.tabService.closeTab(tabId);
  }
}