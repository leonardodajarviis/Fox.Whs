using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fox.Whs.Migrations
{
    /// <inheritdoc />
    public partial class MuaXuanNamAy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "FoxWms_BlowingProcesses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<short>(
                name: "CreatorId",
                table: "FoxWms_BlowingProcesses",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "FoxWms_BlowingProcesses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "ModifierId",
                table: "FoxWms_BlowingProcesses",
                type: "smallint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Printers",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Printers", x => x.Code);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_BlowingProcesses_CreatorId",
                table: "FoxWms_BlowingProcesses",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_BlowingProcesses_ModifierId",
                table: "FoxWms_BlowingProcesses",
                column: "ModifierId");

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_BlowingProcesses_OUSR_CreatorId",
                table: "FoxWms_BlowingProcesses",
                column: "CreatorId",
                principalTable: "OUSR",
                principalColumn: "USERID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_BlowingProcesses_OUSR_ModifierId",
                table: "FoxWms_BlowingProcesses",
                column: "ModifierId",
                principalTable: "OUSR",
                principalColumn: "USERID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_BlowingProcesses_OUSR_CreatorId",
                table: "FoxWms_BlowingProcesses");

            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_BlowingProcesses_OUSR_ModifierId",
                table: "FoxWms_BlowingProcesses");

            migrationBuilder.DropTable(
                name: "Printers");

            migrationBuilder.DropIndex(
                name: "IX_FoxWms_BlowingProcesses_CreatorId",
                table: "FoxWms_BlowingProcesses");

            migrationBuilder.DropIndex(
                name: "IX_FoxWms_BlowingProcesses_ModifierId",
                table: "FoxWms_BlowingProcesses");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "FoxWms_BlowingProcesses");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "FoxWms_BlowingProcesses");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "FoxWms_BlowingProcesses");

            migrationBuilder.DropColumn(
                name: "ModifierId",
                table: "FoxWms_BlowingProcesses");
        }
    }
}
