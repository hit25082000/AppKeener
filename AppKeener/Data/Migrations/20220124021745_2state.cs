using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppKeener.Data.Migrations
{
    public partial class _2state : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FornecedorId",
                table: "Produtos",
                newName: "ProdutoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProdutoId",
                table: "Produtos",
                newName: "FornecedorId");
        }
    }
}
