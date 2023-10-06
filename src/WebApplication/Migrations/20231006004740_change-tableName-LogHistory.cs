using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class changetableNameLogHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LogHistories",
                table: "LogHistories");

            migrationBuilder.RenameTable(
                name: "LogHistories",
                newName: "LogHistory");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "LogHistory",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddPrimaryKey(
                name: "PK_LogHistory",
                table: "LogHistory",
                column: "Identity");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LogHistory",
                table: "LogHistory");

            migrationBuilder.RenameTable(
                name: "LogHistory",
                newName: "LogHistories");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "LogHistories",
                nullable: false,
                oldClrType: typeof(DateTimeOffset));

            migrationBuilder.AddPrimaryKey(
                name: "PK_LogHistories",
                table: "LogHistories",
                column: "Identity");
        }
    }
}
