using System;
using System.Collections.Generic;

namespace ITSOLUTION.Domain.Entities
{
    public class Tenant
    {
        // Este ID mapea con el UNIQUEIDENTIFIER de SQL Server
        public Guid Id { get; set; }

        public string NombreComercial { get; set; } = string.Empty;

        public string? RazonSocial { get; set; } // El "?" significa que permite valores nulos (NULL)

        public string RUC_NIT { get; set; } = string.Empty;

        public string PlanSuscripcion { get; set; } = "Standard";

        public string Estado { get; set; } = "Activo";

        public DateTimeOffset FechaCreacion { get; set; } = DateTimeOffset.UtcNow;

        public bool IsDeleted { get; set; } = false;

        public string Dominio { get; set; } = string.Empty;
        public string CorreoContacto { get; set; } = string.Empty;

        // 🔗 Navegación explícita (Relación 1 Empresa -> N Sucursales)
        public ICollection<Sucursal> Sucursales { get; set; } = new List<Sucursal>();
    }
}