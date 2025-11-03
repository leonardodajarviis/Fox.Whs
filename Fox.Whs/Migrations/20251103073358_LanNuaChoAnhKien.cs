using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fox.Whs.Migrations
{
    /// <inheritdoc />
    public partial class LanNuaChoAnhKien : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_PrintingProcesses_OHEM_ShiftLeaderId",
                table: "FoxWms_PrintingProcesses");

            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_PrintingProcesses_OUSR_CreatorId",
                table: "FoxWms_PrintingProcesses");

            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_PrintingProcesses_OUSR_ModifierId",
                table: "FoxWms_PrintingProcesses");

            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_PrintingProcessLines_FoxWms_PrintingProcesses_PrintingProcessId",
                table: "FoxWms_PrintingProcessLines");

            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_PrintingProcessLines_OCRD_CardCode",
                table: "FoxWms_PrintingProcessLines");

            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_PrintingProcessLines_OHEM_WorkerId",
                table: "FoxWms_PrintingProcessLines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FoxWms_PrintingProcessLines",
                table: "FoxWms_PrintingProcessLines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FoxWms_PrintingProcesses",
                table: "FoxWms_PrintingProcesses");

            migrationBuilder.RenameTable(
                name: "FoxWms_PrintingProcessLines",
                newName: "FoxWms_PrintingProcessLine");

            migrationBuilder.RenameTable(
                name: "FoxWms_PrintingProcesses",
                newName: "FoxWms_PrintingProcess");

            migrationBuilder.RenameIndex(
                name: "IX_FoxWms_PrintingProcessLines_WorkerId",
                table: "FoxWms_PrintingProcessLine",
                newName: "IX_FoxWms_PrintingProcessLine_WorkerId");

            migrationBuilder.RenameIndex(
                name: "IX_FoxWms_PrintingProcessLines_PrintingProcessId",
                table: "FoxWms_PrintingProcessLine",
                newName: "IX_FoxWms_PrintingProcessLine_PrintingProcessId");

            migrationBuilder.RenameIndex(
                name: "IX_FoxWms_PrintingProcessLines_CardCode",
                table: "FoxWms_PrintingProcessLine",
                newName: "IX_FoxWms_PrintingProcessLine_CardCode");

            migrationBuilder.RenameIndex(
                name: "IX_FoxWms_PrintingProcesses_ShiftLeaderId",
                table: "FoxWms_PrintingProcess",
                newName: "IX_FoxWms_PrintingProcess_ShiftLeaderId");

            migrationBuilder.RenameIndex(
                name: "IX_FoxWms_PrintingProcesses_ModifierId",
                table: "FoxWms_PrintingProcess",
                newName: "IX_FoxWms_PrintingProcess_ModifierId");

            migrationBuilder.RenameIndex(
                name: "IX_FoxWms_PrintingProcesses_CreatorId",
                table: "FoxWms_PrintingProcess",
                newName: "IX_FoxWms_PrintingProcess_CreatorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FoxWms_PrintingProcessLine",
                table: "FoxWms_PrintingProcessLine",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FoxWms_PrintingProcess",
                table: "FoxWms_PrintingProcess",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_PrintingProcess_OHEM_ShiftLeaderId",
                table: "FoxWms_PrintingProcess",
                column: "ShiftLeaderId",
                principalTable: "OHEM",
                principalColumn: "empID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_PrintingProcess_OUSR_CreatorId",
                table: "FoxWms_PrintingProcess",
                column: "CreatorId",
                principalTable: "OUSR",
                principalColumn: "USERID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_PrintingProcess_OUSR_ModifierId",
                table: "FoxWms_PrintingProcess",
                column: "ModifierId",
                principalTable: "OUSR",
                principalColumn: "USERID");

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_PrintingProcessLine_FoxWms_PrintingProcess_PrintingProcessId",
                table: "FoxWms_PrintingProcessLine",
                column: "PrintingProcessId",
                principalTable: "FoxWms_PrintingProcess",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_PrintingProcessLine_OCRD_CardCode",
                table: "FoxWms_PrintingProcessLine",
                column: "CardCode",
                principalTable: "OCRD",
                principalColumn: "CardCode");

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_PrintingProcessLine_OHEM_WorkerId",
                table: "FoxWms_PrintingProcessLine",
                column: "WorkerId",
                principalTable: "OHEM",
                principalColumn: "empID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_PrintingProcess_OHEM_ShiftLeaderId",
                table: "FoxWms_PrintingProcess");

            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_PrintingProcess_OUSR_CreatorId",
                table: "FoxWms_PrintingProcess");

            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_PrintingProcess_OUSR_ModifierId",
                table: "FoxWms_PrintingProcess");

            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_PrintingProcessLine_FoxWms_PrintingProcess_PrintingProcessId",
                table: "FoxWms_PrintingProcessLine");

            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_PrintingProcessLine_OCRD_CardCode",
                table: "FoxWms_PrintingProcessLine");

            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_PrintingProcessLine_OHEM_WorkerId",
                table: "FoxWms_PrintingProcessLine");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FoxWms_PrintingProcessLine",
                table: "FoxWms_PrintingProcessLine");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FoxWms_PrintingProcess",
                table: "FoxWms_PrintingProcess");

            migrationBuilder.RenameTable(
                name: "FoxWms_PrintingProcessLine",
                newName: "FoxWms_PrintingProcessLines");

            migrationBuilder.RenameTable(
                name: "FoxWms_PrintingProcess",
                newName: "FoxWms_PrintingProcesses");

            migrationBuilder.RenameIndex(
                name: "IX_FoxWms_PrintingProcessLine_WorkerId",
                table: "FoxWms_PrintingProcessLines",
                newName: "IX_FoxWms_PrintingProcessLines_WorkerId");

            migrationBuilder.RenameIndex(
                name: "IX_FoxWms_PrintingProcessLine_PrintingProcessId",
                table: "FoxWms_PrintingProcessLines",
                newName: "IX_FoxWms_PrintingProcessLines_PrintingProcessId");

            migrationBuilder.RenameIndex(
                name: "IX_FoxWms_PrintingProcessLine_CardCode",
                table: "FoxWms_PrintingProcessLines",
                newName: "IX_FoxWms_PrintingProcessLines_CardCode");

            migrationBuilder.RenameIndex(
                name: "IX_FoxWms_PrintingProcess_ShiftLeaderId",
                table: "FoxWms_PrintingProcesses",
                newName: "IX_FoxWms_PrintingProcesses_ShiftLeaderId");

            migrationBuilder.RenameIndex(
                name: "IX_FoxWms_PrintingProcess_ModifierId",
                table: "FoxWms_PrintingProcesses",
                newName: "IX_FoxWms_PrintingProcesses_ModifierId");

            migrationBuilder.RenameIndex(
                name: "IX_FoxWms_PrintingProcess_CreatorId",
                table: "FoxWms_PrintingProcesses",
                newName: "IX_FoxWms_PrintingProcesses_CreatorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FoxWms_PrintingProcessLines",
                table: "FoxWms_PrintingProcessLines",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FoxWms_PrintingProcesses",
                table: "FoxWms_PrintingProcesses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_PrintingProcesses_OHEM_ShiftLeaderId",
                table: "FoxWms_PrintingProcesses",
                column: "ShiftLeaderId",
                principalTable: "OHEM",
                principalColumn: "empID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_PrintingProcesses_OUSR_CreatorId",
                table: "FoxWms_PrintingProcesses",
                column: "CreatorId",
                principalTable: "OUSR",
                principalColumn: "USERID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_PrintingProcesses_OUSR_ModifierId",
                table: "FoxWms_PrintingProcesses",
                column: "ModifierId",
                principalTable: "OUSR",
                principalColumn: "USERID");

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_PrintingProcessLines_FoxWms_PrintingProcesses_PrintingProcessId",
                table: "FoxWms_PrintingProcessLines",
                column: "PrintingProcessId",
                principalTable: "FoxWms_PrintingProcesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_PrintingProcessLines_OCRD_CardCode",
                table: "FoxWms_PrintingProcessLines",
                column: "CardCode",
                principalTable: "OCRD",
                principalColumn: "CardCode");

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_PrintingProcessLines_OHEM_WorkerId",
                table: "FoxWms_PrintingProcessLines",
                column: "WorkerId",
                principalTable: "OHEM",
                principalColumn: "empID");
        }
    }
}
