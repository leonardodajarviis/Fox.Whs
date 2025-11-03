using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fox.Whs.Migrations
{
    /// <inheritdoc />
    public partial class AnhKienViDai : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TotalRewindingOutput",
                table: "FoxWms_BlowingProcess",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalReservedOutput",
                table: "FoxWms_BlowingProcess",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalBlowingOutput",
                table: "FoxWms_BlowingProcess",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalBlowingLoss",
                table: "FoxWms_BlowingProcess",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.CreateTable(
                name: "FoxWms_RewindingProcess",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShiftLeaderId = table.Column<int>(type: "int", nullable: false),
                    IsDraft = table.Column<bool>(type: "bit", nullable: false),
                    ProductionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductionShift = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalRewindingOutput = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    TotalBlowingStageMold = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    TotalRewindingStageMold = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoxWms_RewindingProcess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoxWms_RewindingProcess_OHEM_ShiftLeaderId",
                        column: x => x.ShiftLeaderId,
                        principalTable: "OHEM",
                        principalColumn: "empID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FoxWms_SlittingProcess",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShiftLeaderId = table.Column<int>(type: "int", nullable: false),
                    IsDraft = table.Column<bool>(type: "bit", nullable: false),
                    ProductionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductionShift = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalProcessingMold = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    TotalBlowingStageMold = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    TotalPrintingStageMold = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    TotalSlittingStageMold = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    CreatorId = table.Column<short>(type: "smallint", nullable: false),
                    ModifierId = table.Column<short>(type: "smallint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoxWms_SlittingProcess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoxWms_SlittingProcess_OHEM_ShiftLeaderId",
                        column: x => x.ShiftLeaderId,
                        principalTable: "OHEM",
                        principalColumn: "empID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoxWms_SlittingProcess_OUSR_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "OUSR",
                        principalColumn: "USERID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoxWms_SlittingProcess_OUSR_ModifierId",
                        column: x => x.ModifierId,
                        principalTable: "OUSR",
                        principalColumn: "USERID");
                });

            migrationBuilder.CreateTable(
                name: "FoxWms_RewindingProcessLine",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductionOrderId = table.Column<int>(type: "int", nullable: false),
                    RewindingProcessId = table.Column<int>(type: "int", nullable: false),
                    ItemCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductionBatch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardCode = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    ProductType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Thickness = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SemiProductWidth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RewindingMachine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkerId = table.Column<int>(type: "int", nullable: true),
                    RewindingSpeed = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MachineStopMinutes = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    StopReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RollCount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    WeightKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    BoxCount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    RequiredDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    ActualCompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DelayReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BlowingLossKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    BlowingLossReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HumanLossKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    HumanLossReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MachineLossKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    MachineLossReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalLossKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    PoSurplusKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    BtpWarehouseConfirmed = table.Column<bool>(type: "bit", precision: 18, scale: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoxWms_RewindingProcessLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoxWms_RewindingProcessLine_FoxWms_RewindingProcess_RewindingProcessId",
                        column: x => x.RewindingProcessId,
                        principalTable: "FoxWms_RewindingProcess",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoxWms_RewindingProcessLine_OCRD_CardCode",
                        column: x => x.CardCode,
                        principalTable: "OCRD",
                        principalColumn: "CardCode");
                    table.ForeignKey(
                        name: "FK_FoxWms_RewindingProcessLine_OHEM_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "OHEM",
                        principalColumn: "empID");
                });

            migrationBuilder.CreateTable(
                name: "FoxWms_SlittingProcessLine",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductionOrderId = table.Column<int>(type: "int", nullable: false),
                    SlittingProcessId = table.Column<int>(type: "int", nullable: false),
                    ItemCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductionBatch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardCode = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    ProductType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Thickness = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SemiProductWidth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrintPatternName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ColorCount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SlittingMachine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkerId = table.Column<int>(type: "int", nullable: true),
                    SlittingSpeed = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MachineStopMinutes = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    StopReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RollCount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    PieceCount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    WeightKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    BoxCount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
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
                    CutViaKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    HumanLossKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    HumanLossReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MachineLossKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    MachineLossReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalLossKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    PoSurplusBtpInKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    PoSurplusTpSlittingKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    BtpWarehouseConfirmed = table.Column<bool>(type: "bit", precision: 18, scale: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoxWms_SlittingProcessLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoxWms_SlittingProcessLine_FoxWms_SlittingProcess_SlittingProcessId",
                        column: x => x.SlittingProcessId,
                        principalTable: "FoxWms_SlittingProcess",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoxWms_SlittingProcessLine_OCRD_CardCode",
                        column: x => x.CardCode,
                        principalTable: "OCRD",
                        principalColumn: "CardCode");
                    table.ForeignKey(
                        name: "FK_FoxWms_SlittingProcessLine_OHEM_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "OHEM",
                        principalColumn: "empID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_RewindingProcess_ShiftLeaderId",
                table: "FoxWms_RewindingProcess",
                column: "ShiftLeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_RewindingProcessLine_CardCode",
                table: "FoxWms_RewindingProcessLine",
                column: "CardCode");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_RewindingProcessLine_RewindingProcessId",
                table: "FoxWms_RewindingProcessLine",
                column: "RewindingProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_RewindingProcessLine_WorkerId",
                table: "FoxWms_RewindingProcessLine",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_SlittingProcess_CreatorId",
                table: "FoxWms_SlittingProcess",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_SlittingProcess_ModifierId",
                table: "FoxWms_SlittingProcess",
                column: "ModifierId");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_SlittingProcess_ShiftLeaderId",
                table: "FoxWms_SlittingProcess",
                column: "ShiftLeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_SlittingProcessLine_CardCode",
                table: "FoxWms_SlittingProcessLine",
                column: "CardCode");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_SlittingProcessLine_SlittingProcessId",
                table: "FoxWms_SlittingProcessLine",
                column: "SlittingProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_SlittingProcessLine_WorkerId",
                table: "FoxWms_SlittingProcessLine",
                column: "WorkerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoxWms_RewindingProcessLine");

            migrationBuilder.DropTable(
                name: "FoxWms_SlittingProcessLine");

            migrationBuilder.DropTable(
                name: "FoxWms_RewindingProcess");

            migrationBuilder.DropTable(
                name: "FoxWms_SlittingProcess");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalRewindingOutput",
                table: "FoxWms_BlowingProcess",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldPrecision: 18,
                oldScale: 4);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalReservedOutput",
                table: "FoxWms_BlowingProcess",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldPrecision: 18,
                oldScale: 4);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalBlowingOutput",
                table: "FoxWms_BlowingProcess",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldPrecision: 18,
                oldScale: 4);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalBlowingLoss",
                table: "FoxWms_BlowingProcess",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldPrecision: 18,
                oldScale: 4);
        }
    }
}
