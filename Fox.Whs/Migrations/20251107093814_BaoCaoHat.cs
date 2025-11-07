using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fox.Whs.Migrations
{
    /// <inheritdoc />
    public partial class BaoCaoHat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalBlowingStageMold",
                table: "FoxWms_CuttingProcess",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalCuttingStageMold",
                table: "FoxWms_CuttingProcess",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrintingStageMold",
                table: "FoxWms_CuttingProcess",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalBlowingStageMold",
                table: "FoxWms_CuttingProcess");

            migrationBuilder.DropColumn(
                name: "TotalCuttingStageMold",
                table: "FoxWms_CuttingProcess");

            migrationBuilder.DropColumn(
                name: "TotalPrintingStageMold",
                table: "FoxWms_CuttingProcess");
        }
    }
}
