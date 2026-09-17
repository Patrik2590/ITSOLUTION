import { Component, inject, OnInit, ChangeDetectorRef, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { EmpresaService, GuardarEmpresaRequest } from '../../core/services/empresa.service';

interface Sucursal {
  id?: number;
  nombre: string;
  codigo: string;
  direccion: string;
  estaActiva?: boolean; 
}

interface PermisoModulo {
  modulo: string;
  ver: boolean;
  crear: boolean;
  editar: boolean;
  eliminar: boolean;
}

@Component({
  selector: 'app-empresas',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './empresas.component.html'
})
export class EmpresasComponent implements OnInit {
  private empresaService = inject(EmpresaService);
  private cdr = inject(ChangeDetectorRef);

  // 👇 NUEVO: Se agregó 'detalle-sucursales' a los estados de la vista
  vistaActual: 'lista' | 'formulario' | 'detalle-sucursales' = 'lista';
  tabFormulario: 'datos' | 'sucursales' | 'permisos' = 'datos';
  modoFormulario: 'crear' | 'editar' = 'crear'; 
  
  menuActivoId: string | null = null;
  menuSucursalActivoId: number | null = null; // Para el menú de sucursales
  empresaSeleccionadaId: string | null = null;
  
  // 👇 NUEVO: Variable para almacenar la empresa seleccionada en la vista de detalle
  empresaSeleccionadaDetalle: any = null;

  empresas: any[] = [];

  empresaForm = {
    id: '', 
    nombre: '',
    ruc: '',
    dominio: '',
    emailContacto: '',
    telefono: '',
    direccionMatriz: ''
  };

  sucursales: Sucursal[] = [];
  nuevaSucursal: Sucursal = { nombre: '', codigo: '', direccion: '', estaActiva: true };

  rolSeleccionado: string = 'Administrador TI';
  matrizPermisos: PermisoModulo[] = [
    { modulo: 'Inventario TI (Equipos)', ver: true, crear: true, editar: true, eliminar: false },
    { modulo: 'Gestión de Tickets', ver: true, crear: true, editar: true, eliminar: true },
    { modulo: 'Licencias de Software', ver: true, crear: true, editar: false, eliminar: false },
    { modulo: 'Reportes y Analítica', ver: true, crear: false, editar: false, eliminar: false },
    { modulo: 'Ajustes de Empresa', ver: true, crear: false, editar: false, eliminar: false }
  ];

  ngOnInit(): void {
    this.cargarEmpresas();
  }

  @HostListener('document:click', ['$event'])
  cerrarMenuClickFuera(event: Event) {
    const target = event.target as HTMLElement;
    if (!target.closest('.menu-acciones-container')) {
      this.menuActivoId = null;
      this.menuSucursalActivoId = null;
    }
  }

  cargarEmpresas() {
    this.empresaService.obtenerEmpresas().subscribe({
      next: (data: any[]) => {
        const empresasActivas = data.filter(t => 
          t.isDeleted !== 1 && t.IsDeleted !== 1 && 
          t.isDeleted !== true && t.IsDeleted !== true
        );

        this.empresas = empresasActivas.map(t => ({
          id: t.id || t.Id,
          nombre: t.nombreComercial || t.NombreComercial || 'Sin Nombre', 
          ruc: t.ruC_NIT || t.ruc_NIT || t.RUC_NIT || t.ruc_Nit || 'Sin RUC',
          dominio: t.dominio || t.Dominio || 'N/A', 
          estado: t.estado || t.Estado || 'Desconocido',
          sucursalesCount: t.sucursalesCount || t.SucursalesCount || t.sucursales?.length || t.Sucursales?.length || 0,
          _raw: t 
        }));
        
        this.cdr.detectChanges(); 
      },
      error: (err) => console.error('Error al cargar la lista de empresas', err)
    });
  }

  toggleMenu(id: string, event: Event) {
    event.stopPropagation();
    this.menuActivoId = this.menuActivoId === id ? null : id;
  }

  toggleMenuSucursal(id: number | undefined, index: number, event: Event) {
    event.stopPropagation();
    // Usa el ID si existe, sino el índice temporal
    const targetId = id ?? index; 
    this.menuSucursalActivoId = this.menuSucursalActivoId === targetId ? null : targetId;
  }

  // 👇 NUEVO: Función para abrir la vista de detalles al dar clic en la fila
  abrirDetalleSucursales(empresa: any) {
    this.empresaSeleccionadaDetalle = empresa;
    this.vistaActual = 'detalle-sucursales';
  }

  abrirFormularioCreacion() {
    this.resetFormulario();
    this.modoFormulario = 'crear';
    this.vistaActual = 'formulario';
    this.tabFormulario = 'datos';
  }

  abrirFormularioEdicion(empresa: any) {
    this.menuActivoId = null; 
    this.modoFormulario = 'editar';
    this.empresaSeleccionadaId = empresa.id;
    
    this.empresaForm = {
      id: empresa.id,
      nombre: empresa.nombre,
      ruc: empresa.ruc,
      dominio: empresa.dominio !== 'N/A' ? empresa.dominio : '',
      emailContacto: empresa._raw.emailContacto || '',
      telefono: empresa._raw.telefono || '',
      direccionMatriz: empresa._raw.direccionMatriz || ''
    };

    this.sucursales = empresa._raw.sucursales || empresa._raw.Sucursales || []; 
    this.vistaActual = 'formulario';
    this.tabFormulario = 'datos';
  }

  cancelar() {
    this.vistaActual = 'lista';
    this.menuActivoId = null;
    this.empresaSeleccionadaDetalle = null;
  }

  agregarSucursal() {
    if (this.nuevaSucursal.nombre && this.nuevaSucursal.codigo) {
      this.sucursales.push({ ...this.nuevaSucursal, estaActiva: true });
      this.nuevaSucursal = { nombre: '', codigo: '', direccion: '', estaActiva: true };
    }
  }

  // 👇 MODIFICADO: Ahora hace eliminación lógica enviando el estado al backend
   // Getter para saber si la tabla debe mostrar el mensaje de vacío
  get tieneSucursalesActivas(): boolean {
    return this.sucursales.some(suc => suc.estaActiva !== false);
  }

  // Se elimina buscando el objeto directamente en lugar del índice del HTML
  eliminarSucursalEnFormulario(suc: Sucursal) {
    if (suc.id) {
      suc.estaActiva = false;
    } else {
      const index = this.sucursales.indexOf(suc);
      if (index !== -1) {
        this.sucursales.splice(index, 1);
      }
    }
  }

  guardarEmpresaCompleta() {
    if (!this.empresaForm.nombre || !this.empresaForm.ruc) {
      alert('Por favor complete los campos obligatorios (Nombre y RUC).');
      return;
    }

    const payload: GuardarEmpresaRequest = {
      nombre: this.empresaForm.nombre,
      ruc: this.empresaForm.ruc,
      dominio: this.empresaForm.dominio,
      emailContacto: this.empresaForm.emailContacto,
      sucursales: this.sucursales, 
      permisos: [] 
    };

    const requestObservable = this.modoFormulario === 'crear' 
      ? this.empresaService.guardarEmpresa(payload)
      : this.empresaService.actualizarEmpresa(this.empresaSeleccionadaId!, payload);

    requestObservable.subscribe({
      next: (res) => {
        this.cargarEmpresas(); 
        this.cancelar(); 
      },
      error: (err) => {
        console.error('Error al guardar en el servidor:', err);
        alert(err.error?.details || 'Ocurrió un error al guardar la empresa.');
      }
    });
  }

  eliminarEmpresa(empresa: any) {
    this.menuActivoId = null;
    const accion = empresa.estado === 'Activo' ? 'desactivar' : 'activar';
    const confirmacion = confirm(`¿Estás seguro de que deseas ${accion} la empresa "${empresa.nombre}"?`);
  
    if (!confirmacion) return;

    this.empresaService.eliminarEmpresa(empresa.id).subscribe({
      next: () => {
        this.cargarEmpresas();
      },
      error: (err: any) => {
        console.error(`Error al ${accion} la empresa:`, err);
      }
    });
  }

  private resetFormulario() {
    this.empresaForm = { id: '', nombre: '', ruc: '', dominio: '', emailContacto: '', telefono: '', direccionMatriz: '' };
    this.sucursales = [];
    this.empresaSeleccionadaId = null;
  }
}