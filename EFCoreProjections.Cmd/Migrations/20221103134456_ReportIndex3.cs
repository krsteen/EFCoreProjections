using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCoreProjections.Cmd.Migrations
{
    public partial class ReportIndex3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reports_Name_IsArchived",
                table: "Reports");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_Name",
                table: "Reports",
                column: "Name")
                .Annotation("SqlServer:Include", new[] { "ReportId", "Status", "ConfigId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
