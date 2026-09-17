using System;

namespace ITSOLUTION.Application.DTOs
{
    public class TenantDto
    {
        public Guid Id { get; set; }
        public string NombreComercial { get; set; }
        public string RUC_NIT { get; set; }
        public string Dominio { get; set; }
        public string Estado { get; set; }
        public int SucursalesCount { get; set; } // Para mostrar cuántas sedes tiene


        public List<SucursalDto> Sucursales { get; set; } = new List<SucursalDto>();

    }
}