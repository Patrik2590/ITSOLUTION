using System;

namespace ITSOLUTION.Application.DTOs.Activos
{
    public class CreateActivoDto
    {
        public string CodigoInventario { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string TipoEquipo { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public string NumeroSerie { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Ubicacion { get; set; } = string.Empty;
        public string? Observaciones { get; set; }

        // Asignación (Opcional al crear)
        public int? EmpleadoId { get; set; }

        // Especificaciones
        public string? Procesador { get; set; }
        public string? MemoriaRam { get; set; }
        public string? Almacenamiento { get; set; }
        public string? DireccionIP { get; set; }
        public string? DireccionMAC { get; set; }
    }
}