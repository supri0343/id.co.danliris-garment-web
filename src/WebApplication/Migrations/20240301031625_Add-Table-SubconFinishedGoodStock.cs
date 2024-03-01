using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class AddTableSubconFinishedGoodStock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GarmentSubconFinishedGoodStocks",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    FinishedGoodStockNo = table.Column<string>(maxLength: 25, nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    UnitCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitName = table.Column<string>(maxLength: 100, nullable: true),
                    RONo = table.Column<string>(maxLength: 25, nullable: true),
                    Article = table.Column<string>(maxLength: 50, nullable: true),
                    ComodityId = table.Column<int>(nullable: false),
                    ComodityCode = table.Column<string>(maxLength: 25, nullable: true),
                    ComodityName = table.Column<string>(maxLength: 100, nullable: true),
                    SizeId = table.Column<int>(nullable: false),
                    SizeName = table.Column<string>(maxLength: 50, nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    UomId = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(maxLength: 100, nullable: true),
                    BasicPrice = table.Column<double>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    UId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconFinishedGoodStocks", x => x.Identity);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconFinishedGoodStocks_FinishedGoodStockNo",
                table: "GarmentSubconFinishedGoodStocks",
                column: "FinishedGoodStockNo",
                unique: true,
                filter: "[Deleted]=(0)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentSubconFinishedGoodStocks");
        }
    }
}
