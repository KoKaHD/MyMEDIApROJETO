using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RESTfulAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddPagamentoEExpedicaoEncomenda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataExpedicao",
                table: "Encomendas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataPagamento",
                table: "Encomendas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EstadoPagamento",
                table: "Encomendas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataExpedicao",
                table: "Encomendas");

            migrationBuilder.DropColumn(
                name: "DataPagamento",
                table: "Encomendas");

            migrationBuilder.DropColumn(
                name: "EstadoPagamento",
                table: "Encomendas");
        }
    }
}
