using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class AddTableSubconExpenditureGoodReturn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GarmentSubconExpenditureGoodReturns",
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
                    ReturNo = table.Column<string>(maxLength: 25, nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    UnitCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitName = table.Column<string>(maxLength: 100, nullable: true),
                    ReturType = table.Column<string>(maxLength: 25, nullable: true),
                    ExpenditureNo = table.Column<string>(maxLength: 50, nullable: true),
                    DONo = table.Column<string>(maxLength: 50, nullable: true),
                    BCNo = table.Column<string>(maxLength: 50, nullable: true),
                    BCType = table.Column<string>(maxLength: 50, nullable: true),
                    URNNo = table.Column<string>(maxLength: 50, nullable: true),
                    RONo = table.Column<string>(maxLength: 25, nullable: true),
                    Article = table.Column<string>(maxLength: 50, nullable: true),
                    ComodityId = table.Column<int>(nullable: false),
                    ComodityCode = table.Column<string>(maxLength: 25, nullable: true),
                    ComodityName = table.Column<string>(maxLength: 100, nullable: true),
                    BuyerId = table.Column<int>(nullable: false),
                    BuyerCode = table.Column<string>(maxLength: 25, nullable: true),
                    BuyerName = table.Column<string>(maxLength: 100, nullable: true),
                    ReturDate = table.Column<DateTimeOffset>(nullable: false),
                    Invoice = table.Column<string>(maxLength: 50, nullable: true),
                    ContractNo = table.Column<string>(nullable: true),
                    ReturDesc = table.Column<string>(maxLength: 500, nullable: true),
                    UId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconExpenditureGoodReturns", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSubconExpenditureGoodReturnItems",
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
                    ReturId = table.Column<Guid>(nullable: false),
                    ExpenditureGoodId = table.Column<Guid>(nullable: false),
                    ExpenditureGoodItemId = table.Column<Guid>(nullable: false),
                    FinishedGoodStockId = table.Column<Guid>(nullable: false),
                    SizeId = table.Column<int>(nullable: false),
                    SizeName = table.Column<string>(maxLength: 100, nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    UomId = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(maxLength: 10, nullable: true),
                    Description = table.Column<string>(maxLength: 2000, nullable: true),
                    BasicPrice = table.Column<double>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    UId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconExpenditureGoodReturnItems", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentSubconExpenditureGoodReturnItems_GarmentSubconExpenditureGoodReturns_ReturId",
                        column: x => x.ReturId,
                        principalTable: "GarmentSubconExpenditureGoodReturns",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconExpenditureGoodReturnItems_ReturId",
                table: "GarmentSubconExpenditureGoodReturnItems",
                column: "ReturId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconExpenditureGoodReturns_ReturNo",
                table: "GarmentSubconExpenditureGoodReturns",
                column: "ReturNo",
                unique: true,
                filter: "[Deleted]=(0)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentSubconExpenditureGoodReturnItems");

            migrationBuilder.DropTable(
                name: "GarmentSubconExpenditureGoodReturns");
        }
    }
}
