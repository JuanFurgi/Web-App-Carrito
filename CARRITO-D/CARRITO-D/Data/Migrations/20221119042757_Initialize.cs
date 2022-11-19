using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CARRITO_D.Migrations
{
    public partial class Initialize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StocksItems_Categorias_CategoriaId",
                table: "StocksItems");

            migrationBuilder.DropIndex(
                name: "IX_StocksItems_CategoriaId",
                table: "StocksItems");

            migrationBuilder.DropColumn(
                name: "CategoriaId",
                table: "StocksItems");

            migrationBuilder.DropColumn(
                name: "Subtotal",
                table: "CarritosItems");

            migrationBuilder.DropColumn(
                name: "Subtotal",
                table: "Carritos");

            migrationBuilder.AlterColumn<string>(
                name: "Direccion",
                table: "Sucursales",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "StocksItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "CarritosItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "StocksItems");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CarritosItems");

            migrationBuilder.AlterColumn<string>(
                name: "Direccion",
                table: "Sucursales",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoriaId",
                table: "StocksItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Subtotal",
                table: "CarritosItems",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Subtotal",
                table: "Carritos",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateIndex(
                name: "IX_StocksItems_CategoriaId",
                table: "StocksItems",
                column: "CategoriaId");

            migrationBuilder.AddForeignKey(
                name: "FK_StocksItems_Categorias_CategoriaId",
                table: "StocksItems",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "CategoriaId");
        }
    }
}
