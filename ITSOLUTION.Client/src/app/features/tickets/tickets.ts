import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TicketService, Ticket } from '../../core/services/ticket.service';
// 1️⃣ Importamos el nuevo servicio de notificaciones
import { NotificationService } from '../../core/services/notification.service';

@Component({
  selector: 'app-tickets',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './tickets.html',
  styleUrls: ['./tickets.css'] // Asegúrate de que apunte a tu archivo CSS
})
export class TicketsComponent implements OnInit {
  tickets: Ticket[] = [];
  filteredTickets: Ticket[] = [];
  selectedTicket: Ticket | null = null;
  
  isLoading: boolean = true;
  currentFilter: string = 'Todos'; 

  showModal: boolean = false;
  isSubmitting: boolean = false;
  nuevoTicket: Partial<Ticket> = { nombreUsuario: '', area: '', solicita: '' };
  selectedFile: File | null = null;
  cierreData = { actividadesRealizadas: '', observaciones: '' };

  // --- NUEVAS VARIABLES DE IMPRESIÓN ---
  printMode: 'single' | 'batch' | null = null;
  ticketsParaImprimir: Ticket[] = [];

  // 2️⃣ Inyectamos el notification service en el constructor
  constructor(
    private ticketService: TicketService, 
    private cdr: ChangeDetectorRef,
    private notification: NotificationService
  ) {}

  ngOnInit(): void { this.loadTickets(); }

  loadTickets() {
    this.isLoading = true;
    const savedTicketId = this.selectedTicket?.id;
    this.ticketService.getTickets().subscribe({
      next: (data) => {
        this.tickets = data;
        this.applyFilter(this.currentFilter, true); 
        if (savedTicketId) this.selectedTicket = this.tickets.find(t => t.id === savedTicketId) || null;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.isLoading = false;
        this.notification.toast('error', 'No se pudieron cargar los tickets');
      }
    });
  }

  applyFilter(status: string, keepSelection: boolean = false) {
    this.currentFilter = status;
    if (status === 'Todos') this.filteredTickets = [...this.tickets];
    else this.filteredTickets = this.tickets.filter(t => t.status === status);
    if (!keepSelection) this.selectedTicket = null; 
  }

  selectTicket(ticket: Ticket) {
    this.selectedTicket = ticket;
    this.cierreData = { actividadesRealizadas: ticket.actividadesRealizadas || '', observaciones: ticket.observaciones || '' };
  }

  marcarAtencion() {
    if (!this.selectedTicket?.id) return;
    this.ticketService.atenderTicket(this.selectedTicket.id).subscribe({
      next: () => {
        this.notification.toast('success', 'Ticket tomado en proceso');
        this.loadTickets();
      },
      error: () => this.notification.toast('error', 'Hubo un error al tomar el ticket')
    });
  }

  // 3️⃣ Convertimos a async para poder usar el modal de confirmación
  async guardarCierre() {
    if (!this.selectedTicket?.id) return;
    
    // Validamos usando Toast en lugar de alert
    if (!this.cierreData.actividadesRealizadas) {
      this.notification.toast('warning', 'Debe ingresar las actividades.');
      return;
    }

    // Modal de confirmación premium
    const confirmado = await this.notification.confirm(
      '¿Cerrar Ticket?', 
      'Una vez cerrado no podrá ser modificado.'
    );

    if (confirmado) {
      this.ticketService.cerrarTicket(this.selectedTicket.id, this.cierreData).subscribe({
        next: () => {
          this.notification.toast('success', 'Ticket cerrado exitosamente');
          this.loadTickets();
        },
        error: () => this.notification.toast('error', 'Error al intentar cerrar el ticket')
      });
    }
  }

  // --- LÓGICA DE IMPRESIÓN ---
  imprimirIndividual() {
    this.printMode = 'single';
    this.cdr.detectChanges();
    setTimeout(() => { window.print(); this.printMode = null; }, 150);
  }

  imprimirLote() {
    this.ticketsParaImprimir = this.filteredTickets.filter(t => t.status === 'Cerrado');
    if (this.ticketsParaImprimir.length === 0) {
      // Reemplazamos el alert feo por un toast informativo
      this.notification.toast('info', 'No hay tickets cerrados en esta vista para imprimir.');
      return;
    }
    this.printMode = 'batch';
    this.cdr.detectChanges();
    setTimeout(() => { window.print(); this.printMode = null; }, 150);
  }

  // --- MODAL ---
  openModal() { this.showModal = true; this.nuevoTicket = { nombreUsuario: '', area: '', solicita: '' }; this.selectedFile = null; }
  closeModal() { this.showModal = false; }
  onFileSelected(event: any) { const file = event.target.files[0]; if (file) this.selectedFile = file; }

  crearTicket() {
    // Validación inicial
    if (!this.nuevoTicket.nombreUsuario || !this.nuevoTicket.area || !this.nuevoTicket.solicita) {
      this.notification.toast('warning', 'Por favor, complete todos los campos');
      return;
    }

    this.isSubmitting = true;
    this.ticketService.createTicket(this.nuevoTicket).subscribe({
      next: (created) => {
        if (this.selectedFile && created.id) {
          this.ticketService.uploadAttachment(created.id, this.selectedFile).subscribe({
            next: () => this.finalizarCreacion(true),
            error: () => { 
              this.notification.toast('warning', 'El ticket se creó, pero falló la carga del archivo'); 
              this.finalizarCreacion(false); 
            }
          });
        } else {
          this.finalizarCreacion(true);
        }
      },
      error: () => {
        this.notification.toast('error', 'Hubo un problema al crear el reporte');
        this.isSubmitting = false;
      }
    });
  }
  
  // 4️⃣ Modificamos para mostrar mensaje de éxito si todo salió bien
  private finalizarCreacion(exito: boolean = true) { 
    this.isSubmitting = false; 
    this.closeModal(); 
    this.loadTickets(); 
    if (exito) {
      this.notification.toast('success', 'Reporte generado correctamente');
    }
  }
}