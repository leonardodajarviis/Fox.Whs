using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fox.Whs.Migrations
{
    /// <inheritdoc />
    public partial class _9DayAgo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ItemName",
                table: "FoxWms_SlittingProcessLine",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemName",
                table: "FoxWms_RewindingProcessLine",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemName",
                table: "FoxWms_PrintingProcessLine",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemName",
                table: "FoxWms_CuttingProcessLine",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemName",
                table: "FoxWms_BlowingProcessLine",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemName",
                table: "FoxWms_SlittingProcessLine");

            migrationBuilder.DropColumn(
                name: "ItemName",
                table: "FoxWms_RewindingProcessLine");

            migrationBuilder.DropColumn(
                name: "ItemName",
                table: "FoxWms_PrintingProcessLine");

            migrationBuilder.DropColumn(
                name: "ItemName",
                table: "FoxWms_CuttingProcessLine");

            migrationBuilder.DropColumn(
                name: "ItemName",
                table: "FoxWms_BlowingProcessLine");
        }
    }
}
