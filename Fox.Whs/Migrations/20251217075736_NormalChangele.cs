using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fox.Whs.Migrations
{
    /// <inheritdoc />
    public partial class NormalChangele : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PrintingMachineName",
                table: "FoxWms_SlittingProcessLine",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SlittingMachineName",
                table: "FoxWms_SlittingProcessLine",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RewindingMachineName",
                table: "FoxWms_RewindingProcessLine",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrintingMachineName",
                table: "FoxWms_PrintingProcessLine",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MixingMachine",
                table: "FoxWms_GrainMixingProcessLine",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MixingMachineName",
                table: "FoxWms_GrainMixingProcessLine",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MixingMachine",
                table: "FoxWms_GrainMixingBlowingProcessLine",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MixingMachineName",
                table: "FoxWms_GrainMixingBlowingProcessLine",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BlowingMachineName",
                table: "FoxWms_GrainMixingBlowingProcess",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CuttingMachineName",
                table: "FoxWms_CuttingProcessLine",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrintingMachineName",
                table: "FoxWms_CuttingProcessLine",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BlowingMachineName",
                table: "FoxWms_BlowingProcessLine",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrintingMachineName",
                table: "FoxWms_SlittingProcessLine");

            migrationBuilder.DropColumn(
                name: "SlittingMachineName",
                table: "FoxWms_SlittingProcessLine");

            migrationBuilder.DropColumn(
                name: "RewindingMachineName",
                table: "FoxWms_RewindingProcessLine");

            migrationBuilder.DropColumn(
                name: "PrintingMachineName",
                table: "FoxWms_PrintingProcessLine");

            migrationBuilder.DropColumn(
                name: "MixingMachine",
                table: "FoxWms_GrainMixingProcessLine");

            migrationBuilder.DropColumn(
                name: "MixingMachineName",
                table: "FoxWms_GrainMixingProcessLine");

            migrationBuilder.DropColumn(
                name: "MixingMachine",
                table: "FoxWms_GrainMixingBlowingProcessLine");

            migrationBuilder.DropColumn(
                name: "MixingMachineName",
                table: "FoxWms_GrainMixingBlowingProcessLine");

            migrationBuilder.DropColumn(
                name: "BlowingMachineName",
                table: "FoxWms_GrainMixingBlowingProcess");

            migrationBuilder.DropColumn(
                name: "CuttingMachineName",
                table: "FoxWms_CuttingProcessLine");

            migrationBuilder.DropColumn(
                name: "PrintingMachineName",
                table: "FoxWms_CuttingProcessLine");

            migrationBuilder.DropColumn(
                name: "BlowingMachineName",
                table: "FoxWms_BlowingProcessLine");
        }
    }
}
