using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fox.Whs.Migrations
{
    /// <inheritdoc />
    public partial class LineHoat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "FoxWms_SlittingProcessLine",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "FoxWms_RewindingProcessLine",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "FoxWms_PrintingProcessLine",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "FoxWms_GrainMixingProcessLine",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "FoxWms_CuttingProcessLine",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "FoxWms_BlowingProcessLine",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "FoxWms_SlittingProcessLine");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "FoxWms_RewindingProcessLine");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "FoxWms_PrintingProcessLine");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "FoxWms_GrainMixingProcessLine");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "FoxWms_CuttingProcessLine");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "FoxWms_BlowingProcessLine");
        }
    }
}
