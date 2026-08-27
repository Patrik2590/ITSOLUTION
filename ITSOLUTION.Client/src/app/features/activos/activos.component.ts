import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivoService, ActivoIT } from '../../core/services/activo.service';
// Si tienes un servicio para los empleados, impórtalo también para llenar el select de asignación

@Component({
  selector: 'app-activos',
  standalone: true, // Asumo que usas standalone components, si no, quita esto y decláralo en tu módulo
  imports: [CommonModule, FormsModule],
  templateUrl: './activos.component.html'
})
export class ActivosComponent implements OnInit {
  activos: ActivoIT[] = [];
  
  // Control del Modal
  isModalOpen = false;
  
  // Objeto para el formulario
  activoForm: Partial<ActivoIT> = {};

  // 🚀 Opciones para los Datalists (Desplegables que permiten escribir)
  categorias = ['Laptop', 'Computadora de Escritorio', 'Monitor', 'Impresora', 'Servidor', 'Teléfono'];
  marcas = ['Dell', 'HP', 'Lenovo', 'Apple', 'Asus', 'Samsung', 'Epson'];
  estados = ['Nuevo', 'Bueno', 'Regular', 'Malo', 'Dado de Baja'];

  // Aquí iría tu lista de empleados traída de la base de datos
  empleados: any[] = []; 

  constructor(private activoService: ActivoService) {}

  ngOnInit(): void {
    this.cargarActivos();
  }

  cargarActivos() {
    this.activoService.getActivos().subscribe({
      next: (data) => this.activos = data,
      error: (err) => console.error('Error al cargar activos', err)
    });
  }

  abrirModalNuevo() {
    this.activoForm = {}; // Limpiamos el formulario
    this.isModalOpen = true;
  }

  cerrarModal() {
    this.isModalOpen = false;
  }

  guardarActivo() {
    this.activoService.createActivo(this.activoForm).subscribe({
      next: (res) => {
        this.cargarActivos(); // Recargar la tabla
        this.cerrarModal();
      },
      error: (err) => console.error('Error al guardar', err)
    });
  }

  eliminarActivo(id: number) {
    if(confirm('¿Estás seguro de eliminar este equipo?')) {
      this.activoService.deleteActivo(id).subscribe({
        next: () => this.cargarActivos(),
        error: (err) => console.error('Error al eliminar', err)
      });
    }
  }
}