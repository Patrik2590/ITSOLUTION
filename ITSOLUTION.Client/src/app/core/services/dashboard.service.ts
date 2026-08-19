import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

// 1. Definimos las interfaces que coinciden exactamente con tus DTOs de C#
export interface KpiData {
  systemUptime: string;
  activeTickets: number;
  securityAlerts: number;
  pendingUpdates: number;
}

export interface AlertData {
  id: string;
  type: string; // "Critical", "Warning", "Info"
  message: string;
  time: string;
}

export interface DashboardData {
  kpis: KpiData;
  recentAlerts: AlertData[];
}

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  // 2. Colocamos la URL exacta que vimos en tu Swagger
  private apiUrl = 'https://localhost:7069/api/Dashboard';

  constructor(private http: HttpClient) {}

  // 3. Método para obtener los datos
  getDashboardSummary(): Observable<DashboardData> {
    return this.http.get<DashboardData>(this.apiUrl);
  }
}