import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface SucursalDto {
  codigo: string;
  nombre: string;
  direccion: string;
}

export interface PermisoModuloDto {
  modulo: string;
  ver: boolean;
  crear: boolean;
  editar: boolean;
  eliminar: boolean;
}

export interface GuardarEmpresaRequest {
  nombre: string;
  ruc: string;
  dominio: string;
  emailContacto: string;
  sucursales: SucursalDto[];
  permisos: PermisoModuloDto[];
}

@Injectable({
  providedIn: 'root'
})
export class EmpresaService {
  // Apunta a la ruta base de tu controlador TenantsController
  private baseUrl = 'https://localhost:7069/api/Tenants';

  constructor(private http: HttpClient) {}

  guardarEmpresa(data: GuardarEmpresaRequest): Observable<any> {
    // Apunta al endpoint específico para guardar toda la estructura
    return this.http.post<any>(`${this.baseUrl}/Complete`, data);
  }
  // Agrega esto debajo de guardarEmpresa
  actualizarEmpresa(id: string, data: GuardarEmpresaRequest): Observable<any> {
    return this.http.put<any>(`${this.baseUrl}/Complete/${id}`, data);
  }

  obtenerEmpresas(): Observable<any[]> {
    // Apunta al GET general
    return this.http.get<any[]>(this.baseUrl);
  }

 eliminarEmpresa(id: string) {
    // Corregido: se usa this.baseUrl en lugar de this.apiUrl
    return this.http.delete(`${this.baseUrl}/${id}`);
  } 

}