using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartPharma.Migrations
{
    /// <inheritdoc />
    public partial class AjoutLignesVente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ventes_Medicaments_MedicamentId",
                table: "Ventes");

            migrationBuilder.DropIndex(
                name: "IX_Ventes_MedicamentId",
                table: "Ventes");

            migrationBuilder.DropColumn(
                name: "MedicamentId",
                table: "Ventes");

            migrationBuilder.DropColumn(
                name: "QuantiteVendue",
                table: "Ventes");

            migrationBuilder.CreateTable(
                name: "LignesVente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VenteId = table.Column<int>(type: "int", nullable: false),
                    MedicamentId = table.Column<int>(type: "int", nullable: false),
                    Quantite = table.Column<int>(type: "int", nullable: false),
                    PrixUnitaire = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SousTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LignesVente", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LignesVente_Medicaments_MedicamentId",
                        column: x => x.MedicamentId,
                        principalTable: "Medicaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LignesVente_Ventes_VenteId",
                        column: x => x.VenteId,
                        principalTable: "Ventes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LignesVente_MedicamentId",
                table: "LignesVente",
                column: "MedicamentId");

            migrationBuilder.CreateIndex(
                name: "IX_LignesVente_VenteId",
                table: "LignesVente",
                column: "VenteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LignesVente");

            migrationBuilder.AddColumn<int>(
                name: "MedicamentId",
                table: "Ventes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuantiteVendue",
                table: "Ventes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Ventes_MedicamentId",
                table: "Ventes",
                column: "MedicamentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ventes_Medicaments_MedicamentId",
                table: "Ventes",
                column: "MedicamentId",
                principalTable: "Medicaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
