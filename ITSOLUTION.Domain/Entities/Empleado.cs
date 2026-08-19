using System;
using System.Collections.Generic;
using System.Text;

namespace ITSOLUTION.Domain.Entities
{
    public class Empleado : BaseEntity // <-- ¡Magia de herencia aquí!
    {
        public string NombreCompleto { get; set; } = string.Empty;
        public string Departamento { get; set; } = string.Empty;
        public string Puesto { get; set; } = string.Empty;
        public string CorreoCorporativo { get; set; } = string.Empty;
        public bool EstaActivo { get; set; } = true;

        public List<ActivoIT> ActivosAsignados { get; set; } = new List<ActivoIT>();
    }
}