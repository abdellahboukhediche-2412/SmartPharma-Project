using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartPharma.Migrations
{
    /// <inheritdoc />
    public partial class AjoutQuantiteParBoiteEtForme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Forme",
                table: "Medicaments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "QuantiteParBoite",
                table: "Medicaments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Forme",
                table: "Medicaments");

            migrationBuilder.DropColumn(
                name: "QuantiteParBoite",
                table: "Medicaments");
        }
    }
}
