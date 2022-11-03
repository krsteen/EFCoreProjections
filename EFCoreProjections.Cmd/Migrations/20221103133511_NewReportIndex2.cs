using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCoreProjections.Cmd.Migrations
{
    public partial class NewReportIndex2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reports_Name",
                table: "Reports");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_Name_IsArchived",
                table: "Reports",
                columns: new[] { "Name", "IsArchived" })
                .Annotation("SqlServer:Include", new[] { "ReportId", "Status", "ConfigId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reports_Name_IsArchived",
                table: "Reports");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_Name",
                table: "Reports",
                column: "Name",
                filter: "[IsArchived] <> 0")
                .Annotation("SqlServer:Include", new[] { "ReportId", "Status", "ConfigId" });
        }
    }
}
