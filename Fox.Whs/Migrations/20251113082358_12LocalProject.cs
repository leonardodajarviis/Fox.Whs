using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fox.Whs.Migrations
{
    /// <inheritdoc />
    public partial class _12LocalProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "FoxWms_SlittingProcess",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "FoxWms_RewindingProcess",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "FoxWms_PrintingProcess",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "FoxWms_GrainMixingProcess",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "FoxWms_GrainMixingBlowingProcess",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "FoxWms_CuttingProcess",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "FoxWms_BlowingProcess",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "FoxWms_SlittingProcess");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "FoxWms_RewindingProcess");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "FoxWms_PrintingProcess");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "FoxWms_GrainMixingProcess");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "FoxWms_GrainMixingBlowingProcess");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "FoxWms_CuttingProcess");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "FoxWms_BlowingProcess");
        }
    }
}
