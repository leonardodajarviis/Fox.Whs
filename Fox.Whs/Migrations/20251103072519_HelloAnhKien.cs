using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fox.Whs.Migrations
{
    /// <inheritdoc />
    public partial class HelloAnhKien : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_CuttingProcessesLine_FoxWms_CuttingProcesses_CuttingProcessId",
                table: "FoxWms_CuttingProcessesLine");

            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_CuttingProcessesLine_OCRD_CardCode",
                table: "FoxWms_CuttingProcessesLine");

            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_CuttingProcessesLine_OHEM_WorkerId",
                table: "FoxWms_CuttingProcessesLine");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FoxWms_CuttingProcessesLine",
                table: "FoxWms_CuttingProcessesLine");

            migrationBuilder.RenameTable(
                name: "FoxWms_CuttingProcessesLine",
                newName: "FoxWms_CuttingProcessLine");

            migrationBuilder.RenameIndex(
                name: "IX_FoxWms_CuttingProcessesLine_WorkerId",
                table: "FoxWms_CuttingProcessLine",
                newName: "IX_FoxWms_CuttingProcessLine_WorkerId");

            migrationBuilder.RenameIndex(
                name: "IX_FoxWms_CuttingProcessesLine_CuttingProcessId",
                table: "FoxWms_CuttingProcessLine",
                newName: "IX_FoxWms_CuttingProcessLine_CuttingProcessId");

            migrationBuilder.RenameIndex(
                name: "IX_FoxWms_CuttingProcessesLine_CardCode",
                table: "FoxWms_CuttingProcessLine",
                newName: "IX_FoxWms_CuttingProcessLine_CardCode");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FoxWms_CuttingProcessLine",
                table: "FoxWms_CuttingProcessLine",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_CuttingProcessLine_FoxWms_CuttingProcesses_CuttingProcessId",
                table: "FoxWms_CuttingProcessLine",
                column: "CuttingProcessId",
                principalTable: "FoxWms_CuttingProcesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_CuttingProcessLine_OCRD_CardCode",
                table: "FoxWms_CuttingProcessLine",
                column: "CardCode",
                principalTable: "OCRD",
                principalColumn: "CardCode");

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_CuttingProcessLine_OHEM_WorkerId",
                table: "FoxWms_CuttingProcessLine",
                column: "WorkerId",
                principalTable: "OHEM",
                principalColumn: "empID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_CuttingProcessLine_FoxWms_CuttingProcesses_CuttingProcessId",
                table: "FoxWms_CuttingProcessLine");

            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_CuttingProcessLine_OCRD_CardCode",
                table: "FoxWms_CuttingProcessLine");

            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_CuttingProcessLine_OHEM_WorkerId",
                table: "FoxWms_CuttingProcessLine");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FoxWms_CuttingProcessLine",
                table: "FoxWms_CuttingProcessLine");

            migrationBuilder.RenameTable(
                name: "FoxWms_CuttingProcessLine",
                newName: "FoxWms_CuttingProcessesLine");

            migrationBuilder.RenameIndex(
                name: "IX_FoxWms_CuttingProcessLine_WorkerId",
                table: "FoxWms_CuttingProcessesLine",
                newName: "IX_FoxWms_CuttingProcessesLine_WorkerId");

            migrationBuilder.RenameIndex(
                name: "IX_FoxWms_CuttingProcessLine_CuttingProcessId",
                table: "FoxWms_CuttingProcessesLine",
                newName: "IX_FoxWms_CuttingProcessesLine_CuttingProcessId");

            migrationBuilder.RenameIndex(
                name: "IX_FoxWms_CuttingProcessLine_CardCode",
                table: "FoxWms_CuttingProcessesLine",
                newName: "IX_FoxWms_CuttingProcessesLine_CardCode");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FoxWms_CuttingProcessesLine",
                table: "FoxWms_CuttingProcessesLine",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_CuttingProcessesLine_FoxWms_CuttingProcesses_CuttingProcessId",
                table: "FoxWms_CuttingProcessesLine",
                column: "CuttingProcessId",
                principalTable: "FoxWms_CuttingProcesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_CuttingProcessesLine_OCRD_CardCode",
                table: "FoxWms_CuttingProcessesLine",
                column: "CardCode",
                principalTable: "OCRD",
                principalColumn: "CardCode");

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_CuttingProcessesLine_OHEM_WorkerId",
                table: "FoxWms_CuttingProcessesLine",
                column: "WorkerId",
                principalTable: "OHEM",
                principalColumn: "empID");
        }
    }
}
