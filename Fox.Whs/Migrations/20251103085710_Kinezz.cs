using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fox.Whs.Migrations
{
    /// <inheritdoc />
    public partial class Kinezz : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WeightKg",
                table: "FoxWms_SlittingProcessLine",
                newName: "QuantityKg");

            migrationBuilder.RenameColumn(
                name: "WeightKg",
                table: "FoxWms_RewindingProcessLine",
                newName: "QuantityKg");

            migrationBuilder.RenameColumn(
                name: "WeightKg",
                table: "FoxWms_PrintingProcessLine",
                newName: "QuantityKg");

            migrationBuilder.RenameColumn(
                name: "WeightKg",
                table: "FoxWms_CuttingProcessLine",
                newName: "QuantityKg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuantityKg",
                table: "FoxWms_SlittingProcessLine",
                newName: "WeightKg");

            migrationBuilder.RenameColumn(
                name: "QuantityKg",
                table: "FoxWms_RewindingProcessLine",
                newName: "WeightKg");

            migrationBuilder.RenameColumn(
                name: "QuantityKg",
                table: "FoxWms_PrintingProcessLine",
                newName: "WeightKg");

            migrationBuilder.RenameColumn(
                name: "QuantityKg",
                table: "FoxWms_CuttingProcessLine",
                newName: "WeightKg");
        }
    }
}
