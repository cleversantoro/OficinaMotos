using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OficinaMotos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddVeiculoIdToOrdemServico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "veiculo_id",
                table: "os_ordens",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_os_ordens_veiculo_id",
                table: "os_ordens",
                column: "veiculo_id");

            migrationBuilder.AddForeignKey(
                name: "FK_os_ordens_cad_veiculos_veiculo_id",
                table: "os_ordens",
                column: "veiculo_id",
                principalTable: "cad_veiculos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_os_ordens_cad_veiculos_veiculo_id",
                table: "os_ordens");

            migrationBuilder.DropIndex(
                name: "IX_os_ordens_veiculo_id",
                table: "os_ordens");

            migrationBuilder.DropColumn(
                name: "veiculo_id",
                table: "os_ordens");
        }
    }
}
