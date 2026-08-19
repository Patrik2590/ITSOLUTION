using System;
using System.Collections.Generic;
using System.Text;

namespace ITSOLUTION.Application.DTOs
{
    // Este DTO es para DEVOLVER la información completa al Frontend
    public class EmpleadoDto
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Departamento { get; set; } = string.Empty;
        public string Puesto { get; set; } = string.Empty;
        public string CorreoCorporativo { get; set; } = string.Empty;
        public bool EstaActivo { get; set; }
    }

    // Este DTO es para RECIBIR datos cuando registramos uno nuevo
    public class CreateEmpleadoDto
    {
        public string NombreCompleto { get; set; } = string.Empty;
        public string Departamento { get; set; } = string.Empty;
        public string Puesto { get; set; } = string.Empty;
        public string CorreoCorporativo { get; set; } = string.Empty;
    }
}