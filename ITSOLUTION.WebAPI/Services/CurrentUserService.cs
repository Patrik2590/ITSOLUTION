using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using ITSOLUTION.Application.Interfaces;

namespace ITSOLUTION.WebAPI.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        // Inyectamos el lector de contexto HTTP (que contiene el Token del usuario)
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid TenantId
        {
            get
            {
                // Buscamos el claim "TenantId" dentro del Token del usuario logueado
                var tenantClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("TenantId")?.Value;
                return string.IsNullOrEmpty(tenantClaim) ? Guid.Empty : Guid.Parse(tenantClaim);
            }
        }

        public int SucursalId
        {
            get
            {
                // Buscamos el claim "SucursalId" dentro del Token
                var sucursalClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("SucursalId")?.Value;
                return string.IsNullOrEmpty(sucursalClaim) ? 0 : int.Parse(sucursalClaim);
            }
        }
    }
}