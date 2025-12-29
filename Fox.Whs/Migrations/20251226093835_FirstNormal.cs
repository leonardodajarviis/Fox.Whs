using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fox.Whs.Migrations
{
    /// <inheritdoc />
    public partial class FirstNormal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FoxWms_BlowingProcess",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShiftLeaderId = table.Column<int>(type: "int", nullable: false),
                    IsDraft = table.Column<bool>(type: "bit", nullable: false),
                    ProductionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductionShift = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalBlowingOutput = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    TotalRewindingOutput = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    TotalReservedOutput = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    TotalBlowingLoss = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ListOfWorkersText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatorId = table.Column<short>(type: "smallint", nullable: false),
                    ModifierId = table.Column<short>(type: "smallint", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoxWms_BlowingProcess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoxWms_BlowingProcess_OHEM_ShiftLeaderId",
                        column: x => x.ShiftLeaderId,
                        principalTable: "OHEM",
                        principalColumn: "empID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoxWms_BlowingProcess_OUSR_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "OUSR",
                        principalColumn: "USERID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoxWms_BlowingProcess_OUSR_ModifierId",
                        column: x => x.ModifierId,
                        principalTable: "OUSR",
                        principalColumn: "USERID");
                });

            migrationBuilder.CreateTable(
                name: "FoxWms_CuttingProcess",
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
                    TotalBlowingStageMold = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    TotalPrintingStageMold = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    TotalCuttingStageMold = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatorId = table.Column<short>(type: "smallint", nullable: false),
                    ModifierId = table.Column<short>(type: "smallint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoxWms_CuttingProcess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoxWms_CuttingProcess_OHEM_ShiftLeaderId",
                        column: x => x.ShiftLeaderId,
                        principalTable: "OHEM",
                        principalColumn: "empID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoxWms_CuttingProcess_OUSR_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "OUSR",
                        principalColumn: "USERID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoxWms_CuttingProcess_OUSR_ModifierId",
                        column: x => x.ModifierId,
                        principalTable: "OUSR",
                        principalColumn: "USERID");
                });

            migrationBuilder.CreateTable(
                name: "FoxWms_GrainMixingBlowingProcess",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDraft = table.Column<bool>(type: "bit", nullable: false),
                    ProductionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductionShift = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ItemCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BlowingMachine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BlowingMachineName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatorId = table.Column<short>(type: "smallint", nullable: false),
                    ModifierId = table.Column<short>(type: "smallint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoxWms_GrainMixingBlowingProcess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoxWms_GrainMixingBlowingProcess_OUSR_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "OUSR",
                        principalColumn: "USERID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoxWms_GrainMixingBlowingProcess_OUSR_ModifierId",
                        column: x => x.ModifierId,
                        principalTable: "OUSR",
                        principalColumn: "USERID");
                });

            migrationBuilder.CreateTable(
                name: "FoxWms_GrainMixingProcess",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDraft = table.Column<bool>(type: "bit", nullable: false),
                    ProductionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WorkerCount = table.Column<int>(type: "int", nullable: false),
                    TotalHoursWorked = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    LaborProductivity = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatorId = table.Column<short>(type: "smallint", nullable: false),
                    ModifierId = table.Column<short>(type: "smallint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoxWms_GrainMixingProcess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoxWms_GrainMixingProcess_OUSR_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "OUSR",
                        principalColumn: "USERID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoxWms_GrainMixingProcess_OUSR_ModifierId",
                        column: x => x.ModifierId,
                        principalTable: "OUSR",
                        principalColumn: "USERID");
                });

            migrationBuilder.CreateTable(
                name: "FoxWms_PrintingProcess",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShiftLeaderId = table.Column<int>(type: "int", nullable: false),
                    IsDraft = table.Column<bool>(type: "bit", nullable: false),
                    ProductionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductionShift = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalPrintingOutput = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    TotalProcessingMold = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    TotalBlowingStageMold = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    TotalPrintingStageMold = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatorId = table.Column<short>(type: "smallint", nullable: false),
                    ModifierId = table.Column<short>(type: "smallint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoxWms_PrintingProcess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoxWms_PrintingProcess_OHEM_ShiftLeaderId",
                        column: x => x.ShiftLeaderId,
                        principalTable: "OHEM",
                        principalColumn: "empID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoxWms_PrintingProcess_OUSR_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "OUSR",
                        principalColumn: "USERID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoxWms_PrintingProcess_OUSR_ModifierId",
                        column: x => x.ModifierId,
                        principalTable: "OUSR",
                        principalColumn: "USERID");
                });

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
                    TotalRewindingStageMold = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatorId = table.Column<short>(type: "smallint", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifierId = table.Column<short>(type: "smallint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                    table.ForeignKey(
                        name: "FK_FoxWms_RewindingProcess_OUSR_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "OUSR",
                        principalColumn: "USERID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoxWms_RewindingProcess_OUSR_ModifierId",
                        column: x => x.ModifierId,
                        principalTable: "OUSR",
                        principalColumn: "USERID");
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
                    TotalSlittingOutput = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    TotalProcessingMold = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    TotalBlowingStageMold = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    TotalPrintingStageMold = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    TotalSlittingStageMold = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatorId = table.Column<short>(type: "smallint", nullable: false),
                    ModifierId = table.Column<short>(type: "smallint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
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
                name: "FoxWms_UserSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<short>(type: "smallint", nullable: false),
                    AccessTokenJti = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RefreshTokenJti = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    RefreshExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RevokedReason = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoxWms_UserSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoxWms_UserSessions_OUSR_UserId",
                        column: x => x.UserId,
                        principalTable: "OUSR",
                        principalColumn: "USERID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FoxWms_BlowingProcessLine",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlowingProcessId = table.Column<int>(type: "int", nullable: false),
                    ProductionOrderId = table.Column<int>(type: "int", nullable: true),
                    ItemCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductionBatch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardCode = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    ProductType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Thickness = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SemiProductWidth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BlowingMachine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BlowingMachineName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkerId = table.Column<int>(type: "int", nullable: true),
                    BlowingSpeed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StopDurationMinutes = table.Column<int>(type: "int", nullable: false),
                    StopReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuantityRolls = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    QuantityKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    RewindOrSplitWeight = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ReservedWeight = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    RequiredDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ActualCompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DelayReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WidthChange = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    InnerCoating = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    TrimmedEdge = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ElectricalIssue = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    MaterialLossKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    MaterialLossReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HumanErrorKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    HumanErrorReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MachineErrorKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    MachineErrorReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtherErrorKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    OtherErrorReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalLoss = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ExcessPO = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    SemiProductWarehouseConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BlowingStageInventory = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoxWms_BlowingProcessLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoxWms_BlowingProcessLine_FoxWms_BlowingProcess_BlowingProcessId",
                        column: x => x.BlowingProcessId,
                        principalTable: "FoxWms_BlowingProcess",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoxWms_BlowingProcessLine_OCRD_CardCode",
                        column: x => x.CardCode,
                        principalTable: "OCRD",
                        principalColumn: "CardCode");
                    table.ForeignKey(
                        name: "FK_FoxWms_BlowingProcessLine_OHEM_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "OHEM",
                        principalColumn: "empID");
                });

            migrationBuilder.CreateTable(
                name: "FoxWms_CuttingProcessLine",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductionOrderId = table.Column<int>(type: "int", nullable: false),
                    CuttingProcessId = table.Column<int>(type: "int", nullable: false),
                    ItemCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductionBatch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardCode = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    ProductType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Thickness = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SemiProductWidth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Size = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ColorCount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CuttingMachine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CuttingMachineName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkerId = table.Column<int>(type: "int", nullable: true),
                    CuttingSpeed = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MachineStopMinutes = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    StopReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PieceCount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    QuantityKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    BagCount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    FoldedCount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    RequiredDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ActualCompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DelayReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcessingLossKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ProcessingLossReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BlowingLossKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    BlowingLossReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrintingLossKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    PrintingLossReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrintingMachine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrintingMachineName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransferKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    HumanLossKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    HumanLossReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MachineLossKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    MachineLossReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalLossKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ExcessPOLess5Kg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ExcessPOOver5Kg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ExcessPOCut = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ExcessPOPsc = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    BtpWarehouseConfirmed = table.Column<bool>(type: "bit", nullable: true),
                    RemainingInventoryKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoxWms_CuttingProcessLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoxWms_CuttingProcessLine_FoxWms_CuttingProcess_CuttingProcessId",
                        column: x => x.CuttingProcessId,
                        principalTable: "FoxWms_CuttingProcess",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoxWms_CuttingProcessLine_OCRD_CardCode",
                        column: x => x.CardCode,
                        principalTable: "OCRD",
                        principalColumn: "CardCode");
                    table.ForeignKey(
                        name: "FK_FoxWms_CuttingProcessLine_OHEM_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "OHEM",
                        principalColumn: "empID");
                });

            migrationBuilder.CreateTable(
                name: "FoxWms_GrainMixingBlowingProcessLine",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductionOrderId = table.Column<int>(type: "int", nullable: false),
                    GrainMixingBlowingProcessId = table.Column<int>(type: "int", nullable: false),
                    ProductionBatch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardCode = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    MaterialIssueVoucherNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MixtureType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Specification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkerId = table.Column<int>(type: "int", nullable: true),
                    MachineName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MixingMachine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MixingMachineName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PpTron = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PpHdNhot = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PpLdpe = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PpDc = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PpAdditive = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PpColor = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PpOther = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PpRit = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    HdLldpe2320 = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    HdRecycled = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    HdTalcol = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    HdDc = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    HdColor = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    HdOther = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    HdHd = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PeAdditive = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PeTalcol = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PeColor = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PeOther = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PeLdpe = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PeLldpe = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PeRecycled = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PeDc = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ShrinkRe707 = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ShrinkSlip = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ShrinkStatic = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ShrinkDc = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ShrinkTalcol = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ShrinkOther = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ShrinkLdpe = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ShrinkLldpe = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ShrinkRecycled = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ShrinkTangDai = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    WrapRecycledCa = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    WrapRecycledCb = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    WrapGlue = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    WrapColor = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    WrapDc = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    WrapLdpe = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    WrapLldpe = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    WrapSlip = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    WrapAdditive = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    WrapOther = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    WrapTangDaiC6 = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    WrapTangDaiC8 = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    EvaPop3070 = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    EvaLdpe = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    EvaDc = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    EvaTalcol = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    EvaSlip = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    EvaStaticAdditive = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    EvaOther = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    EvaTgc = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    QuantityKg = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    RequiredDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ActualCompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DelayReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoxWms_GrainMixingBlowingProcessLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoxWms_GrainMixingBlowingProcessLine_FoxWms_GrainMixingBlowingProcess_GrainMixingBlowingProcessId",
                        column: x => x.GrainMixingBlowingProcessId,
                        principalTable: "FoxWms_GrainMixingBlowingProcess",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoxWms_GrainMixingBlowingProcessLine_OCRD_CardCode",
                        column: x => x.CardCode,
                        principalTable: "OCRD",
                        principalColumn: "CardCode");
                    table.ForeignKey(
                        name: "FK_FoxWms_GrainMixingBlowingProcessLine_OHEM_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "OHEM",
                        principalColumn: "empID");
                });

            migrationBuilder.CreateTable(
                name: "FoxWms_GrainMixingProcessLine",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GrainMixingProcessId = table.Column<int>(type: "int", nullable: false),
                    ProductionOrderId = table.Column<int>(type: "int", nullable: false),
                    ProductionBatch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardCode = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    MaterialIssueVoucherNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MixtureType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Specification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkerId = table.Column<int>(type: "int", nullable: true),
                    MachineName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MixingMachine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MixingMachineName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PpTron = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PpHdNhot = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PpLdpe = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PpDc = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PpAdditive = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PpColor = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PpOther = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PpRit = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    HdLldpe2320 = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    HdRecycled = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    HdTalcol = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    HdDc = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    HdColor = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    HdOther = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    HdHd = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PeAdditive = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PeTalcol = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PeColor = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PeOther = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PeLdpe = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PeLldpe = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PeRecycled = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PeDc = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ShrinkRe707 = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ShrinkSlip = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ShrinkStatic = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ShrinkDc = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ShrinkTalcol = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ShrinkOther = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ShrinkLdpe = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ShrinkLldpe = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ShrinkRecycled = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ShrinkTangDai = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    WrapRecycledCa = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    WrapRecycledCb = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    WrapGlue = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    WrapColor = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    WrapDc = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    WrapLdpe = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    WrapLldpe = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    WrapSlip = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    WrapAdditive = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    WrapOther = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    WrapTangDaiC6 = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    WrapTangDaiC8 = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    EvaPop3070 = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    EvaLdpe = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    EvaDc = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    EvaTalcol = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    EvaSlip = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    EvaStaticAdditive = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    EvaOther = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    EvaTgc = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    QuantityKg = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    RequiredDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ActualCompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DelayReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoxWms_GrainMixingProcessLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoxWms_GrainMixingProcessLine_FoxWms_GrainMixingProcess_GrainMixingProcessId",
                        column: x => x.GrainMixingProcessId,
                        principalTable: "FoxWms_GrainMixingProcess",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoxWms_GrainMixingProcessLine_OCRD_CardCode",
                        column: x => x.CardCode,
                        principalTable: "OCRD",
                        principalColumn: "CardCode");
                    table.ForeignKey(
                        name: "FK_FoxWms_GrainMixingProcessLine_OHEM_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "OHEM",
                        principalColumn: "empID");
                });

            migrationBuilder.CreateTable(
                name: "FoxWms_PrintingProcessLine",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrintingProcessId = table.Column<int>(type: "int", nullable: false),
                    ProductionOrderId = table.Column<int>(type: "int", nullable: false),
                    ItemCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductionBatch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardCode = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    ProductType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Thickness = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SemiProductWidth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrintPatternName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ColorCount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrintingMachine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrintingMachineName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkerId = table.Column<int>(type: "int", nullable: true),
                    PrintingSpeed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MachineStopMinutes = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    StopReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RollCount = table.Column<int>(type: "int", nullable: false),
                    PieceCount = table.Column<int>(type: "int", nullable: false),
                    QuantityKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: true),
                    RequiredDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ActualCompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DelayReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcessingLossKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ProcessingLossReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BlowingLossKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    BlowingLossReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OppRollHeadKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    OppRollHeadReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HumanLossKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    HumanLossReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MachineLossKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    MachineLossReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalLossKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ExcessPO = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    BtpWarehouseConfirmation = table.Column<bool>(type: "bit", nullable: false),
                    PrintingStageInventoryKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoxWms_PrintingProcessLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoxWms_PrintingProcessLine_FoxWms_PrintingProcess_PrintingProcessId",
                        column: x => x.PrintingProcessId,
                        principalTable: "FoxWms_PrintingProcess",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoxWms_PrintingProcessLine_OCRD_CardCode",
                        column: x => x.CardCode,
                        principalTable: "OCRD",
                        principalColumn: "CardCode");
                    table.ForeignKey(
                        name: "FK_FoxWms_PrintingProcessLine_OHEM_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "OHEM",
                        principalColumn: "empID");
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
                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductionBatch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardCode = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    ProductType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Thickness = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SemiProductWidth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RewindingMachine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RewindingMachineName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkerId = table.Column<int>(type: "int", nullable: true),
                    RewindingSpeed = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MachineStopMinutes = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    StopReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RollCount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    QuantityKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    BoxCount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    RequiredDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ActualCompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DelayReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BlowingLossKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    BlowingLossReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HumanLossKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    HumanLossReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MachineLossKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    MachineLossReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalLossKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ExcessPO = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    BtpWarehouseConfirmed = table.Column<bool>(type: "bit", precision: 18, scale: 4, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductionBatch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardCode = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    ProductType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Thickness = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SemiProductWidth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrintPatternName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ColorCount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SlittingMachine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SlittingMachineName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkerId = table.Column<int>(type: "int", nullable: true),
                    SlittingSpeed = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MachineStopMinutes = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    StopReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RollCount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    PieceCount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    QuantityKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    BoxCount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    RequiredDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ActualCompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DelayReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcessingLossKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ProcessingLossReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BlowingLossKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    BlowingLossReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrintingLossKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    PrintingLossReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrintingMachine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrintingMachineName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CutViaKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    HumanLossKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    HumanLossReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MachineLossKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    MachineLossReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalLossKg = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ExcessPOPrinting = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ExcessPOSlitting = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    BtpWarehouseConfirmed = table.Column<bool>(type: "bit", precision: 18, scale: 4, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "IX_FoxWms_BlowingProcess_CreatorId",
                table: "FoxWms_BlowingProcess",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_BlowingProcess_ModifierId",
                table: "FoxWms_BlowingProcess",
                column: "ModifierId");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_BlowingProcess_ShiftLeaderId",
                table: "FoxWms_BlowingProcess",
                column: "ShiftLeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_BlowingProcessLine_BlowingProcessId",
                table: "FoxWms_BlowingProcessLine",
                column: "BlowingProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_BlowingProcessLine_CardCode",
                table: "FoxWms_BlowingProcessLine",
                column: "CardCode");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_BlowingProcessLine_WorkerId",
                table: "FoxWms_BlowingProcessLine",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_CuttingProcess_CreatorId",
                table: "FoxWms_CuttingProcess",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_CuttingProcess_ModifierId",
                table: "FoxWms_CuttingProcess",
                column: "ModifierId");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_CuttingProcess_ShiftLeaderId",
                table: "FoxWms_CuttingProcess",
                column: "ShiftLeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_CuttingProcessLine_CardCode",
                table: "FoxWms_CuttingProcessLine",
                column: "CardCode");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_CuttingProcessLine_CuttingProcessId",
                table: "FoxWms_CuttingProcessLine",
                column: "CuttingProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_CuttingProcessLine_WorkerId",
                table: "FoxWms_CuttingProcessLine",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_GrainMixingBlowingProcess_CreatorId",
                table: "FoxWms_GrainMixingBlowingProcess",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_GrainMixingBlowingProcess_ModifierId",
                table: "FoxWms_GrainMixingBlowingProcess",
                column: "ModifierId");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_GrainMixingBlowingProcessLine_CardCode",
                table: "FoxWms_GrainMixingBlowingProcessLine",
                column: "CardCode");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_GrainMixingBlowingProcessLine_GrainMixingBlowingProcessId",
                table: "FoxWms_GrainMixingBlowingProcessLine",
                column: "GrainMixingBlowingProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_GrainMixingBlowingProcessLine_WorkerId",
                table: "FoxWms_GrainMixingBlowingProcessLine",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_GrainMixingProcess_CreatorId",
                table: "FoxWms_GrainMixingProcess",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_GrainMixingProcess_ModifierId",
                table: "FoxWms_GrainMixingProcess",
                column: "ModifierId");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_GrainMixingProcessLine_CardCode",
                table: "FoxWms_GrainMixingProcessLine",
                column: "CardCode");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_GrainMixingProcessLine_GrainMixingProcessId",
                table: "FoxWms_GrainMixingProcessLine",
                column: "GrainMixingProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_GrainMixingProcessLine_WorkerId",
                table: "FoxWms_GrainMixingProcessLine",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_PrintingProcess_CreatorId",
                table: "FoxWms_PrintingProcess",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_PrintingProcess_ModifierId",
                table: "FoxWms_PrintingProcess",
                column: "ModifierId");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_PrintingProcess_ShiftLeaderId",
                table: "FoxWms_PrintingProcess",
                column: "ShiftLeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_PrintingProcessLine_CardCode",
                table: "FoxWms_PrintingProcessLine",
                column: "CardCode");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_PrintingProcessLine_PrintingProcessId",
                table: "FoxWms_PrintingProcessLine",
                column: "PrintingProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_PrintingProcessLine_WorkerId",
                table: "FoxWms_PrintingProcessLine",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_RewindingProcess_CreatorId",
                table: "FoxWms_RewindingProcess",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_RewindingProcess_ModifierId",
                table: "FoxWms_RewindingProcess",
                column: "ModifierId");

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

            migrationBuilder.CreateIndex(
                name: "IX_FoxWms_UserSessions_UserId",
                table: "FoxWms_UserSessions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoxWms_BlowingProcessLine");

            migrationBuilder.DropTable(
                name: "FoxWms_CuttingProcessLine");

            migrationBuilder.DropTable(
                name: "FoxWms_GrainMixingBlowingProcessLine");

            migrationBuilder.DropTable(
                name: "FoxWms_GrainMixingProcessLine");

            migrationBuilder.DropTable(
                name: "FoxWms_PrintingProcessLine");

            migrationBuilder.DropTable(
                name: "FoxWms_RewindingProcessLine");

            migrationBuilder.DropTable(
                name: "FoxWms_SlittingProcessLine");

            migrationBuilder.DropTable(
                name: "FoxWms_UserSessions");

            migrationBuilder.DropTable(
                name: "FoxWms_BlowingProcess");

            migrationBuilder.DropTable(
                name: "FoxWms_CuttingProcess");

            migrationBuilder.DropTable(
                name: "FoxWms_GrainMixingBlowingProcess");

            migrationBuilder.DropTable(
                name: "FoxWms_GrainMixingProcess");

            migrationBuilder.DropTable(
                name: "FoxWms_PrintingProcess");

            migrationBuilder.DropTable(
                name: "FoxWms_RewindingProcess");

            migrationBuilder.DropTable(
                name: "FoxWms_SlittingProcess");
        }
    }
}
