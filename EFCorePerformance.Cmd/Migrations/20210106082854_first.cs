using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCorePerformance.Cmd.Migrations
{
    public partial class first : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReportConfigsWithBasicIndexes",
                columns: table => new
                {
                    ConfigId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VeryUsefulInformation = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportConfigsWithBasicIndexes", x => x.ConfigId);
                });

            migrationBuilder.CreateTable(
                name: "ReportConfigsWithBetterIndexes",
                columns: table => new
                {
                    ConfigId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VeryUsefulInformation = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportConfigsWithBetterIndexes", x => x.ConfigId);
                });

            migrationBuilder.CreateTable(
                name: "ReportsWithBasicIndex",
                columns: table => new
                {
                    ReportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    ConfigId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportsWithBasicIndex", x => x.ReportId);
                    table.ForeignKey(
                        name: "FK_ReportsWithBasicIndex_ReportConfigsWithBasicIndexes_ConfigId",
                        column: x => x.ConfigId,
                        principalTable: "ReportConfigsWithBasicIndexes",
                        principalColumn: "ConfigId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportsWithBetterIndex",
                columns: table => new
                {
                    ReportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    ConfigId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportsWithBetterIndex", x => x.ReportId);
                    table.ForeignKey(
                        name: "FK_ReportsWithBetterIndex_ReportConfigsWithBetterIndexes_ConfigId",
                        column: x => x.ConfigId,
                        principalTable: "ReportConfigsWithBetterIndexes",
                        principalColumn: "ConfigId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportCommentsWithBasicIndex",
                columns: table => new
                {
                    CommentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportId = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportCommentsWithBasicIndex", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_ReportCommentsWithBasicIndex_ReportsWithBasicIndex_ReportId",
                        column: x => x.ReportId,
                        principalTable: "ReportsWithBasicIndex",
                        principalColumn: "ReportId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportCommentsWithBetterIndex",
                columns: table => new
                {
                    CommentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportId = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportCommentsWithBetterIndex", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_ReportCommentsWithBetterIndex_ReportsWithBetterIndex_ReportId",
                        column: x => x.ReportId,
                        principalTable: "ReportsWithBetterIndex",
                        principalColumn: "ReportId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReportCommentsWithBasicIndex_ReportId",
                table: "ReportCommentsWithBasicIndex",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCommentsWithBetterIndex_ReportId",
                table: "ReportCommentsWithBetterIndex",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportsWithBasicIndex_ConfigId",
                table: "ReportsWithBasicIndex",
                column: "ConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportsWithBetterIndex_ConfigId",
                table: "ReportsWithBetterIndex",
                column: "ConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportsWithBetterIndex_Name",
                table: "ReportsWithBetterIndex",
                column: "Name",
                filter: "[IsArchived] = 0")
                .Annotation("SqlServer:Include", new[] { "ReportId", "Status" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportCommentsWithBasicIndex");

            migrationBuilder.DropTable(
                name: "ReportCommentsWithBetterIndex");

            migrationBuilder.DropTable(
                name: "ReportsWithBasicIndex");

            migrationBuilder.DropTable(
                name: "ReportsWithBetterIndex");

            migrationBuilder.DropTable(
                name: "ReportConfigsWithBasicIndexes");

            migrationBuilder.DropTable(
                name: "ReportConfigsWithBetterIndexes");
        }
    }
}
