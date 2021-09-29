using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class update_cuttingInDetail_color_length : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "GarmentCuttingInDetails",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "GarmentMonitoringFinishingReportTemplate",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(nullable: true),
                    RoJob = table.Column<string>(maxLength: 25, nullable: false),
                    Article = table.Column<string>(maxLength: 100, nullable: true),
                    Stock = table.Column<double>(nullable: false),
                    SewingQtyPcs = table.Column<double>(nullable: false),
                    FinishingQtyPcs = table.Column<double>(nullable: false),
                    RemainQty = table.Column<double>(nullable: false),
                    UomUnit = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentMonitoringFinishingReportTemplate", x => x.RoJob);
                });

            migrationBuilder.CreateTable(
                name: "SewingInHomeListView",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(nullable: true),
                    SewingInNo = table.Column<string>(maxLength: 25, nullable: true),
                    Article = table.Column<string>(maxLength: 50, nullable: true),
                    TotalQuantity = table.Column<double>(nullable: false),
                    TotalRemainingQuantity = table.Column<double>(nullable: false),
                    RONo = table.Column<string>(maxLength: 25, nullable: true),
                    UnitFromCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitCode = table.Column<string>(maxLength: 25, nullable: true),
                    SewingFrom = table.Column<string>(maxLength: 25, nullable: true),
                    SewingInDate = table.Column<DateTimeOffset>(nullable: false),
                    Products = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SewingInHomeListView", x => x.Identity);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentMonitoringFinishingReportTemplate");

            migrationBuilder.DropTable(
                name: "SewingInHomeListView");

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "GarmentCuttingInDetails",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 1000,
                oldNullable: true);
        }
    }
}
