using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fox.Whs.Migrations
{
    /// <inheritdoc />
    public partial class updateinsertnotescolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "FoxWms_SlittingProcess",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "FoxWms_RewindingProcess",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "FoxWms_PrintingProcess",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "FoxWms_GrainMixingProcess",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "FoxWms_GrainMixingBlowingProcess",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "FoxWms_CuttingProcess",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "FoxWms_BlowingProcess",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "FoxWms_SlittingProcess");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "FoxWms_RewindingProcess");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "FoxWms_PrintingProcess");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "FoxWms_GrainMixingProcess");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "FoxWms_GrainMixingBlowingProcess");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "FoxWms_CuttingProcess");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "FoxWms_BlowingProcess");
        }
    }
}
