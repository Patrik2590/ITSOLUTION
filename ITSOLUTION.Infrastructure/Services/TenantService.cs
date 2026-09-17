using ITSOLUTION.Application.DTOs;
using ITSOLUTION.Application.Interfaces;
using ITSOLUTION.Domain.Entities;
using ITSOLUTION.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                    RUC_NIT = t.RUC_NIT,
                    Dominio = t.Dominio,
                    Estado = t.Estado,
                    SucursalesCount = t.Sucursales.Count(s => s.EstaActiva),
                    // 👇 NUEVO: Mapear las sucursales para enviarlas al frontend
                    Sucursales = t.Sucursales.Select(s => new SucursalDto
                    {
                        Id = s.Id,
                        Codigo = s.Codigo,
                        Nombre = s.Nombre,
                        Direccion = s.Direccion,
                        EstaActiva = s.EstaActiva
                    }).ToList()
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
                RUC_NIT = newTenantDto.RUC_NIT,
                Estado = "Activo",
                PlanSuscripcion = "Básico",
                FechaCreacion = DateTimeOffset.UtcNow,
                IsDeleted = false
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

            // Ejecutamos el Soft Delete real
            tenant.IsDeleted = true;

            // Por doble seguridad, también lo marcamos como inactivo
            tenant.Estado = "Inactivo";

            _context.Tenants.Update(tenant);
            await _context.SaveChangesAsync();

            return true;
        }

        // --- MÉTODO DE CREACIÓN COMPLETA CORREGIDO ---
        public async Task<TenantDto> CreateTenantWithDetailsAsync(CreateTenantCompleteDto request)
        {
            // 1. Validar si el RUC ya existe 
            // (Gracias al HasQueryFilter en el DbContext, esto ignora automáticamente a las empresas eliminadas lógicamente)
            var rucExiste = await _context.Tenants.AnyAsync(t => t.RUC_NIT == request.Ruc);
            if (rucExiste)
            {
                throw new Exception($"Ya existe una empresa registrada con el RUC {request.Ruc}.");
            }

            // 2. Iniciar la transacción para garantizar integridad (Todo o Nada)
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var tenantId = Guid.NewGuid();

                var tenantEntity = new Tenant
                {
                    Id = tenantId,
                    NombreComercial = request.Nombre,
                    RazonSocial = request.Nombre,
                    RUC_NIT = request.Ruc,
                    Dominio = request.Dominio,
                    CorreoContacto = request.EmailContacto,
                    Estado = "Activo",
                    PlanSuscripcion = "Standard",
                    FechaCreacion = DateTimeOffset.UtcNow,
                    IsDeleted = false
                };

                // 3. Mapeo de Sucursales
                if (request.Sucursales != null && request.Sucursales.Any())
                {
                    foreach (var suc in request.Sucursales)
                    {
                        tenantEntity.Sucursales.Add(new Sucursal
                        {
                            Codigo = suc.Codigo,
                            Nombre = suc.Nombre,
                            Direccion = suc.Direccion,
                            EstaActiva = true,
                            TenantId = tenantId
                        });
                    }
                }

                // 4. Creación del Rol Base
                var rolAdmin = new Rol
                {
                    Nombre = "Administrador TI",
                    Descripcion = "Rol principal generado por el sistema",
                    TenantId = tenantId
                };

                _context.Tenants.Add(tenantEntity);
                _context.Roles.Add(rolAdmin);

                // 5. Guardar cambios en la base de datos
                await _context.SaveChangesAsync();

                // 6. Confirmar la transacción (Si llegamos aquí, todo se guardó correctamente)
                await transaction.CommitAsync();

                return new TenantDto
                {
                    Id = tenantEntity.Id,
                    NombreComercial = tenantEntity.NombreComercial,
                    RUC_NIT = tenantEntity.RUC_NIT,
                    Estado = tenantEntity.Estado
                };
            }
            catch (Exception)
            {
                // Si ocurre cualquier error (ej. base de datos caída, error de mapeo, llave foránea), 
                // se deshacen TODOS los cambios previos evitando registros huérfanos.
                await transaction.RollbackAsync();
                throw; // Relanza la excepción para que tu bloque try-catch en el Controlador la atrape y devuelva el HTTP 500
            }
        }

        public async Task<TenantDto> UpdateTenantWithDetailsAsync(Guid id, CreateTenantCompleteDto request)
        {
            var rucExiste = await _context.Tenants.AnyAsync(t => t.RUC_NIT == request.Ruc && t.Id != id);
            if (rucExiste) throw new Exception($"Ya existe otra empresa registrada con el RUC {request.Ruc}.");

            var tenantEntity = await _context.Tenants
                .Include(t => t.Sucursales)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tenantEntity == null) throw new Exception("Empresa no encontrada.");

            tenantEntity.NombreComercial = request.Nombre;
            tenantEntity.RazonSocial = request.Nombre;
            tenantEntity.RUC_NIT = request.Ruc;
            tenantEntity.Dominio = request.Dominio;
            tenantEntity.CorreoContacto = request.EmailContacto;

            // 👇 CORRECCIÓN: Actualizar existentes (Soft Delete) o añadir nuevas sin borrar físicamente
            if (request.Sucursales != null && request.Sucursales.Any())
            {
                foreach (var sucRequest in request.Sucursales)
                {
                    if (sucRequest.Id.HasValue && sucRequest.Id.Value > 0)
                    {
                        // Si ya tiene ID, buscamos la sucursal existente en memoria y la actualizamos
                        var sucursalExistente = tenantEntity.Sucursales.FirstOrDefault(s => s.Id == sucRequest.Id.Value);
                        if (sucursalExistente != null)
                        {
                            sucursalExistente.Codigo = sucRequest.Codigo;
                            sucursalExistente.Nombre = sucRequest.Nombre;
                            sucursalExistente.Direccion = sucRequest.Direccion;
                            sucursalExistente.EstaActiva = sucRequest.EstaActiva; // <--- Aquí se aplica el falso para borrar
                        }
                    }
                    else
                    {
                        // Si no tiene ID, es una sucursal nueva creada desde Angular
                        tenantEntity.Sucursales.Add(new Sucursal
                        {
                            Codigo = sucRequest.Codigo,
                            Nombre = sucRequest.Nombre,
                            Direccion = sucRequest.Direccion,
                            EstaActiva = true, // Las nuevas siempre nacen activas
                            TenantId = id
                        });
                    }
                }
            }

            // _context.Tenants.Update(tenantEntity) no es estrictamente necesario 
            // cuando usas Entity Framework Tracking (con el Include), pero está bien dejarlo por seguridad.

            await _context.SaveChangesAsync();

            return new TenantDto
            {
                Id = tenantEntity.Id,
                NombreComercial = tenantEntity.NombreComercial,
                RUC_NIT = tenantEntity.RUC_NIT,
                Estado = tenantEntity.Estado
            };
        }
    }
}