using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fox.Whs.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditLogsForProcesses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FoxWms_AuditLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Action = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    KeyValuesJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OldValuesJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValuesJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<short>(type: "smallint", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoxWms_AuditLogs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoxWms_AuditLogs");
        }
    }
}
