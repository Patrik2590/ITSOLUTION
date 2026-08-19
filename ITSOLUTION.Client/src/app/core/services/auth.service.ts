import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';

// 🔄 ACTUALIZADO: Adaptado al nuevo backend RBAC
export interface AuthResponse {
  token: string;
  nombre: string;
  roles: string[]; // 👈 Cambio clave: ahora es un arreglo de textos
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  // Asegúrate de que este puerto coincide con el de tu Visual Studio al correr la API
  private apiUrl = 'https://localhost:7069/api/Auth/login'; 

  // Subject para emitir reactivamente los cambios de usuario
  private currentUserSubject = new BehaviorSubject<AuthResponse | null>(null); 
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(private http: HttpClient) {
    // Al iniciar el servicio (o recargar con F5), intentamos recuperar la sesión
    this.cargarUsuarioDesdeStorage();
  }

  // --- MÉTODOS DE AUTENTICACIÓN REAL ---

  login(correo: string, password: string): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(this.apiUrl, { correo, password }).pipe(
      tap(response => {
        // Guardamos el token de seguridad y los datos en el navegador
        localStorage.setItem('jwt_token', response.token);
        // 🔄 ACTUALIZADO: Guardamos el arreglo 'roles' en lugar de 'rol'
        localStorage.setItem('user_data', JSON.stringify({ nombre: response.nombre, roles: response.roles }));
        
        // Avisamos a toda la aplicación que alguien inició sesión
        this.currentUserSubject.next(response);
      })
    );
  }

  logout() {
    localStorage.removeItem('jwt_token');
    localStorage.removeItem('user_data');
    this.currentUserSubject.next(null);
  }

  // Obtener el valor actual sincrónicamente
  get currentUserValue(): AuthResponse | null {
    return this.currentUserSubject.value;
  }

  private cargarUsuarioDesdeStorage() {
    const token = localStorage.getItem('jwt_token');
    const userData = localStorage.getItem('user_data');
    if (token && userData) {
      this.currentUserSubject.next(JSON.parse(userData));
    }
  }

  // 🌟 NUEVO: Método necesario para la directiva *hasRole y Guards
  getRolesUsuario(): string[] {
    const user = this.currentUserValue;
    return user?.roles || []; // Si no hay roles, devuelve un arreglo vacío para evitar errores
  }

  // --- REGLAS DE NEGOCIO (Permisos Mantenidos y Actualizados) ---

  // ¿Puede ver todos los tickets de la empresa o sucursal?
  canViewAllTickets(): boolean {
    const roles = this.getRolesUsuario();
    // 🔄 Validamos si el arreglo incluye alguno de estos roles exactos que insertamos en la BD
    return roles.includes('Administrador TI') || roles.includes('Técnico Soporte');
  }

  // ¿Puede cambiar estados de los tickets?
  canManageTickets(): boolean {
    const roles = this.getRolesUsuario();
    return roles.includes('Administrador TI') || roles.includes('Técnico Soporte');
  }

  // ¿Puede imprimir reportes masivos?
  canPrintBatch(): boolean {
    const roles = this.getRolesUsuario();
    // 🛡️ Solo el Administrador TI tiene este poder
    return roles.includes('Administrador TI');
  }
}