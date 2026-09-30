using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace banco_vb.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaContaRelacionadaTitular : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContaRelacionadaTitular",
                table: "Transacoes",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContaRelacionadaTitular",
                table: "Transacoes");
        }
    }
}
