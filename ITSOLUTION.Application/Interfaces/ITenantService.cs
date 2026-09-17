using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ITSOLUTION.Application.DTOs;

namespace ITSOLUTION.Application.Interfaces
{
    public interface ITenantService
    {
        Task<List<TenantDto>> GetAllTenantsAsync();
        Task<TenantDto?> GetTenantByIdAsync(Guid id);
        Task<TenantDto> CreateTenantAsync(TenantDto newTenantDto);
        Task<TenantDto?> UpdateTenantAsync(Guid id, TenantDto updatedTenantDto);

        Task<bool> DeactivateTenantAsync(Guid id);

        // Agrégalo en tu lista de métodos dentro de public interface ITenantService
        Task<TenantDto> UpdateTenantWithDetailsAsync(Guid id, CreateTenantCompleteDto request);

        // Nuevo método para la creación compleja
        Task<TenantDto> CreateTenantWithDetailsAsync(CreateTenantCompleteDto request);
    }
}