using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CARRITO_D.Migrations
{
    public partial class AgregoFotos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Foto",
                table: "Productos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Foto",
                table: "Personas",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Foto",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "Foto",
                table: "Personas");
        }
    }
}
