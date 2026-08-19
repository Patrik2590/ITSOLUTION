using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITSOLUTION.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ActualizarUsuariosHelpdesk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "Rol",
                value: "JefeSistemas");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Correo", "NombreCompleto", "Rol", "SucursalId" },
                values: new object[] { "juan@techcorp.com", "Juan Soporte", "Tecnico", 1 });

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Correo", "NombreCompleto", "Rol", "SucursalId", "TenantId" },
                values: new object[] { "maria@techcorp.com", "María RH", "Empleado", 1, new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Correo", "EstaActivo", "NombreCompleto", "Password", "Rol", "SucursalId", "TenantId" },
                values: new object[] { 4, "carlos@globalnet.com", true, "Carlos TI", "password123", "Tecnico", 3, new Guid("22222222-2222-2222-2222-222222222222") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "Rol",
                value: "Administrador");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Correo", "NombreCompleto", "Rol", "SucursalId" },
                values: new object[] { "ventas@techcorp.com", "Vendedor Guayaquil", "Vendedor", 2 });

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Correo", "NombreCompleto", "Rol", "SucursalId", "TenantId" },
                values: new object[] { "admin@globalnet.com", "Admin GlobalNet", "Administrador", 3, new Guid("22222222-2222-2222-2222-222222222222") });
        }
    }
}
