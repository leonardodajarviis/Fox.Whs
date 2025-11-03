using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fox.Whs.Migrations
{
    /// <inheritdoc />
    public partial class Hiruka : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "FoxWms_PrintingProcesses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<short>(
                name: "CreatorId",
                table: "FoxWms_PrintingProcesses",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "FoxWms_PrintingProcesses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "ModifierId",
                table: "FoxWms_PrintingProcesses",
                type: "smallint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FoxWms_CuttingProcesses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShiftLeaderId = table.Column<int>(type: "int", nullable: false),
                    IsDraft = table.Column<bool>(type: "bit", nullable: false),
                    ProductionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductionShift = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalCuttingOutput = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    TotalFoldedCount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    TotalProcessingMold = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    CreatorId = table.Column<short>(type: "smallint", nullable: false),
                    ModifierId = table.Column<short>(type: "smallint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoxWms_CuttingProcesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoxWms_CuttingProcesses_OHEM_ShiftLeaderId",
                        column: x => x.ShiftLeaderId,
                        principalTable: "OHEM",
                        principalColumn: "empID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoxWms_CuttingProcesses_OUSR_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "OUSR",
                        principalColumn: "USERID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoxWms_CuttingProcesses_OUSR_ModifierId",
                        column: x => x.ModifierId,
                        principalTable: "OUSR",
                        principalColumn: "USERID");
                });

            migrationBuilder.CreateTable(
                name: "FoxWms_CuttingProcessesLine",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductionOrderId = table.Column<int>(type: "int", nullable: false),
                    CuttingProcessId = table.Column<int>(type: "int", nullable: false),
                    ItemCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductionBatch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardCode = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    ProductType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Thickness = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SemiProductWidth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Size = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ColorCount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CuttingMachine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkerId = table.Column<int>(type: "int", nullable: true),
                    CuttingSpeed = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MachineStopMinutes = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    StopReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PieceCount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    WeightKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    BagCount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    FoldedCount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    RequiredDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    ActualCompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DelayReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcessingLossKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ProcessingLossReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BlowingLossKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    BlowingLossReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrintingLossKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    PrintingLossReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrintingMachine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransferKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    HumanLossKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    HumanLossReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MachineLossKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    MachineLossReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalLossKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    PoSurplusLess5Kg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    PoSurplusOver5Kg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    PoSurplusCutKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    BtpWarehouseConfirmed = table.Column<bool>(type: "bit", nullable: true),
                    RemainingInventoryKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoxWms_CuttingProcessesLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoxWms_CuttingProcessesLine_FoxWms_CuttingProcesses_CuttingProcessId",
                        column: x => x.CuttingProcessId,
                        principalTable: "FoxWms_CuttingProcesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoxWms_CuttingProcessesLine_OCRD_CardCode",
                        column: x => x.CardCode,
                        principalTable: "OCRD",
                        principalColumn: "CardCode");
                    table.ForeignKey(
                        name: "FK_FoxWms_CuttingProcessesLine_OHEM_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "OHEM",
                        principalColumn: "empID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_PrintingProcesses_CreatorId",
                table: "FoxWms_PrintingProcesses",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_PrintingProcesses_ModifierId",
                table: "FoxWms_PrintingProcesses",
                column: "ModifierId");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_CuttingProcesses_CreatorId",
                table: "FoxWms_CuttingProcesses",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_CuttingProcesses_ModifierId",
                table: "FoxWms_CuttingProcesses",
                column: "ModifierId");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_CuttingProcesses_ShiftLeaderId",
                table: "FoxWms_CuttingProcesses",
                column: "ShiftLeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_CuttingProcessesLine_CardCode",
                table: "FoxWms_CuttingProcessesLine",
                column: "CardCode");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_CuttingProcessesLine_CuttingProcessId",
                table: "FoxWms_CuttingProcessesLine",
                column: "CuttingProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_CuttingProcessesLine_WorkerId",
                table: "FoxWms_CuttingProcessesLine",
                column: "WorkerId");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_PrintingProcesses_OUSR_CreatorId",
                table: "FoxWms_PrintingProcesses");

            migrationBuilder.DropForeignKey(
                name: "FK_FoxWms_PrintingProcesses_OUSR_ModifierId",
                table: "FoxWms_PrintingProcesses");

            migrationBuilder.DropTable(
                name: "FoxWms_CuttingProcessesLine");

            migrationBuilder.DropTable(
                name: "FoxWms_CuttingProcesses");

            migrationBuilder.DropIndex(
                name: "IX_FoxWms_PrintingProcesses_CreatorId",
                table: "FoxWms_PrintingProcesses");

            migrationBuilder.DropIndex(
                name: "IX_FoxWms_PrintingProcesses_ModifierId",
                table: "FoxWms_PrintingProcesses");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "FoxWms_PrintingProcesses");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "FoxWms_PrintingProcesses");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "FoxWms_PrintingProcesses");

            migrationBuilder.DropColumn(
                name: "ModifierId",
                table: "FoxWms_PrintingProcesses");
        }
    }
}
