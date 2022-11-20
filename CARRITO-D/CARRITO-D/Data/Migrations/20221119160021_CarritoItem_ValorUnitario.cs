using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CARRITO_D.Migrations
{
    public partial class CarritoItem_ValorUnitario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValorUnitario",
                table: "CarritosItems");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "ValorUnitario",
                table: "CarritosItems",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
