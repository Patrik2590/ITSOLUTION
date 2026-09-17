using ITSOLUTION.Application.Interfaces;
using ITSOLUTION.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace ITSOLUTION.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        private readonly ICurrentUserService _currentUserService;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUserService currentUserService) : base(options)
        {
            _currentUserService = currentUserService;
        }

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Sucursal> Sucursales { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<ActivoIT> Activos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketAttachment> TicketAttachments { get; set; }

        // 🔐 NUEVAS TABLAS: Motor de Seguridad RBAC
        public DbSet<Permiso> Permisos { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<RolPermiso> RolesPermisos { get; set; }
        public DbSet<UsuarioRol> UsuariosRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🛡️ 1. FILTROS GLOBALES DE SEGURIDAD (Multi-Tenant y Multi-Sucursal)
            modelBuilder.Entity<Empleado>().HasQueryFilter(e =>
                e.TenantId == _currentUserService.TenantId && e.SucursalId == _currentUserService.SucursalId);

            modelBuilder.Entity<ActivoIT>().HasQueryFilter(e =>
                e.TenantId == _currentUserService.TenantId && e.SucursalId == _currentUserService.SucursalId);

            modelBuilder.Entity<Ticket>().HasQueryFilter(t =>
                t.TenantId == _currentUserService.TenantId && t.SucursalId == _currentUserService.SucursalId);


            // 🏢 NOTA SENIOR: Los roles pertenecen a la Empresa (Tenant), no a una sucursal específica.
            modelBuilder.Entity<Rol>().HasQueryFilter(r =>
                r.TenantId == _currentUserService.TenantId);

            modelBuilder.Entity<Tenant>().HasQueryFilter(t => !t.IsDeleted);

            // 🛑 2. DESACTIVAR EL BORRADO EN CASCADA GLOBALMENTE
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }

            // 🔗 3. LLAVES PRIMARIAS COMPUESTAS (Para las tablas intermedias de Roles)
            modelBuilder.Entity<RolPermiso>()
                .HasKey(rp => new { rp.RolId, rp.PermisoId });

            modelBuilder.Entity<UsuarioRol>()
                .HasKey(ur => new { ur.UsuarioId, ur.RolId });

            // 🏢 4. DATOS SEMILLA (Empresas y Sucursales)
            var tenantIdA = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var tenantIdB = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var fechaSemilla = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);

            modelBuilder.Entity<Tenant>().HasData(
                new Tenant { Id = tenantIdA, NombreComercial = "TechCorp S.A.", RazonSocial = "Technology Corporation Ecuador S.A.", RUC_NIT = "1790000000001", PlanSuscripcion = "Premium", Estado = "Activo", FechaCreacion = fechaSemilla, IsDeleted = false },
                new Tenant { Id = tenantIdB, NombreComercial = "GlobalNet", RazonSocial = "Redes Globales del Ecuador Cia. Ltda.", RUC_NIT = "0990000000001", PlanSuscripcion = "Standard", Estado = "Activo", FechaCreacion = fechaSemilla, IsDeleted = false }
            );

            modelBuilder.Entity<Sucursal>().HasData(
                new Sucursal { Id = 1, Nombre = "TechCorp Matriz Quito", Direccion = "Av. Amazonas y Naciones Unidas", EstaActiva = true, TenantId = tenantIdA },
                new Sucursal { Id = 2, Nombre = "TechCorp Sucursal Guayaquil", Direccion = "Malecón 2000", EstaActiva = true, TenantId = tenantIdA },
                new Sucursal { Id = 3, Nombre = "GlobalNet Cuenca", Direccion = "Centro Histórico", EstaActiva = true, TenantId = tenantIdB }
            );

            // 🧑‍💻 5. DATOS SEMILLA (Usuarios)
            // Ya sin la propiedad obsoleta "Rol"
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario { Id = 1, Correo = "admin@techcorp.com", Password = "password123", NombreCompleto = "Admin TechCorp", EstaActivo = true, TenantId = tenantIdA, SucursalId = 1 },
                new Usuario { Id = 2, Correo = "juan@techcorp.com", Password = "password123", NombreCompleto = "Juan Soporte", EstaActivo = true, TenantId = tenantIdA, SucursalId = 1 },
                new Usuario { Id = 3, Correo = "maria@techcorp.com", Password = "password123", NombreCompleto = "María RH", EstaActivo = true, TenantId = tenantIdA, SucursalId = 1 },
                new Usuario { Id = 4, Correo = "carlos@globalnet.com", Password = "password123", NombreCompleto = "Carlos TI", EstaActivo = true, TenantId = tenantIdB, SucursalId = 3 }
            );

            // ==========================================
            // 🛡️ MOTOR RBAC: POBLANDO LA SEGURIDAD
            // ==========================================

            // 6. DATOS SEMILLA (Permisos Globales del Sistema)
            // Estos son fijos y tú como creador los defines.
            modelBuilder.Entity<Permiso>().HasData(
                new Permiso { Id = 1, Nombre = "Ver Tickets", Codigo = "TICKETS_READ", Modulo = "Helpdesk" },
                new Permiso { Id = 2, Nombre = "Crear Tickets", Codigo = "TICKETS_CREATE", Modulo = "Helpdesk" },
                new Permiso { Id = 3, Nombre = "Resolver Tickets", Codigo = "TICKETS_RESOLVE", Modulo = "Helpdesk" },
                new Permiso { Id = 4, Nombre = "Gestionar Usuarios", Codigo = "USERS_MANAGE", Modulo = "Administración" }
            );

            // 7. DATOS SEMILLA (Roles específicos de la Empresa TechCorp)
            modelBuilder.Entity<Rol>().HasData(
                new Rol { Id = 1, Nombre = "Administrador TI", Descripcion = "Control total del sistema en la empresa", TenantId = tenantIdA },
                new Rol { Id = 2, Nombre = "Técnico Soporte", Descripcion = "Solo atiende incidencias de su sucursal", TenantId = tenantIdA }
            );

            // 8. DATOS SEMILLA (Vincular Permisos con Roles)
            modelBuilder.Entity<RolPermiso>().HasData(
                // El Administrador tiene todo
                new RolPermiso { RolId = 1, PermisoId = 1 },
                new RolPermiso { RolId = 1, PermisoId = 2 },
                new RolPermiso { RolId = 1, PermisoId = 3 },
                new RolPermiso { RolId = 1, PermisoId = 4 },

                // El Técnico solo ve y resuelve tickets
                new RolPermiso { RolId = 2, PermisoId = 1 },
                new RolPermiso { RolId = 2, PermisoId = 3 }
            );

            // 9. DATOS SEMILLA (Vincular Usuarios con Roles)
            modelBuilder.Entity<UsuarioRol>().HasData(
                new UsuarioRol { UsuarioId = 1, RolId = 1 }, // Admin TechCorp (ID: 1) -> Administrador TI (ID: 1)
                new UsuarioRol { UsuarioId = 2, RolId = 2 }  // Juan Soporte (ID: 2) -> Técnico Soporte (ID: 2)
            );
        }
    }
}