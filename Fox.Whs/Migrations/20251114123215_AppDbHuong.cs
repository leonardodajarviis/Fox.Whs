using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fox.Whs.Migrations
{
    /// <inheritdoc />
    public partial class AppDbHuong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductOrderId",
                table: "FoxWms_GrainMixingProcessLine",
                newName: "ProductionOrderId");

            migrationBuilder.RenameColumn(
                name: "ProductOrderId",
                table: "FoxWms_GrainMixingBlowingProcessLine",
                newName: "ProductionOrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductionOrderId",
                table: "FoxWms_GrainMixingProcessLine",
                newName: "ProductOrderId");

            migrationBuilder.RenameColumn(
                name: "ProductionOrderId",
                table: "FoxWms_GrainMixingBlowingProcessLine",
                newName: "ProductOrderId");
        }
    }
}
