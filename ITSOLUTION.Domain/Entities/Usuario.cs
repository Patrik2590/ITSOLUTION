using System;
using System.Collections.Generic; // 👈 Necesario para ICollection
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSOLUTION.Domain.Entities
{
    public class Usuario
    {
        public int Id { get; set; }

        public string Correo { get; set; } = string.Empty;

        // 🔒 NOTA SENIOR: Para esta fase de desarrollo usaremos texto plano. 
        // Antes de salir a producción, implementaremos BCrypt para encriptar estas contraseñas.
        public string Password { get; set; } = string.Empty;

        public string NombreCompleto { get; set; } = string.Empty;

        public bool EstaActivo { get; set; } = true;

        // 🏢 Relación estricta con la Empresa (Tenant)
        public Guid TenantId { get; set; }
        [ForeignKey("TenantId")]
        public Tenant? Tenant { get; set; }

        // 📍 Relación estricta con la Sucursal
        public int SucursalId { get; set; }
        [ForeignKey("SucursalId")]
        public Sucursal? Sucursal { get; set; }

        // 🔗 NUEVA RELACIÓN: Un usuario puede tener uno o múltiples roles (Motor RBAC)
        public ICollection<UsuarioRol> UsuariosRoles { get; set; } = new List<UsuarioRol>();
    }
}