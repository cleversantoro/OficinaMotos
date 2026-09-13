using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OficinaMotos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProximoKmRevisaoToVeiculo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "proximo_km_revisao",
                table: "cad_veiculos",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "proximo_km_revisao",
                table: "cad_veiculos");
        }
    }
}
