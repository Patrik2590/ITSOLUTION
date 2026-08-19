using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ITSOLUTION.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SincronizacionEmpresarial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Tenants",
                columns: new[] { "Id", "Estado", "FechaCreacion", "IsDeleted", "NombreComercial", "PlanSuscripcion", "RUC_NIT", "RazonSocial" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Activo", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, "TechCorp S.A.", "Premium", "1790000000001", "Technology Corporation Ecuador S.A." },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Activo", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, "GlobalNet", "Standard", "0990000000001", "Redes Globales del Ecuador Cia. Ltda." }
                });

            migrationBuilder.InsertData(
                table: "Sucursales",
                columns: new[] { "Id", "Direccion", "EstaActiva", "Nombre", "TenantId" },
                values: new object[,]
                {
                    { 1, "Av. Amazonas y Naciones Unidas", true, "TechCorp Matriz Quito", new Guid("11111111-1111-1111-1111-111111111111") },
                    { 2, "Malecón 2000", true, "TechCorp Sucursal Guayaquil", new Guid("11111111-1111-1111-1111-111111111111") },
                    { 3, "Centro Histórico", true, "GlobalNet Cuenca", new Guid("22222222-2222-2222-2222-222222222222") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Sucursales",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Sucursales",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Sucursales",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Tenants",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Tenants",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));
        }
    }
}
