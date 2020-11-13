using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Supply.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Mercadorias",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(nullable: false),
                    Descricao = table.Column<string>(nullable: true),
                    Tipo = table.Column<string>(nullable: true),
                    Fabricante = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mercadorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Movimentacoes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DataHora = table.Column<DateTime>(nullable: false),
                    MovimentacaoEntrada = table.Column<bool>(nullable: false),
                    Quantidade = table.Column<int>(nullable: false),
                    Local = table.Column<string>(nullable: true),
                    IdMercadoria = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movimentacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Movimentacoes_Mercadorias_IdMercadoria",
                        column: x => x.IdMercadoria,
                        principalTable: "Mercadorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Movimentacoes_IdMercadoria",
                table: "Movimentacoes",
                column: "IdMercadoria");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movimentacoes");

            migrationBuilder.DropTable(
                name: "Mercadorias");
        }
    }
}
