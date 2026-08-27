import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface TicketAttachment {
  id?: number;
  fileName: string;
  filePath: string;
  contentType: string;
  uploadedAt: string;
}

export interface Ticket {
  id?: number;
  nombreUsuario: string;
  area: string;
  solicita: string;
  fechaReporte?: string;
  horaAtencion?: string;
  horaCierre?: string;
  actividadesRealizadas?: string;
  observaciones?: string;
  
  
  // Tipado estricto para evitar errores de "No Overlap" en el HTML
  status?: 'Abierto' | 'EnProceso' | 'Cerrado'; 
  
  priority?: string;
  tenantId?: string;
  sucursalId?: number;
  attachments?: TicketAttachment[];
}

@Injectable({
  providedIn: 'root'
})
export class TicketService {
  private apiUrl = 'https://localhost:7069/api/Tickets';

  constructor(private http: HttpClient) {}

  getTickets(): Observable<Ticket[]> {
    return this.http.get<Ticket[]>(this.apiUrl);
  }

  createTicket(ticket: Partial<Ticket>): Observable<Ticket> {
    return this.http.post<Ticket>(this.apiUrl, ticket);
  }

  // Métodos para el Técnico
  atenderTicket(id: number): Observable<Ticket> {
    return this.http.put<Ticket>(`${this.apiUrl}/${id}/Atender`, {});
  }

  cerrarTicket(id: number, data: Partial<Ticket>): Observable<Ticket> {
    return this.http.put<Ticket>(`${this.apiUrl}/${id}/Cerrar`, data);
  }

  uploadAttachment(id: number, file: File): Observable<TicketAttachment> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post<TicketAttachment>(`${this.apiUrl}/${id}/Upload`, formData);
  }
}