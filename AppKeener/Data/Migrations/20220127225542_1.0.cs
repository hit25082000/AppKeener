using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppKeener.Data.Migrations
{
    public partial class _10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Imagem",
                table: "Produtos");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Imagem",
                table: "Produtos",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
