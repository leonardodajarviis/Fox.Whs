using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fox.Whs.Migrations
{
    /// <inheritdoc />
    public partial class KhuKaRi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductTypeName",
                table: "FoxWms_PrintingProcessLines",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductTypeName",
                table: "FoxWms_CuttingProcessesLine",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductTypeName",
                table: "FoxWms_BlowingProcessLines",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductTypeName",
                table: "FoxWms_PrintingProcessLines");

            migrationBuilder.DropColumn(
                name: "ProductTypeName",
                table: "FoxWms_CuttingProcessesLine");

            migrationBuilder.DropColumn(
                name: "ProductTypeName",
                table: "FoxWms_BlowingProcessLines");
        }
    }
}
