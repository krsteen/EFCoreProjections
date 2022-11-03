using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCoreProjections.Cmd.Migrations
{
    public partial class TwoReportIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Reports_IsArchived_Name",
                table: "Reports",
                columns: new[] { "IsArchived", "Name" })
                .Annotation("SqlServer:Include", new[] { "ReportId", "Status", "ConfigId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reports_IsArchived_Name",
                table: "Reports");
        }
    }
}
