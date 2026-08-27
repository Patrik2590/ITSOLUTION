using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITSOLUTION.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateActivos_AddInventarioFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Almacenamiento",
                table: "Activos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodigoInventario",
                table: "Activos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MemoriaRam",
                table: "Activos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Observaciones",
                table: "Activos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Procesador",
                table: "Activos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ubicacion",
                table: "Activos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Almacenamiento",
                table: "Activos");

            migrationBuilder.DropColumn(
                name: "CodigoInventario",
                table: "Activos");

            migrationBuilder.DropColumn(
                name: "MemoriaRam",
                table: "Activos");

            migrationBuilder.DropColumn(
                name: "Observaciones",
                table: "Activos");

            migrationBuilder.DropColumn(
                name: "Procesador",
                table: "Activos");

            migrationBuilder.DropColumn(
                name: "Ubicacion",
                table: "Activos");
        }
    }
}
