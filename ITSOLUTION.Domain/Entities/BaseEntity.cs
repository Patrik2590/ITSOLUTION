using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSOLUTION.Domain.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        // ¡Cambiado de int a Guid para que coincida con el Tenant original!
        public Guid TenantId { get; set; }

        [ForeignKey("TenantId")]
        public Tenant? Tenant { get; set; }

        public int SucursalId { get; set; }

        [ForeignKey("SucursalId")]
        public Sucursal? Sucursal { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public string CreadoPor { get; set; } = string.Empty;
        public DateTime? FechaModificacion { get; set; }
        public string? ModificadoPor { get; set; }
    }
}