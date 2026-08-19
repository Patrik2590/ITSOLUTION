import { Injectable } from '@angular/core';
import Swal from 'sweetalert2';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  // Notificación tipo "Toast" (esquina superior derecha)
  toast(icon: 'success' | 'error' | 'warning' | 'info', title: string) {
    Swal.fire({
      toast: true,
      position: 'top-end',
      icon: icon,
      title: title,
      showConfirmButton: false,
      timer: 3000,
      timerProgressBar: true,
      background: '#1f2937', // bg-gray-800 de Tailwind
      color: '#ffffff',
      iconColor: icon === 'success' ? '#10b981' : icon === 'error' ? '#ef4444' : icon === 'warning' ? '#f59e0b' : '#3b82f6'
    });
  }

  // Cuadro de diálogo de confirmación para acciones críticas
  async confirm(title: string, text: string): Promise<boolean> {
    const result = await Swal.fire({
      title: title,
      text: text,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#0891b2', // bg-cyan-600
      cancelButtonColor: '#374151', // bg-gray-700
      confirmButtonText: 'Sí, continuar',
      cancelButtonText: 'Cancelar',
      background: '#1f2937',
      color: '#ffffff',
    });
    return result.isConfirmed;
  }
}