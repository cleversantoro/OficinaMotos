using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OficinaMotos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOrdemServicoStatusEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "os_ordens",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Aberta",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldDefaultValue: "ABERTA")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "os_ordens",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "ABERTA",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldDefaultValue: "Aberta")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
