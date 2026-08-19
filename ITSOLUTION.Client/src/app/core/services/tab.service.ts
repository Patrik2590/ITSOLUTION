import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { Tab } from '../models/tab.model';

@Injectable({
  providedIn: 'root'
})
export class TabService {
  // Estado inicial: Siempre abrimos el Dashboard por defecto
  private initialTabs: Tab[] = [
    { id: '/dashboard', label: 'Dashboard', icon: 'pi-home', route: '/dashboard', active: true }
  ];

  // BehaviorSubjects para emitir los cambios de estado de forma reactiva
  private tabsSubject = new BehaviorSubject<Tab[]>(this.initialTabs);
  public tabs$ = this.tabsSubject.asObservable();

  constructor(private router: Router) {}

  /**
   * Abre una nueva pestaña o enfoca una existente si ya está abierta.
   */
  openTab(tab: Omit<Tab, 'active'>) {
    const currentTabs = this.tabsSubject.getValue();
    const existingTabIndex = currentTabs.findIndex(t => t.id === tab.id);

    // Desactivar todas las pestañas actuales
    const updatedTabs = currentTabs.map(t => ({ ...t, active: false }));

    if (existingTabIndex !== -1) {
      // Si la pestaña ya existe, simplemente la activamos
      updatedTabs[existingTabIndex].active = true;
    } else {
      // Si no existe, la agregamos a la lista como activa
      updatedTabs.push({ ...tab, active: true });
    }

    this.tabsSubject.next(updatedTabs);
    this.router.navigate([tab.route]);
  }

  /**
   * Cierra una pestaña específica y maneja la redirección.
   */
  closeTab(tabId: string) {
    const currentTabs = this.tabsSubject.getValue();
    
    // Regla de negocio: No permitir cerrar el Dashboard (Tab Principal)
    if (tabId === '/dashboard' && currentTabs.length === 1) return;

    const tabToCloseIndex = currentTabs.findIndex(t => t.id === tabId);
    if (tabToCloseIndex === -1) return;

    const isClosingActiveTab = currentTabs[tabToCloseIndex].active;
    const newTabs = currentTabs.filter(t => t.id !== tabId);

    // Si cerramos la pestaña activa, enfocamos la anterior (o la siguiente si era la primera)
    if (isClosingActiveTab && newTabs.length > 0) {
      const nextFocusIndex = tabToCloseIndex > 0 ? tabToCloseIndex - 1 : 0;
      newTabs[nextFocusIndex].active = true;
      this.router.navigate([newTabs[nextFocusIndex].route]);
    }

    this.tabsSubject.next(newTabs);
  }

  /**
   * Cambia el foco a una pestaña sin recargarla.
   */
  setActiveTab(tabId: string) {
    const currentTabs = this.tabsSubject.getValue();
    const updatedTabs = currentTabs.map(t => ({
      ...t,
      active: t.id === tabId
    }));
    
    this.tabsSubject.next(updatedTabs);
    const activeTab = updatedTabs.find(t => t.active);
    if (activeTab) {
      this.router.navigate([activeTab.route]);
    }
  }
}