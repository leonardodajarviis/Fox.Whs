using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fox.Whs.Migrations
{
    /// <inheritdoc />
    public partial class EmHuong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PoSurplusTpSlittingKg",
                table: "FoxWms_SlittingProcessLine",
                newName: "ExcessPOSlitting");

            migrationBuilder.RenameColumn(
                name: "PoSurplusBtpInKg",
                table: "FoxWms_SlittingProcessLine",
                newName: "ExcessPOPrinting");

            migrationBuilder.RenameColumn(
                name: "PoSurplusKg",
                table: "FoxWms_RewindingProcessLine",
                newName: "ExcessPO");

            migrationBuilder.RenameColumn(
                name: "PoSurplus",
                table: "FoxWms_PrintingProcessLine",
                newName: "ExcessPO");

            migrationBuilder.RenameColumn(
                name: "PoSurplusOver5Kg",
                table: "FoxWms_CuttingProcessLine",
                newName: "ExcessPOOver5Kg");

            migrationBuilder.RenameColumn(
                name: "PoSurplusLess5Kg",
                table: "FoxWms_CuttingProcessLine",
                newName: "ExcessPOLess5Kg");

            migrationBuilder.RenameColumn(
                name: "PoSurplusCutKg",
                table: "FoxWms_CuttingProcessLine",
                newName: "ExcessPOCut");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExcessPOSlitting",
                table: "FoxWms_SlittingProcessLine",
                newName: "PoSurplusTpSlittingKg");

            migrationBuilder.RenameColumn(
                name: "ExcessPOPrinting",
                table: "FoxWms_SlittingProcessLine",
                newName: "PoSurplusBtpInKg");

            migrationBuilder.RenameColumn(
                name: "ExcessPO",
                table: "FoxWms_RewindingProcessLine",
                newName: "PoSurplusKg");

            migrationBuilder.RenameColumn(
                name: "ExcessPO",
                table: "FoxWms_PrintingProcessLine",
                newName: "PoSurplus");

            migrationBuilder.RenameColumn(
                name: "ExcessPOOver5Kg",
                table: "FoxWms_CuttingProcessLine",
                newName: "PoSurplusOver5Kg");

            migrationBuilder.RenameColumn(
                name: "ExcessPOLess5Kg",
                table: "FoxWms_CuttingProcessLine",
                newName: "PoSurplusLess5Kg");

            migrationBuilder.RenameColumn(
                name: "ExcessPOCut",
                table: "FoxWms_CuttingProcessLine",
                newName: "PoSurplusCutKg");
        }
    }
}
