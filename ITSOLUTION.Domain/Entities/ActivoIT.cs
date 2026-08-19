using System;
using System.Collections.Generic;
using System.Text;


namespace ITSOLUTION.Domain.Entities
{
    public class ActivoIT : BaseEntity // <-- ¡Y aquí también!
    {
        public string Categoria { get; set; } = string.Empty;
        public string TipoEquipo { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public string NumeroSerie { get; set; } = string.Empty;
        public string? DireccionIP { get; set; }
        public string? DireccionMAC { get; set; }
        public string? Especificaciones { get; set; }
        public string Estado { get; set; } = "En Bodega";

        public int? EmpleadoId { get; set; }
        public Empleado? EmpleadoAsignado { get; set; }
    }
}