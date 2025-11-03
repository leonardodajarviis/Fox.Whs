using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fox.Whs.Migrations
{
    /// <inheritdoc />
    public partial class PhatCuoiNao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_CuttingProcesses_OHEM_ShiftLeaderId",
                table: "FoxWms_CuttingProcesses");

            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_CuttingProcesses_OUSR_CreatorId",
                table: "FoxWms_CuttingProcesses");

            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_CuttingProcesses_OUSR_ModifierId",
                table: "FoxWms_CuttingProcesses");

            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_CuttingProcessLine_FoxWms_CuttingProcesses_CuttingProcessId",
                table: "FoxWms_CuttingProcessLine");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FoxWms_CuttingProcesses",
                table: "FoxWms_CuttingProcesses");

            migrationBuilder.RenameTable(
                name: "FoxWms_CuttingProcesses",
                newName: "FoxWms_CuttingProcess");

            migrationBuilder.RenameIndex(
                name: "IX_FoxWms_CuttingProcesses_ShiftLeaderId",
                table: "FoxWms_CuttingProcess",
                newName: "IX_FoxWms_CuttingProcess_ShiftLeaderId");

            migrationBuilder.RenameIndex(
                name: "IX_FoxWms_CuttingProcesses_ModifierId",
                table: "FoxWms_CuttingProcess",
                newName: "IX_FoxWms_CuttingProcess_ModifierId");

            migrationBuilder.RenameIndex(
                name: "IX_FoxWms_CuttingProcesses_CreatorId",
                table: "FoxWms_CuttingProcess",
                newName: "IX_FoxWms_CuttingProcess_CreatorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FoxWms_CuttingProcess",
                table: "FoxWms_CuttingProcess",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_CuttingProcess_OHEM_ShiftLeaderId",
                table: "FoxWms_CuttingProcess",
                column: "ShiftLeaderId",
                principalTable: "OHEM",
                principalColumn: "empID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_CuttingProcess_OUSR_CreatorId",
                table: "FoxWms_CuttingProcess",
                column: "CreatorId",
                principalTable: "OUSR",
                principalColumn: "USERID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_CuttingProcess_OUSR_ModifierId",
                table: "FoxWms_CuttingProcess",
                column: "ModifierId",
                principalTable: "OUSR",
                principalColumn: "USERID");

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_CuttingProcessLine_FoxWms_CuttingProcess_CuttingProcessId",
                table: "FoxWms_CuttingProcessLine",
                column: "CuttingProcessId",
                principalTable: "FoxWms_CuttingProcess",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_CuttingProcess_OHEM_ShiftLeaderId",
                table: "FoxWms_CuttingProcess");

            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_CuttingProcess_OUSR_CreatorId",
                table: "FoxWms_CuttingProcess");

            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_CuttingProcess_OUSR_ModifierId",
                table: "FoxWms_CuttingProcess");

            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_CuttingProcessLine_FoxWms_CuttingProcess_CuttingProcessId",
                table: "FoxWms_CuttingProcessLine");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FoxWms_CuttingProcess",
                table: "FoxWms_CuttingProcess");

            migrationBuilder.RenameTable(
                name: "FoxWms_CuttingProcess",
                newName: "FoxWms_CuttingProcesses");

            migrationBuilder.RenameIndex(
                name: "IX_FoxWms_CuttingProcess_ShiftLeaderId",
                table: "FoxWms_CuttingProcesses",
                newName: "IX_FoxWms_CuttingProcesses_ShiftLeaderId");

            migrationBuilder.RenameIndex(
                name: "IX_FoxWms_CuttingProcess_ModifierId",
                table: "FoxWms_CuttingProcesses",
                newName: "IX_FoxWms_CuttingProcesses_ModifierId");

            migrationBuilder.RenameIndex(
                name: "IX_FoxWms_CuttingProcess_CreatorId",
                table: "FoxWms_CuttingProcesses",
                newName: "IX_FoxWms_CuttingProcesses_CreatorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FoxWms_CuttingProcesses",
                table: "FoxWms_CuttingProcesses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_CuttingProcesses_OHEM_ShiftLeaderId",
                table: "FoxWms_CuttingProcesses",
                column: "ShiftLeaderId",
                principalTable: "OHEM",
                principalColumn: "empID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_CuttingProcesses_OUSR_CreatorId",
                table: "FoxWms_CuttingProcesses",
                column: "CreatorId",
                principalTable: "OUSR",
                principalColumn: "USERID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_CuttingProcesses_OUSR_ModifierId",
                table: "FoxWms_CuttingProcesses",
                column: "ModifierId",
                principalTable: "OUSR",
                principalColumn: "USERID");

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_CuttingProcessLine_FoxWms_CuttingProcesses_CuttingProcessId",
                table: "FoxWms_CuttingProcessLine",
                column: "CuttingProcessId",
                principalTable: "FoxWms_CuttingProcesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
