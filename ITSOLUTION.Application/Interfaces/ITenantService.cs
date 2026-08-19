using ITSOLUTION.Application.DTOs;

namespace ITSOLUTION.Application.Interfaces
{
    public interface ITenantService
    {
        Task<List<TenantDto>> GetAllTenantsAsync();
        Task<TenantDto?> GetTenantByIdAsync(Guid id);
        Task<TenantDto> CreateTenantAsync(TenantDto newTenantDto);
        Task<TenantDto?> UpdateTenantAsync(Guid id, TenantDto updatedTenantDto);
        Task<bool> DeactivateTenantAsync(Guid id); // Borrado lógico
    }
}