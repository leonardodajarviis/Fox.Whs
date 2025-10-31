using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fox.Whs.Migrations
{
    /// <inheritdoc />
    public partial class Hinata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "Fox_BlowingProcessLines");

            migrationBuilder.AddColumn<string>(
                name: "CardCode",
                table: "Fox_BlowingProcessLines",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductionOrderId",
                table: "Fox_BlowingProcessLines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Fox_BlowingProcessLines_CardCode",
                table: "Fox_BlowingProcessLines",
                column: "CardCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Fox_BlowingProcessLines_OCRD_CardCode",
                table: "Fox_BlowingProcessLines",
                column: "CardCode",
                principalTable: "OCRD",
                principalColumn: "CardCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fox_BlowingProcessLines_OCRD_CardCode",
                table: "Fox_BlowingProcessLines");

            migrationBuilder.DropIndex(
                name: "IX_Fox_BlowingProcessLines_CardCode",
                table: "Fox_BlowingProcessLines");

            migrationBuilder.DropColumn(
                name: "CardCode",
                table: "Fox_BlowingProcessLines");

            migrationBuilder.DropColumn(
                name: "ProductionOrderId",
                table: "Fox_BlowingProcessLines");

            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "Fox_BlowingProcessLines",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
