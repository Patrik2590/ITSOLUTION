using ITSOLUTION.Application.DTOs;
using ITSOLUTION.Application.Interfaces;
using ITSOLUTION.Domain.Entities;
using ITSOLUTION.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ITSOLUTION.Infrastructure.Services
{
    public class TenantService : ITenantService
    {
        private readonly ApplicationDbContext _context;

        public TenantService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TenantDto>> GetAllTenantsAsync()
        {
            return await _context.Tenants
                .Select(t => new TenantDto
                {
                    Id = t.Id,
                    NombreComercial = t.NombreComercial,
                    // CORREGIDO: Ruc_NIT en mayúsculas
                    RUC_NIT = t.RUC_NIT,
                    Estado = t.Estado
                }).ToListAsync();
        }

        public async Task<TenantDto?> GetTenantByIdAsync(Guid id)
        {
            var tenant = await _context.Tenants.FindAsync(id);
            if (tenant == null) return null;

            return new TenantDto
            {
                Id = tenant.Id,
                NombreComercial = tenant.NombreComercial,
                // CORREGIDO: Ruc_NIT en mayúsculas
                RUC_NIT = tenant.RUC_NIT,
                Estado = tenant.Estado
            };
        }

        public async Task<TenantDto> CreateTenantAsync(TenantDto newTenantDto)
        {
            var tenantEntity = new Tenant
            {
                Id = Guid.NewGuid(),
                NombreComercial = newTenantDto.NombreComercial,
                // CORREGIDO: Ruc_NIT en mayúsculas
                RUC_NIT = newTenantDto.RUC_NIT,
                Estado = "Activo"
            };

            _context.Tenants.Add(tenantEntity);
            await _context.SaveChangesAsync();

            newTenantDto.Id = tenantEntity.Id;
            newTenantDto.Estado = tenantEntity.Estado;
            return newTenantDto;
        }

        public async Task<TenantDto?> UpdateTenantAsync(Guid id, TenantDto updatedTenantDto)
        {
            var tenant = await _context.Tenants.FindAsync(id);
            if (tenant == null) return null;

            tenant.NombreComercial = updatedTenantDto.NombreComercial;
            // CORREGIDO: Ruc_NIT en mayúsculas
            tenant.RUC_NIT = updatedTenantDto.RUC_NIT;

            await _context.SaveChangesAsync();

            updatedTenantDto.Id = tenant.Id;
            updatedTenantDto.Estado = tenant.Estado;
            return updatedTenantDto;
        }

        public async Task<bool> DeactivateTenantAsync(Guid id)
        {
            var tenant = await _context.Tenants.FindAsync(id);
            if (tenant == null) return false;

            tenant.Estado = "Inactivo";
            await _context.SaveChangesAsync();
            return true;
        }
    }
}