using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCorePerformance.Cmd.Migrations
{
    public partial class firstcreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReportConfigsWithBasicIndexes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VeryUsefulInformation = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportConfigsWithBasicIndexes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReportConfigsWithBetterIndexes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VeryUsefulInformation = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportConfigsWithBetterIndexes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReportsWithBasicIndex",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConfigId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportsWithBasicIndex", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportsWithBasicIndex_ReportConfigsWithBasicIndexes_ConfigId",
                        column: x => x.ConfigId,
                        principalTable: "ReportConfigsWithBasicIndexes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportsWithBetterIndex",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConfigId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportsWithBetterIndex", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportsWithBetterIndex_ReportConfigsWithBetterIndexes_ConfigId",
                        column: x => x.ConfigId,
                        principalTable: "ReportConfigsWithBetterIndexes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportCommentsWithBasicIndex",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportId = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportCommentsWithBasicIndex", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportCommentsWithBasicIndex_ReportsWithBasicIndex_ReportId",
                        column: x => x.ReportId,
                        principalTable: "ReportsWithBasicIndex",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportCommentsWithBetterIndex",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportId = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportCommentsWithBetterIndex", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportCommentsWithBetterIndex_ReportsWithBetterIndex_ReportId",
                        column: x => x.ReportId,
                        principalTable: "ReportsWithBetterIndex",
                        principalColumn: "Id",
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
