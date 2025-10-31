using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fox.Whs.Migrations
{
    /// <inheritdoc />
    public partial class Sakura : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fox_BlowingProcesses_OHEM_ShiftLeaderId",
                table: "Fox_BlowingProcesses");

            migrationBuilder.DropForeignKey(
                name: "FK_Fox_BlowingProcessLines_Fox_BlowingProcesses_BlowingProcessId",
                table: "Fox_BlowingProcessLines");

            migrationBuilder.DropForeignKey(
                name: "FK_Fox_BlowingProcessLines_OCRD_CardCode",
                table: "Fox_BlowingProcessLines");

            migrationBuilder.DropForeignKey(
                name: "FK_Fox_BlowingProcessLines_OHEM_WorkerId",
                table: "Fox_BlowingProcessLines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Fox_BlowingProcessLines",
                table: "Fox_BlowingProcessLines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Fox_BlowingProcesses",
                table: "Fox_BlowingProcesses");

            migrationBuilder.RenameTable(
                name: "Fox_BlowingProcessLines",
                newName: "FoxWms_BlowingProcessLines");

            migrationBuilder.RenameTable(
                name: "Fox_BlowingProcesses",
                newName: "FoxWms_BlowingProcesses");

            migrationBuilder.RenameIndex(
                name: "IX_Fox_BlowingProcessLines_WorkerId",
                table: "FoxWms_BlowingProcessLines",
                newName: "IX_FoxWms_BlowingProcessLines_WorkerId");

            migrationBuilder.RenameIndex(
                name: "IX_Fox_BlowingProcessLines_CardCode",
                table: "FoxWms_BlowingProcessLines",
                newName: "IX_FoxWms_BlowingProcessLines_CardCode");

            migrationBuilder.RenameIndex(
                name: "IX_Fox_BlowingProcessLines_BlowingProcessId",
                table: "FoxWms_BlowingProcessLines",
                newName: "IX_FoxWms_BlowingProcessLines_BlowingProcessId");

            migrationBuilder.RenameIndex(
                name: "IX_Fox_BlowingProcesses_ShiftLeaderId",
                table: "FoxWms_BlowingProcesses",
                newName: "IX_FoxWms_BlowingProcesses_ShiftLeaderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FoxWms_BlowingProcessLines",
                table: "FoxWms_BlowingProcessLines",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FoxWms_BlowingProcesses",
                table: "FoxWms_BlowingProcesses",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "FoxWms_PrintingProcesses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShiftLeaderId = table.Column<int>(type: "int", nullable: false),
                    IsDraft = table.Column<bool>(type: "bit", nullable: false),
                    ProductionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductionShift = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalPrintingOutput = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalProcessingMold = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalBlowingStageMold = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalPrintingStageMold = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoxWms_PrintingProcesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoxWms_PrintingProcesses_OHEM_ShiftLeaderId",
                        column: x => x.ShiftLeaderId,
                        principalTable: "OHEM",
                        principalColumn: "empID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FoxWms_PrintingProcessLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrintingProcessId = table.Column<int>(type: "int", nullable: false),
                    ProductionOrderId = table.Column<int>(type: "int", nullable: false),
                    ItemCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductionBatch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardCode = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    ProductType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Thickness = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SemiProductWidth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrintPatternName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ColorCount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrintingMachine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkerId = table.Column<int>(type: "int", nullable: true),
                    PrintingSpeed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MachineStopMinutes = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    StopReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RollCount = table.Column<int>(type: "int", nullable: false),
                    PieceCount = table.Column<int>(type: "int", nullable: false),
                    WeightKg = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    RequiredDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    ActualCompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DelayReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcessingLossKg = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ProcessingLossReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BlowingLossKg = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    BlowingLossReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OppRollHeadKg = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    OppRollHeadReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HumanLossKg = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    HumanLossReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MachineLossKg = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    MachineLossReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalLossKg = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PoSurplus = table.Column<bool>(type: "bit", nullable: false),
                    BtpWarehouseConfirmation = table.Column<bool>(type: "bit", nullable: false),
                    PrintingStageInventoryKg = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoxWms_PrintingProcessLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoxWms_PrintingProcessLines_FoxWms_PrintingProcesses_PrintingProcessId",
                        column: x => x.PrintingProcessId,
                        principalTable: "FoxWms_PrintingProcesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoxWms_PrintingProcessLines_OCRD_CardCode",
                        column: x => x.CardCode,
                        principalTable: "OCRD",
                        principalColumn: "CardCode");
                    table.ForeignKey(
                        name: "FK_FoxWms_PrintingProcessLines_OHEM_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "OHEM",
                        principalColumn: "empID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_PrintingProcesses_ShiftLeaderId",
                table: "FoxWms_PrintingProcesses",
                column: "ShiftLeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_PrintingProcessLines_CardCode",
                table: "FoxWms_PrintingProcessLines",
                column: "CardCode");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_PrintingProcessLines_PrintingProcessId",
                table: "FoxWms_PrintingProcessLines",
                column: "PrintingProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_PrintingProcessLines_WorkerId",
                table: "FoxWms_PrintingProcessLines",
                column: "WorkerId");

            migrationBuilder.AddForeignKey(
                name: "FK_FoxWms_BlowingProcesses_OHEM_ShiftLeaderId",
                table: "FoxWms_BlowingProcesses",
                column: "ShiftLeaderId",
                principalTable: "OHEM",
                principalColumn: "empID",
                onDelete: ReferentialAction.Cascade);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_BlowingProcesses_OHEM_ShiftLeaderId",
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

            migrationBuilder.DropTable(
                name: "FoxWms_PrintingProcessLines");

            migrationBuilder.DropTable(
                name: "FoxWms_PrintingProcesses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FoxWms_BlowingProcessLines",
                table: "FoxWms_BlowingProcessLines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FoxWms_BlowingProcesses",
                table: "FoxWms_BlowingProcesses");

            migrationBuilder.RenameTable(
                name: "FoxWms_BlowingProcessLines",
                newName: "Fox_BlowingProcessLines");

            migrationBuilder.RenameTable(
                name: "FoxWms_BlowingProcesses",
                newName: "Fox_BlowingProcesses");

            migrationBuilder.RenameIndex(
                name: "IX_FoxWms_BlowingProcessLines_WorkerId",
                table: "Fox_BlowingProcessLines",
                newName: "IX_Fox_BlowingProcessLines_WorkerId");

            migrationBuilder.RenameIndex(
                name: "IX_FoxWms_BlowingProcessLines_CardCode",
                table: "Fox_BlowingProcessLines",
                newName: "IX_Fox_BlowingProcessLines_CardCode");

            migrationBuilder.RenameIndex(
                name: "IX_FoxWms_BlowingProcessLines_BlowingProcessId",
                table: "Fox_BlowingProcessLines",
                newName: "IX_Fox_BlowingProcessLines_BlowingProcessId");

            migrationBuilder.RenameIndex(
                name: "IX_FoxWms_BlowingProcesses_ShiftLeaderId",
                table: "Fox_BlowingProcesses",
                newName: "IX_Fox_BlowingProcesses_ShiftLeaderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Fox_BlowingProcessLines",
                table: "Fox_BlowingProcessLines",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Fox_BlowingProcesses",
                table: "Fox_BlowingProcesses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Fox_BlowingProcesses_OHEM_ShiftLeaderId",
                table: "Fox_BlowingProcesses",
                column: "ShiftLeaderId",
                principalTable: "OHEM",
                principalColumn: "empID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Fox_BlowingProcessLines_Fox_BlowingProcesses_BlowingProcessId",
                table: "Fox_BlowingProcessLines",
                column: "BlowingProcessId",
                principalTable: "Fox_BlowingProcesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Fox_BlowingProcessLines_OCRD_CardCode",
                table: "Fox_BlowingProcessLines",
                column: "CardCode",
                principalTable: "OCRD",
                principalColumn: "CardCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Fox_BlowingProcessLines_OHEM_WorkerId",
                table: "Fox_BlowingProcessLines",
                column: "WorkerId",
                principalTable: "OHEM",
                principalColumn: "empID");
        }
    }
}
