using System;
using System.Collections.Generic;
using System.Text;

namespace ITSOLUTION.Domain.Entities
{
    public class Permiso
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;      // Ej: "Crear Nuevo Ticket"
        public string Codigo { get; set; } = string.Empty;      // Ej: "TICKETS_CREATE" (Este usaremos en Angular y .NET)
        public string Modulo { get; set; } = string.Empty;      // Ej: "Helpdesk" (Para agruparlos visualmente)

        public ICollection<RolPermiso> RolesPermisos { get; set; } = new List<RolPermiso>();
    }
}
