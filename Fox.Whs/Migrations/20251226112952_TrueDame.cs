using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fox.Whs.Migrations
{
    /// <inheritdoc />
    public partial class TrueDame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_BlowingProcess_OHEM_ShiftLeaderId",
                table: "FoxWms_BlowingProcess");

            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_CuttingProcess_OHEM_ShiftLeaderId",
                table: "FoxWms_CuttingProcess");

            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_PrintingProcess_OHEM_ShiftLeaderId",
                table: "FoxWms_PrintingProcess");

            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_RewindingProcess_OHEM_ShiftLeaderId",
                table: "FoxWms_RewindingProcess");

            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_SlittingProcess_OHEM_ShiftLeaderId",
                table: "FoxWms_SlittingProcess");

            migrationBuilder.AddColumn<string>(
                name: "OrginalWorkerName",
                table: "FoxWms_SlittingProcessLine",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ShiftLeaderId",
                table: "FoxWms_SlittingProcess",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "ShiftLeaderOriginalName",
                table: "FoxWms_SlittingProcess",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrginalWorkerName",
                table: "FoxWms_RewindingProcessLine",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ShiftLeaderId",
                table: "FoxWms_RewindingProcess",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "ShiftLeaderOriginalName",
                table: "FoxWms_RewindingProcess",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrginalWorkerName",
                table: "FoxWms_PrintingProcessLine",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ShiftLeaderId",
                table: "FoxWms_PrintingProcess",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "ShiftLeaderOriginalName",
                table: "FoxWms_PrintingProcess",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrginalWorkerName",
                table: "FoxWms_GrainMixingProcessLine",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrginalWorkerName",
                table: "FoxWms_GrainMixingBlowingProcessLine",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrginalWorkerName",
                table: "FoxWms_CuttingProcessLine",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ShiftLeaderId",
                table: "FoxWms_CuttingProcess",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "ShiftLeaderOriginalName",
                table: "FoxWms_CuttingProcess",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrginalWorkerName",
                table: "FoxWms_BlowingProcessLine",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ShiftLeaderId",
                table: "FoxWms_BlowingProcess",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "ShiftLeaderOriginalName",
                table: "FoxWms_BlowingProcess",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_BlowingProcess_OHEM_ShiftLeaderId",
                table: "FoxWms_BlowingProcess",
                column: "ShiftLeaderId",
                principalTable: "OHEM",
                principalColumn: "empID");

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_CuttingProcess_OHEM_ShiftLeaderId",
                table: "FoxWms_CuttingProcess",
                column: "ShiftLeaderId",
                principalTable: "OHEM",
                principalColumn: "empID");

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_PrintingProcess_OHEM_ShiftLeaderId",
                table: "FoxWms_PrintingProcess",
                column: "ShiftLeaderId",
                principalTable: "OHEM",
                principalColumn: "empID");

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_RewindingProcess_OHEM_ShiftLeaderId",
                table: "FoxWms_RewindingProcess",
                column: "ShiftLeaderId",
                principalTable: "OHEM",
                principalColumn: "empID");

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_SlittingProcess_OHEM_ShiftLeaderId",
                table: "FoxWms_SlittingProcess",
                column: "ShiftLeaderId",
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
                name: "FK_FoxWms_CuttingProcess_OHEM_ShiftLeaderId",
                table: "FoxWms_CuttingProcess");

            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_PrintingProcess_OHEM_ShiftLeaderId",
                table: "FoxWms_PrintingProcess");

            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_RewindingProcess_OHEM_ShiftLeaderId",
                table: "FoxWms_RewindingProcess");

            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_SlittingProcess_OHEM_ShiftLeaderId",
                table: "FoxWms_SlittingProcess");

            migrationBuilder.DropColumn(
                name: "OrginalWorkerName",
                table: "FoxWms_SlittingProcessLine");

            migrationBuilder.DropColumn(
                name: "ShiftLeaderOriginalName",
                table: "FoxWms_SlittingProcess");

            migrationBuilder.DropColumn(
                name: "OrginalWorkerName",
                table: "FoxWms_RewindingProcessLine");

            migrationBuilder.DropColumn(
                name: "ShiftLeaderOriginalName",
                table: "FoxWms_RewindingProcess");

            migrationBuilder.DropColumn(
                name: "OrginalWorkerName",
                table: "FoxWms_PrintingProcessLine");

            migrationBuilder.DropColumn(
                name: "ShiftLeaderOriginalName",
                table: "FoxWms_PrintingProcess");

            migrationBuilder.DropColumn(
                name: "OrginalWorkerName",
                table: "FoxWms_GrainMixingProcessLine");

            migrationBuilder.DropColumn(
                name: "OrginalWorkerName",
                table: "FoxWms_GrainMixingBlowingProcessLine");

            migrationBuilder.DropColumn(
                name: "OrginalWorkerName",
                table: "FoxWms_CuttingProcessLine");

            migrationBuilder.DropColumn(
                name: "ShiftLeaderOriginalName",
                table: "FoxWms_CuttingProcess");

            migrationBuilder.DropColumn(
                name: "OrginalWorkerName",
                table: "FoxWms_BlowingProcessLine");

            migrationBuilder.DropColumn(
                name: "ShiftLeaderOriginalName",
                table: "FoxWms_BlowingProcess");

            migrationBuilder.AlterColumn<int>(
                name: "ShiftLeaderId",
                table: "FoxWms_SlittingProcess",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ShiftLeaderId",
                table: "FoxWms_RewindingProcess",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ShiftLeaderId",
                table: "FoxWms_PrintingProcess",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ShiftLeaderId",
                table: "FoxWms_CuttingProcess",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ShiftLeaderId",
                table: "FoxWms_BlowingProcess",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_BlowingProcess_OHEM_ShiftLeaderId",
                table: "FoxWms_BlowingProcess",
                column: "ShiftLeaderId",
                principalTable: "OHEM",
                principalColumn: "empID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_CuttingProcess_OHEM_ShiftLeaderId",
                table: "FoxWms_CuttingProcess",
                column: "ShiftLeaderId",
                principalTable: "OHEM",
                principalColumn: "empID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_PrintingProcess_OHEM_ShiftLeaderId",
                table: "FoxWms_PrintingProcess",
                column: "ShiftLeaderId",
                principalTable: "OHEM",
                principalColumn: "empID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_RewindingProcess_OHEM_ShiftLeaderId",
                table: "FoxWms_RewindingProcess",
                column: "ShiftLeaderId",
                principalTable: "OHEM",
                principalColumn: "empID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_SlittingProcess_OHEM_ShiftLeaderId",
                table: "FoxWms_SlittingProcess",
                column: "ShiftLeaderId",
                principalTable: "OHEM",
                principalColumn: "empID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
