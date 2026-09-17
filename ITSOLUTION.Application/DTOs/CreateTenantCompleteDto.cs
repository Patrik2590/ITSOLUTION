using System.Collections.Generic;

namespace ITSOLUTION.Application.DTOs
{
    public class CreateTenantCompleteDto
    {
        // Propiedades principales que coinciden con Angular
        public string Nombre { get; set; } = string.Empty;
        public string Ruc { get; set; } = string.Empty;
        public string Dominio { get; set; } = string.Empty;
        public string EmailContacto { get; set; } = string.Empty;

        // Listas para recibir las sucursales y permisos dinámicos
        public List<SucursalItemDto> Sucursales { get; set; } = new List<SucursalItemDto>();
        public List<PermisoModuloItemDto> Permisos { get; set; } = new List<PermisoModuloItemDto>();
    }

    public class SucursalItemDto
    {
        public int? Id { get; set; }
        public bool EstaActiva { get; set; } = true;
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
    }

    public class PermisoModuloItemDto
    {
        public string Modulo { get; set; } = string.Empty;
        public bool Ver { get; set; }
        public bool Crear { get; set; }
        public bool Editar { get; set; }
        public bool Eliminar { get; set; }
    }
}