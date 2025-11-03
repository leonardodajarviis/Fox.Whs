using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fox.Whs.Migrations
{
    /// <inheritdoc />
    public partial class AnhKienDz : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_BlowingProcesses_OHEM_ShiftLeaderId",
                table: "FoxWms_BlowingProcesses");

            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_BlowingProcesses_OUSR_CreatorId",
                table: "FoxWms_BlowingProcesses");

            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_BlowingProcesses_OUSR_ModifierId",
                table: "FoxWms_BlowingProcesses");

            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_BlowingProcessLines_FoxWms_BlowingProcesses_BlowingProcessId",
                table: "FoxWms_BlowingProcessLines");

            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_BlowingProcessLines_OCRD_CardCode",
                table: "FoxWms_BlowingProcessLines");

            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_BlowingProcessLines_OHEM_WorkerId",
                table: "FoxWms_BlowingProcessLines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FoxWms_BlowingProcessLines",
                table: "FoxWms_BlowingProcessLines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FoxWms_BlowingProcesses",
                table: "FoxWms_BlowingProcesses");

            migrationBuilder.RenameTable(
                name: "FoxWms_BlowingProcessLines",
                newName: "FoxWms_BlowingProcessLine");

            migrationBuilder.RenameTable(
                name: "FoxWms_BlowingProcesses",
                newName: "FoxWms_BlowingProcess");

            migrationBuilder.RenameIndex(
                name: "IX_FoxWms_BlowingProcessLines_WorkerId",
                table: "FoxWms_BlowingProcessLine",
                newName: "IX_FoxWms_BlowingProcessLine_WorkerId");

            migrationBuilder.RenameIndex(
                name: "IX_FoxWms_BlowingProcessLines_CardCode",
                table: "FoxWms_BlowingProcessLine",
                newName: "IX_FoxWms_BlowingProcessLine_CardCode");

            migrationBuilder.RenameIndex(
                name: "IX_FoxWms_BlowingProcessLines_BlowingProcessId",
                table: "FoxWms_BlowingProcessLine",
                newName: "IX_FoxWms_BlowingProcessLine_BlowingProcessId");

            migrationBuilder.RenameIndex(
                name: "IX_FoxWms_BlowingProcesses_ShiftLeaderId",
                table: "FoxWms_BlowingProcess",
                newName: "IX_FoxWms_BlowingProcess_ShiftLeaderId");

            migrationBuilder.RenameIndex(
                name: "IX_FoxWms_BlowingProcesses_ModifierId",
                table: "FoxWms_BlowingProcess",
                newName: "IX_FoxWms_BlowingProcess_ModifierId");

            migrationBuilder.RenameIndex(
                name: "IX_FoxWms_BlowingProcesses_CreatorId",
                table: "FoxWms_BlowingProcess",
                newName: "IX_FoxWms_BlowingProcess_CreatorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FoxWms_BlowingProcessLine",
                table: "FoxWms_BlowingProcessLine",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FoxWms_BlowingProcess",
                table: "FoxWms_BlowingProcess",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_BlowingProcess_OHEM_ShiftLeaderId",
                table: "FoxWms_BlowingProcess",
                column: "ShiftLeaderId",
                principalTable: "OHEM",
                principalColumn: "empID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_BlowingProcess_OUSR_CreatorId",
                table: "FoxWms_BlowingProcess",
                column: "CreatorId",
                principalTable: "OUSR",
                principalColumn: "USERID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_BlowingProcess_OUSR_ModifierId",
                table: "FoxWms_BlowingProcess",
                column: "ModifierId",
                principalTable: "OUSR",
                principalColumn: "USERID");

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_BlowingProcessLine_FoxWms_BlowingProcess_BlowingProcessId",
                table: "FoxWms_BlowingProcessLine",
                column: "BlowingProcessId",
                principalTable: "FoxWms_BlowingProcess",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_BlowingProcessLine_OCRD_CardCode",
                table: "FoxWms_BlowingProcessLine",
                column: "CardCode",
                principalTable: "OCRD",
                principalColumn: "CardCode");

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_BlowingProcessLine_OHEM_WorkerId",
                table: "FoxWms_BlowingProcessLine",
                column: "WorkerId",
                principalTable: "OHEM",
                principalColumn: "empID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_BlowingProcess_OHEM_ShiftLeaderId",
                table: "FoxWms_BlowingProcess");

            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_BlowingProcess_OUSR_CreatorId",
                table: "FoxWms_BlowingProcess");

            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_BlowingProcess_OUSR_ModifierId",
                table: "FoxWms_BlowingProcess");

            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_BlowingProcessLine_FoxWms_BlowingProcess_BlowingProcessId",
                table: "FoxWms_BlowingProcessLine");

            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_BlowingProcessLine_OCRD_CardCode",
                table: "FoxWms_BlowingProcessLine");

            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_BlowingProcessLine_OHEM_WorkerId",
                table: "FoxWms_BlowingProcessLine");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FoxWms_BlowingProcessLine",
                table: "FoxWms_BlowingProcessLine");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FoxWms_BlowingProcess",
                table: "FoxWms_BlowingProcess");

            migrationBuilder.RenameTable(
                name: "FoxWms_BlowingProcessLine",
                newName: "FoxWms_BlowingProcessLines");

            migrationBuilder.RenameTable(
                name: "FoxWms_BlowingProcess",
                newName: "FoxWms_BlowingProcesses");

            migrationBuilder.RenameIndex(
                name: "IX_FoxWms_BlowingProcessLine_WorkerId",
                table: "FoxWms_BlowingProcessLines",
                newName: "IX_FoxWms_BlowingProcessLines_WorkerId");

            migrationBuilder.RenameIndex(
                name: "IX_FoxWms_BlowingProcessLine_CardCode",
                table: "FoxWms_BlowingProcessLines",
                newName: "IX_FoxWms_BlowingProcessLines_CardCode");

            migrationBuilder.RenameIndex(
                name: "IX_FoxWms_BlowingProcessLine_BlowingProcessId",
                table: "FoxWms_BlowingProcessLines",
                newName: "IX_FoxWms_BlowingProcessLines_BlowingProcessId");

            migrationBuilder.RenameIndex(
                name: "IX_FoxWms_BlowingProcess_ShiftLeaderId",
                table: "FoxWms_BlowingProcesses",
                newName: "IX_FoxWms_BlowingProcesses_ShiftLeaderId");

            migrationBuilder.RenameIndex(
                name: "IX_FoxWms_BlowingProcess_ModifierId",
                table: "FoxWms_BlowingProcesses",
                newName: "IX_FoxWms_BlowingProcesses_ModifierId");

            migrationBuilder.RenameIndex(
                name: "IX_FoxWms_BlowingProcess_CreatorId",
                table: "FoxWms_BlowingProcesses",
                newName: "IX_FoxWms_BlowingProcesses_CreatorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FoxWms_BlowingProcessLines",
                table: "FoxWms_BlowingProcessLines",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FoxWms_BlowingProcesses",
                table: "FoxWms_BlowingProcesses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_BlowingProcesses_OHEM_ShiftLeaderId",
                table: "FoxWms_BlowingProcesses",
                column: "ShiftLeaderId",
                principalTable: "OHEM",
                principalColumn: "empID",
                onDelete: ReferentialAction.Cascade);

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

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_BlowingProcessLines_FoxWms_BlowingProcesses_BlowingProcessId",
                table: "FoxWms_BlowingProcessLines",
                column: "BlowingProcessId",
                principalTable: "FoxWms_BlowingProcesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_BlowingProcessLines_OCRD_CardCode",
                table: "FoxWms_BlowingProcessLines",
                column: "CardCode",
                principalTable: "OCRD",
                principalColumn: "CardCode");

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_BlowingProcessLines_OHEM_WorkerId",
                table: "FoxWms_BlowingProcessLines",
                column: "WorkerId",
                principalTable: "OHEM",
                principalColumn: "empID");
        }
    }
}
