using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class addtableSubconPackingOut : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GarmentSubconPackingOuts",
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
                    PackingOutNo = table.Column<string>(maxLength: 25, nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    UnitCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitName = table.Column<string>(maxLength: 100, nullable: true),
                    PackingOutType = table.Column<string>(maxLength: 25, nullable: true),
                    RONo = table.Column<string>(maxLength: 25, nullable: true),
                    Article = table.Column<string>(maxLength: 50, nullable: true),
                    ComodityId = table.Column<int>(nullable: false),
                    ComodityCode = table.Column<string>(maxLength: 25, nullable: true),
                    ComodityName = table.Column<string>(maxLength: 100, nullable: true),
                    ProductOwnerId = table.Column<int>(nullable: false),
                    ProductOwnerCode = table.Column<string>(maxLength: 25, nullable: true),
                    ProductOwnerName = table.Column<string>(maxLength: 100, nullable: true),
                    PackingOutDate = table.Column<DateTimeOffset>(nullable: false),
                    Invoice = table.Column<string>(maxLength: 50, nullable: true),
                    PackingListId = table.Column<int>(nullable: false),
                    ContractNo = table.Column<string>(nullable: true),
                    Carton = table.Column<double>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsReceived = table.Column<bool>(nullable: false),
                    UId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconPackingOuts", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSubconPackingOutItems",
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
                    PackingOutId = table.Column<Guid>(nullable: false),
                    PackingInItemId = table.Column<Guid>(nullable: false),
                    SizeId = table.Column<int>(nullable: false),
                    SizeName = table.Column<string>(maxLength: 100, nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    ReturQuantity = table.Column<double>(nullable: false),
                    UomId = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(maxLength: 10, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    BasicPrice = table.Column<double>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    UId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconPackingOutItems", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentSubconPackingOutItems_GarmentSubconPackingOuts_PackingOutId",
                        column: x => x.PackingOutId,
                        principalTable: "GarmentSubconPackingOuts",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconPackingOutItems_PackingOutId",
                table: "GarmentSubconPackingOutItems",
                column: "PackingOutId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconPackingOuts_PackingOutNo",
                table: "GarmentSubconPackingOuts",
                column: "PackingOutNo",
                unique: true,
                filter: "[Deleted]=(0)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentSubconPackingOutItems");

            migrationBuilder.DropTable(
                name: "GarmentSubconPackingOuts");
        }
    }
}
