using System;
using System.Collections.Generic;
using System.Text;

// Archivo: RolPermiso.cs
namespace ITSOLUTION.Domain.Entities
{
    public class RolPermiso
    {
        public int RolId { get; set; }
        public Rol Rol { get; set; } = null!;

        public int PermisoId { get; set; }
        public Permiso Permiso { get; set; } = null!;
    }
}

// Archivo: UsuarioRol.cs
namespace ITSOLUTION.Domain.Entities
{
    public class UsuarioRol
    {
        public int UsuarioId { get; set; } // Ajusta el tipo si tu UsuarioId es Guid o int
        public Usuario Usuario { get; set; } = null!;

        public int RolId { get; set; }
        public Rol Rol { get; set; } = null!;
    }
}
