using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fox.Whs.Migrations
{
    /// <inheritdoc />
    public partial class SuCo101 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedAt",
                table: "FoxWms_SlittingProcessLine",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedAt",
                table: "FoxWms_RewindingProcessLine",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedAt",
                table: "FoxWms_PrintingProcessLine",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedAt",
                table: "FoxWms_GrainMixingProcessLine",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedAt",
                table: "FoxWms_GrainMixingBlowingProcessLine",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedAt",
                table: "FoxWms_CuttingProcessLine",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedAt",
                table: "FoxWms_BlowingProcessLine",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovedAt",
                table: "FoxWms_SlittingProcessLine");

            migrationBuilder.DropColumn(
                name: "ApprovedAt",
                table: "FoxWms_RewindingProcessLine");

            migrationBuilder.DropColumn(
                name: "ApprovedAt",
                table: "FoxWms_PrintingProcessLine");

            migrationBuilder.DropColumn(
                name: "ApprovedAt",
                table: "FoxWms_GrainMixingProcessLine");

            migrationBuilder.DropColumn(
                name: "ApprovedAt",
                table: "FoxWms_GrainMixingBlowingProcessLine");

            migrationBuilder.DropColumn(
                name: "ApprovedAt",
                table: "FoxWms_CuttingProcessLine");

            migrationBuilder.DropColumn(
                name: "ApprovedAt",
                table: "FoxWms_BlowingProcessLine");
        }
    }
}
