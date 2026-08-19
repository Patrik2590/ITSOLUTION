using System;

namespace ITSOLUTION.Application.DTOs
{
    public class TenantDto
    {
        public Guid Id { get; set; }
        public string NombreComercial { get; set; } = string.Empty;
        public string RUC_NIT { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
    }
}