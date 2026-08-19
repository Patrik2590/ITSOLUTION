using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITSOLUTION.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EstructuraMultiTenant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreadoPor",
                table: "Empleados",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Empleados",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaModificacion",
                table: "Empleados",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModificadoPor",
                table: "Empleados",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SucursalId",
                table: "Empleados",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "Empleados",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "CreadoPor",
                table: "Activos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Activos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaModificacion",
                table: "Activos",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModificadoPor",
                table: "Activos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SucursalId",
                table: "Activos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "Activos",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Sucursales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstaActiva = table.Column<bool>(type: "bit", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sucursales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sucursales_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_SucursalId",
                table: "Empleados",
                column: "SucursalId");

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_TenantId",
                table: "Empleados",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Activos_SucursalId",
                table: "Activos",
                column: "SucursalId");

            migrationBuilder.CreateIndex(
                name: "IX_Activos_TenantId",
                table: "Activos",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Sucursales_TenantId",
                table: "Sucursales",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activos_Sucursales_SucursalId",
                table: "Activos",
                column: "SucursalId",
                principalTable: "Sucursales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Activos_Tenants_TenantId",
                table: "Activos",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Empleados_Sucursales_SucursalId",
                table: "Empleados",
                column: "SucursalId",
                principalTable: "Sucursales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Empleados_Tenants_TenantId",
                table: "Empleados",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activos_Sucursales_SucursalId",
                table: "Activos");

            migrationBuilder.DropForeignKey(
                name: "FK_Activos_Tenants_TenantId",
                table: "Activos");

            migrationBuilder.DropForeignKey(
                name: "FK_Empleados_Sucursales_SucursalId",
                table: "Empleados");

            migrationBuilder.DropForeignKey(
                name: "FK_Empleados_Tenants_TenantId",
                table: "Empleados");

            migrationBuilder.DropTable(
                name: "Sucursales");

            migrationBuilder.DropIndex(
                name: "IX_Empleados_SucursalId",
                table: "Empleados");

            migrationBuilder.DropIndex(
                name: "IX_Empleados_TenantId",
                table: "Empleados");

            migrationBuilder.DropIndex(
                name: "IX_Activos_SucursalId",
                table: "Activos");

            migrationBuilder.DropIndex(
                name: "IX_Activos_TenantId",
                table: "Activos");

            migrationBuilder.DropColumn(
                name: "CreadoPor",
                table: "Empleados");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Empleados");

            migrationBuilder.DropColumn(
                name: "FechaModificacion",
                table: "Empleados");

            migrationBuilder.DropColumn(
                name: "ModificadoPor",
                table: "Empleados");

            migrationBuilder.DropColumn(
                name: "SucursalId",
                table: "Empleados");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Empleados");

            migrationBuilder.DropColumn(
                name: "CreadoPor",
                table: "Activos");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Activos");

            migrationBuilder.DropColumn(
                name: "FechaModificacion",
                table: "Activos");

            migrationBuilder.DropColumn(
                name: "ModificadoPor",
                table: "Activos");

            migrationBuilder.DropColumn(
                name: "SucursalId",
                table: "Activos");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Activos");
        }
    }
}
