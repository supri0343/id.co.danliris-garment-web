using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class addtableGarmentServiceSubconExpenditureGood : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "BCDate",
            //    table: "GarmentAvalProductItems",
            //    nullable: true,
            //    oldClrType: typeof(DateTime));

            //migrationBuilder.AddColumn<string>(
            //    name: "BCType",
            //    table: "GarmentAvalProductItems",
            //    nullable: true);

            migrationBuilder.CreateTable(
                name: "GarmentServiceSubconExpenditureGoods",
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
                    ServiceSubconExpenditureGoodNo = table.Column<string>(maxLength: 25, nullable: true),
                    ServiceSubconExpenditureGoodDate = table.Column<DateTimeOffset>(nullable: false),
                    IsUsed = table.Column<bool>(nullable: false),
                    BuyerId = table.Column<int>(nullable: false),
                    BuyerCode = table.Column<string>(nullable: true),
                    BuyerName = table.Column<string>(nullable: true),
                    QtyPacking = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(nullable: true),
                    NettWeight = table.Column<double>(nullable: false),
                    GrossWeight = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentServiceSubconExpenditureGoods", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "GarmentServiceSubconExpenditureGoodItems",
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
                    ServiceSubconExpenditureGoodId = table.Column<Guid>(nullable: false),
                    FinishedGoodStockId = table.Column<Guid>(nullable: false),
                    RONo = table.Column<string>(maxLength: 25, nullable: true),
                    Article = table.Column<string>(maxLength: 50, nullable: true),
                    ComodityId = table.Column<int>(nullable: false),
                    ComodityCode = table.Column<string>(maxLength: 25, nullable: true),
                    ComodityName = table.Column<string>(maxLength: 100, nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    UnitCode = table.Column<string>(nullable: true),
                    UnitName = table.Column<string>(nullable: true),
                    UomUnit = table.Column<string>(maxLength: 25, nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    BasicPrice = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentServiceSubconExpenditureGoodItems", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentServiceSubconExpenditureGoodItems_GarmentServiceSubconExpenditureGoods_ServiceSubconExpenditureGoodId",
                        column: x => x.ServiceSubconExpenditureGoodId,
                        principalTable: "GarmentServiceSubconExpenditureGoods",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GarmentServiceSubconExpenditureGoodItems_ServiceSubconExpenditureGoodId",
                table: "GarmentServiceSubconExpenditureGoodItems",
                column: "ServiceSubconExpenditureGoodId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentServiceSubconExpenditureGoods_ServiceSubconExpenditureGoodNo",
                table: "GarmentServiceSubconExpenditureGoods",
                column: "ServiceSubconExpenditureGoodNo",
                unique: true,
                filter: "[Deleted]=(0)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentServiceSubconExpenditureGoodItems");

            migrationBuilder.DropTable(
                name: "GarmentServiceSubconExpenditureGoods");

            //    migrationBuilder.DropColumn(
            //        name: "BCType",
            //        table: "GarmentAvalProductItems");

            //    migrationBuilder.AlterColumn<DateTime>(
            //        name: "BCDate",
            //        table: "GarmentAvalProductItems",
            //        nullable: false,
            //        oldClrType: typeof(DateTime),
            //        oldNullable: true);
        }
    }
}
