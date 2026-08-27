using System;

namespace ITSOLUTION.Application.DTOs.Activos
{
    public class ActivoDto
    {
        public int Id { get; set; }
        public string CodigoInventario { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public string NumeroSerie { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Ubicacion { get; set; } = string.Empty;

        // Datos del empleado asignado
        public int? EmpleadoId { get; set; }
        public string? EmpleadoNombre { get; set; } // Ej: "Jonathan Monteros"

        // Atributos de hardware
        public string? Procesador { get; set; }
        public string? MemoriaRam { get; set; }
        public string? Almacenamiento { get; set; }

        // Redes
        public string? DireccionIP { get; set; }
        public string? DireccionMAC { get; set; }
    }
}