using System;
using System.Collections.Generic;

namespace ITSOLUTION.Domain.Entities
{
    public class Empleado : BaseEntity
    {
        // 🚀 Campos nuevos y separados
        public string Cedula { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;

        public string Departamento { get; set; } = string.Empty;
        public string Puesto { get; set; } = string.Empty;
        public string CorreoCorporativo { get; set; } = string.Empty;
        public bool EstaActivo { get; set; } = true;

        public List<ActivoIT> ActivosAsignados { get; set; } = new List<ActivoIT>();
    }
}