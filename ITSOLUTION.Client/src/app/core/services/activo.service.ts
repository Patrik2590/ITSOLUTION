import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface ActivoIT {
  id?: number;
  codigoInventario: string;
  categoria: string;
  tipoEquipo: string;      // 👈 Faltaba este
  marca: string;
  modelo: string;
  numeroSerie: string;
  estado: string;
  ubicacion: string;
  
  // Especificaciones técnicas (opcionales)
  procesador?: string;
  memoriaRam?: string;
  almacenamiento?: string;
  especificaciones?: string; // 👈 Faltaba este
  direccionIP?: string;
  direccionMAC?: string;
  observaciones?: string;    // 👈 Faltaba este
  
  empleadoId?: number;
  empleadoNombre?: string;
}

@Injectable({
  providedIn: 'root'
})
export class ActivoService {
  // Utilizamos el mismo puerto de tu backend
  private apiUrl = 'https://localhost:7069/api/Activos';

  constructor(private http: HttpClient) {}

  getActivos(): Observable<ActivoIT[]> {
    return this.http.get<ActivoIT[]>(this.apiUrl);
  }

  // Usamos Partial<ActivoIT> tal como lo haces en Tickets
  createActivo(activo: Partial<ActivoIT>): Observable<ActivoIT> {
    return this.http.post<ActivoIT>(this.apiUrl, activo);
  }

  deleteActivo(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}