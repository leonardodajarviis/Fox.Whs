using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fox.Whs.Migrations
{
    /// <inheritdoc />
    public partial class HelloBro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_BlowingProcessLine_OCRD_CardCode",
                table: "FoxWms_BlowingProcessLine");

            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_CuttingProcessLine_OCRD_CardCode",
                table: "FoxWms_CuttingProcessLine");

            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_GrainMixingBlowingProcessLine_OCRD_CardCode",
                table: "FoxWms_GrainMixingBlowingProcessLine");

            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_GrainMixingProcessLine_OCRD_CardCode",
                table: "FoxWms_GrainMixingProcessLine");

            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_PrintingProcessLine_OCRD_CardCode",
                table: "FoxWms_PrintingProcessLine");

            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_RewindingProcessLine_OCRD_CardCode",
                table: "FoxWms_RewindingProcessLine");

            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_SlittingProcessLine_OCRD_CardCode",
                table: "FoxWms_SlittingProcessLine");

            migrationBuilder.DropIndex(
                name: "IX_FoxWms_SlittingProcessLine_CardCode",
                table: "FoxWms_SlittingProcessLine");

            migrationBuilder.DropIndex(
                name: "IX_FoxWms_RewindingProcessLine_CardCode",
                table: "FoxWms_RewindingProcessLine");

            migrationBuilder.DropIndex(
                name: "IX_FoxWms_PrintingProcessLine_CardCode",
                table: "FoxWms_PrintingProcessLine");

            migrationBuilder.DropIndex(
                name: "IX_FoxWms_GrainMixingProcessLine_CardCode",
                table: "FoxWms_GrainMixingProcessLine");

            migrationBuilder.DropIndex(
                name: "IX_FoxWms_GrainMixingBlowingProcessLine_CardCode",
                table: "FoxWms_GrainMixingBlowingProcessLine");

            migrationBuilder.DropIndex(
                name: "IX_FoxWms_CuttingProcessLine_CardCode",
                table: "FoxWms_CuttingProcessLine");

            migrationBuilder.DropIndex(
                name: "IX_FoxWms_BlowingProcessLine_CardCode",
                table: "FoxWms_BlowingProcessLine");

            migrationBuilder.RenameColumn(
                name: "OrginalWorkerName",
                table: "FoxWms_SlittingProcessLine",
                newName: "WorkerOriginalName");

            migrationBuilder.RenameColumn(
                name: "OrginalWorkerName",
                table: "FoxWms_RewindingProcessLine",
                newName: "WorkerOriginalName");

            migrationBuilder.RenameColumn(
                name: "OrginalWorkerName",
                table: "FoxWms_PrintingProcessLine",
                newName: "WorkerOriginalName");

            migrationBuilder.RenameColumn(
                name: "OrginalWorkerName",
                table: "FoxWms_GrainMixingProcessLine",
                newName: "WorkerOriginalName");

            migrationBuilder.RenameColumn(
                name: "OrginalWorkerName",
                table: "FoxWms_GrainMixingBlowingProcessLine",
                newName: "WorkerOriginalName");

            migrationBuilder.RenameColumn(
                name: "OrginalWorkerName",
                table: "FoxWms_CuttingProcessLine",
                newName: "WorkerOriginalName");

            migrationBuilder.RenameColumn(
                name: "OrginalWorkerName",
                table: "FoxWms_BlowingProcessLine",
                newName: "WorkerOriginalName");

            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "FoxWms_SlittingProcessLine",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "FoxWms_RewindingProcessLine",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "FoxWms_PrintingProcessLine",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "FoxWms_GrainMixingProcessLine",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "FoxWms_GrainMixingBlowingProcessLine",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "FoxWms_CuttingProcessLine",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "FoxWms_BlowingProcessLine",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "FoxWms_SlittingProcessLine");

            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "FoxWms_RewindingProcessLine");

            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "FoxWms_PrintingProcessLine");

            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "FoxWms_GrainMixingProcessLine");

            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "FoxWms_GrainMixingBlowingProcessLine");

            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "FoxWms_CuttingProcessLine");

            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "FoxWms_BlowingProcessLine");

            migrationBuilder.RenameColumn(
                name: "WorkerOriginalName",
                table: "FoxWms_SlittingProcessLine",
                newName: "OrginalWorkerName");

            migrationBuilder.RenameColumn(
                name: "WorkerOriginalName",
                table: "FoxWms_RewindingProcessLine",
                newName: "OrginalWorkerName");

            migrationBuilder.RenameColumn(
                name: "WorkerOriginalName",
                table: "FoxWms_PrintingProcessLine",
                newName: "OrginalWorkerName");

            migrationBuilder.RenameColumn(
                name: "WorkerOriginalName",
                table: "FoxWms_GrainMixingProcessLine",
                newName: "OrginalWorkerName");

            migrationBuilder.RenameColumn(
                name: "WorkerOriginalName",
                table: "FoxWms_GrainMixingBlowingProcessLine",
                newName: "OrginalWorkerName");

            migrationBuilder.RenameColumn(
                name: "WorkerOriginalName",
                table: "FoxWms_CuttingProcessLine",
                newName: "OrginalWorkerName");

            migrationBuilder.RenameColumn(
                name: "WorkerOriginalName",
                table: "FoxWms_BlowingProcessLine",
                newName: "OrginalWorkerName");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_SlittingProcessLine_CardCode",
                table: "FoxWms_SlittingProcessLine",
                column: "CardCode");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_RewindingProcessLine_CardCode",
                table: "FoxWms_RewindingProcessLine",
                column: "CardCode");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_PrintingProcessLine_CardCode",
                table: "FoxWms_PrintingProcessLine",
                column: "CardCode");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_GrainMixingProcessLine_CardCode",
                table: "FoxWms_GrainMixingProcessLine",
                column: "CardCode");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_GrainMixingBlowingProcessLine_CardCode",
                table: "FoxWms_GrainMixingBlowingProcessLine",
                column: "CardCode");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_CuttingProcessLine_CardCode",
                table: "FoxWms_CuttingProcessLine",
                column: "CardCode");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_BlowingProcessLine_CardCode",
                table: "FoxWms_BlowingProcessLine",
                column: "CardCode");

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_BlowingProcessLine_OCRD_CardCode",
                table: "FoxWms_BlowingProcessLine",
                column: "CardCode",
                principalTable: "OCRD",
                principalColumn: "CardCode");

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_CuttingProcessLine_OCRD_CardCode",
                table: "FoxWms_CuttingProcessLine",
                column: "CardCode",
                principalTable: "OCRD",
                principalColumn: "CardCode");

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_GrainMixingBlowingProcessLine_OCRD_CardCode",
                table: "FoxWms_GrainMixingBlowingProcessLine",
                column: "CardCode",
                principalTable: "OCRD",
                principalColumn: "CardCode");

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_GrainMixingProcessLine_OCRD_CardCode",
                table: "FoxWms_GrainMixingProcessLine",
                column: "CardCode",
                principalTable: "OCRD",
                principalColumn: "CardCode");

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_PrintingProcessLine_OCRD_CardCode",
                table: "FoxWms_PrintingProcessLine",
                column: "CardCode",
                principalTable: "OCRD",
                principalColumn: "CardCode");

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_RewindingProcessLine_OCRD_CardCode",
                table: "FoxWms_RewindingProcessLine",
                column: "CardCode",
                principalTable: "OCRD",
                principalColumn: "CardCode");

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_SlittingProcessLine_OCRD_CardCode",
                table: "FoxWms_SlittingProcessLine",
                column: "CardCode",
                principalTable: "OCRD",
                principalColumn: "CardCode");
        }
    }
}
