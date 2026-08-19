using System;

namespace ITSOLUTION.Application.Interfaces
{
    public interface ICurrentUserService
    {
        Guid TenantId { get; } // ¡Cambiado a Guid!
        int SucursalId { get; }
    }
}