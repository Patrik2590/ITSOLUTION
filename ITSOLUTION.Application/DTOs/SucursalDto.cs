using System;

namespace ITSOLUTION.Application.DTOs
{
    public class SucursalDto
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public bool EstaActiva { get; set; }
    }
}