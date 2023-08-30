using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class addtableSubconPackingIn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GarmentSubconPackingIns",
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
                    PackingInNo = table.Column<string>(maxLength: 25, nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    UnitCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitName = table.Column<string>(maxLength: 100, nullable: true),
                    UnitFromId = table.Column<int>(nullable: false),
                    UnitFromCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitFromName = table.Column<string>(maxLength: 100, nullable: true),
                    PackingFrom = table.Column<string>(nullable: true),
                    RONo = table.Column<string>(maxLength: 25, nullable: true),
                    Article = table.Column<string>(maxLength: 2000, nullable: true),
                    ComodityId = table.Column<int>(nullable: false),
                    ComodityCode = table.Column<string>(maxLength: 25, nullable: true),
                    ComodityName = table.Column<string>(maxLength: 100, nullable: true),
                    PackingInDate = table.Column<DateTimeOffset>(nullable: false),
                    IsApproved = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconPackingIns", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSubconPackingInItems",
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
                    PackingInId = table.Column<Guid>(nullable: false),
                    CuttingOutItemId = table.Column<Guid>(nullable: false),
                    CuttingOutDetailId = table.Column<Guid>(nullable: false),
                    SewingOutItemId = table.Column<Guid>(nullable: false),
                    SewingOutDetailId = table.Column<Guid>(nullable: false),
                    FinishingOutItemId = table.Column<Guid>(nullable: false),
                    FinishingOutDetailId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    ProductCode = table.Column<string>(maxLength: 25, nullable: true),
                    ProductName = table.Column<string>(maxLength: 100, nullable: true),
                    DesignColor = table.Column<string>(maxLength: 2000, nullable: true),
                    SizeId = table.Column<int>(nullable: false),
                    SizeName = table.Column<string>(maxLength: 100, nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    UomId = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(maxLength: 25, nullable: true),
                    Color = table.Column<string>(nullable: true),
                    RemainingQuantity = table.Column<double>(nullable: false),
                    BasicPrice = table.Column<double>(nullable: false),
                    Price = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconPackingInItems", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentSubconPackingInItems_GarmentSubconPackingIns_PackingInId",
                        column: x => x.PackingInId,
                        principalTable: "GarmentSubconPackingIns",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconPackingInItems_PackingInId",
                table: "GarmentSubconPackingInItems",
                column: "PackingInId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconPackingIns_PackingInNo",
                table: "GarmentSubconPackingIns",
                column: "PackingInNo",
                unique: true,
                filter: "[Deleted]=(0)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentSubconPackingInItems");

            migrationBuilder.DropTable(
                name: "GarmentSubconPackingIns");

        }
    }
}
