using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITSOLUTION.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FormatoBascTickets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Tickets",
                newName: "Solicita");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Tickets",
                newName: "Observaciones");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Tickets",
                newName: "FechaReporte");

            migrationBuilder.AddColumn<string>(
                name: "ActividadesRealizadas",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Area",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "HoraAtencion",
                table: "Tickets",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "HoraCierre",
                table: "Tickets",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NombreUsuario",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActividadesRealizadas",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "Area",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "HoraAtencion",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "HoraCierre",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "NombreUsuario",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "Solicita",
                table: "Tickets",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Observaciones",
                table: "Tickets",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "FechaReporte",
                table: "Tickets",
                newName: "CreatedAt");
        }
    }
}
