using System;
using System.Collections.Generic;
using System.Text;

namespace ITSOLUTION.Domain.Entities
{
    public class Rol
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;      // Ej: "Administrador TI"
        public string Descripcion { get; set; } = string.Empty;

        // 🏢 Aislamiento: Este rol le pertenece a una empresa específica
        public Guid TenantId { get; set; }

        public ICollection<RolPermiso> RolesPermisos { get; set; } = new List<RolPermiso>();
        public ICollection<UsuarioRol> UsuariosRoles { get; set; } = new List<UsuarioRol>();
    }
}