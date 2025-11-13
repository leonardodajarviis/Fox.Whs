using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fox.Whs.Migrations
{
    /// <inheritdoc />
    public partial class AddGrainMixingBlowingProcess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FoxWms_GrainMixingBlowingProcess",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDraft = table.Column<bool>(type: "bit", nullable: false),
                    ProductionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BlowingMachine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatorId = table.Column<short>(type: "smallint", nullable: false),
                    ModifierId = table.Column<short>(type: "smallint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                name: "FoxWms_GrainMixingBlowingProcessLine",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GrainMixingBlowingProcessId = table.Column<int>(type: "int", nullable: false),
                    ProductionBatch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardCode = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    MaterialIssueVoucherNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MixtureType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Specification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkerId = table.Column<int>(type: "int", nullable: true),
                    MachineName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PpTron = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PpHdNhot = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PpLdpe = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PpDc = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PpAdditive = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PpColor = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PpOther = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    HdLldpe2320 = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    HdRecycled = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    HdTalcol = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    HdDc = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    HdColor = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    HdOther = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PeAdditive = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PeTalcol = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PeColor = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PeOther = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PeLdpe = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PeLldpe = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PeRecycled = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ShrinkRe707 = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ShrinkSlip = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ShrinkStatic = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ShrinkDc = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ShrinkTalcol = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ShrinkOther = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
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
                    EvaPop3070 = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    EvaLdpe = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    EvaDc = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    EvaTalcol = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    EvaSlip = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    EvaStaticAdditive = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    EvaOther = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    QuantityKg = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    RequiredDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ActualCompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DelayReason = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoxWms_GrainMixingBlowingProcessLine");

            migrationBuilder.DropTable(
                name: "FoxWms_GrainMixingBlowingProcess");
        }
    }
}
