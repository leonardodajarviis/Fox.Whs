using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fox.Whs.Migrations
{
    /// <inheritdoc />
    public partial class ThuHuong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "FoxWms_RewindingProcess",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<short>(
                name: "CreatorId",
                table: "FoxWms_RewindingProcess",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "FoxWms_RewindingProcess",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "ModifierId",
                table: "FoxWms_RewindingProcess",
                type: "smallint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_RewindingProcess_CreatorId",
                table: "FoxWms_RewindingProcess",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_RewindingProcess_ModifierId",
                table: "FoxWms_RewindingProcess",
                column: "ModifierId");

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_RewindingProcess_OUSR_CreatorId",
                table: "FoxWms_RewindingProcess",
                column: "CreatorId",
                principalTable: "OUSR",
                principalColumn: "USERID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_RewindingProcess_OUSR_ModifierId",
                table: "FoxWms_RewindingProcess",
                column: "ModifierId",
                principalTable: "OUSR",
                principalColumn: "USERID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_RewindingProcess_OUSR_CreatorId",
                table: "FoxWms_RewindingProcess");

            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_RewindingProcess_OUSR_ModifierId",
                table: "FoxWms_RewindingProcess");

            migrationBuilder.DropIndex(
                name: "IX_FoxWms_RewindingProcess_CreatorId",
                table: "FoxWms_RewindingProcess");

            migrationBuilder.DropIndex(
                name: "IX_FoxWms_RewindingProcess_ModifierId",
                table: "FoxWms_RewindingProcess");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "FoxWms_RewindingProcess");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "FoxWms_RewindingProcess");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "FoxWms_RewindingProcess");

            migrationBuilder.DropColumn(
                name: "ModifierId",
                table: "FoxWms_RewindingProcess");
        }
    }
}
