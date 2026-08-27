using System;

namespace ITSOLUTION.Domain.Entities
{
    public class ActivoIT : BaseEntity
    {
        // --- Campos existentes
        public string Categoria { get; set; } = string.Empty;
        public string TipoEquipo { get; set; } = string.Empty;
        public string Marca {  get; set; } = string.Empty;
        public string Modelo {  get; set; } = string.Empty;
        public string NumeroSerie { get; set; } = string.Empty;
        public string? DireccionIP {  get; set; }
        public string? DireccionMAC {  get; set; }
        public string? Especificaciones {  get; set; } // reservado para el JSON de detalles extras
        public string Estado {  get; set; } = string.Empty;

        public int? EmpleadoId { get; set; }
        public virtual Empleado? Empleado { get; set; }

        // -----NUEVOS CAMPOS PARA ALINEAR  CON EL DOCU,ENTO EXCEK DE MUESTRA DE INVENTARIO ---
        public string CodigoInventario { get; set;} = string.Empty;
        public string Ubicacion { get; set; } = string.Empty;
        public string? Observaciones { get; set;}

        //---- Atributos  clave  de hardwarebpara reportes rapidos
        public string? Procesador {  get; set; }
        public string? MemoriaRam { get; set; }
        public string? Almacenamiento {  get; set; }
    }
}