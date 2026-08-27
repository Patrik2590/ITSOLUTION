export interface ActivoIT {
  id: number;
  codigoInventario: string;
  categoria: string;
  marca: string;
  modelo: string;
  numeroSerie: string;
  estado: string;
  ubicacion: string;
  
  empleadoId?: number;
  empleadoNombre?: string; 
  
  procesador?: string;
  memoriaRam?: string;
  almacenamiento?: string;
  direccionIP?: string;
  direccionMAC?: string;
}

export interface CreateActivoDto {
  codigoInventario: string;
  categoria: string;
  tipoEquipo: string;
  marca: string;
  modelo: string;
  numeroSerie: string;
  estado: string;
  ubicacion: string;
  observaciones?: string;
  empleadoId?: number | null;
  
  procesador?: string;
  memoriaRam?: string;
  almacenamiento?: string;
  direccionIP?: string;
  direccionMAC?: string;
}