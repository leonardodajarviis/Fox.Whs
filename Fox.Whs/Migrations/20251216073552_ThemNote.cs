using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fox.Whs.Migrations
{
    /// <inheritdoc />
    public partial class ThemNote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "FoxWms_SlittingProcessLine",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "FoxWms_RewindingProcessLine",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "FoxWms_PrintingProcessLine",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "FoxWms_GrainMixingProcessLine",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "FoxWms_GrainMixingBlowingProcessLine",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "FoxWms_CuttingProcessLine",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "FoxWms_SlittingProcessLine");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "FoxWms_RewindingProcessLine");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "FoxWms_PrintingProcessLine");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "FoxWms_GrainMixingProcessLine");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "FoxWms_GrainMixingBlowingProcessLine");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "FoxWms_CuttingProcessLine");
        }
    }
}
