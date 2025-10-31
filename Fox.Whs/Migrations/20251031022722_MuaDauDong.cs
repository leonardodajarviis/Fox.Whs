using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fox.Whs.Migrations
{
    /// <inheritdoc />
    public partial class MuaDauDong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WeighingDate",
                table: "Fox_BlowingProcessLines");

            migrationBuilder.AddColumn<string>(
                name: "RequiredDate",
                table: "Fox_BlowingProcessLines",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDraft",
                table: "Fox_BlowingProcesses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequiredDate",
                table: "Fox_BlowingProcessLines");

            migrationBuilder.DropColumn(
                name: "IsDraft",
                table: "Fox_BlowingProcesses");

            migrationBuilder.AddColumn<DateTime>(
                name: "WeighingDate",
                table: "Fox_BlowingProcessLines",
                type: "datetime2",
                nullable: true);
        }
    }
}
