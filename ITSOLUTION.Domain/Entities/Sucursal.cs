using System;

namespace ITSOLUTION.Domain.Entities
{
    public class Sucursal
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public bool EstaActiva { get; set; } = true;

        // Llave foránea hacia la Empresa (Tenant)
        public Guid TenantId { get; set; }
        public Tenant? Tenant { get; set; }
    }
}