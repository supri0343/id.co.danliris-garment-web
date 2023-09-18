using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class addtableSubconSewingOut : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GarmentSubconSewingOuts",
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
                    SewingOutNo = table.Column<string>(maxLength: 25, nullable: true),
                    ProductOwnerId = table.Column<int>(nullable: false),
                    ProductOwnerCode = table.Column<string>(maxLength: 25, nullable: true),
                    ProductOwnerName = table.Column<string>(maxLength: 100, nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    UnitCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitName = table.Column<string>(maxLength: 100, nullable: true),
                    SewingTo = table.Column<string>(maxLength: 100, nullable: true),
                    UnitToId = table.Column<int>(nullable: false),
                    UnitToCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitToName = table.Column<string>(maxLength: 100, nullable: true),
                    RONo = table.Column<string>(maxLength: 25, nullable: true),
                    Article = table.Column<string>(maxLength: 50, nullable: true),
                    ComodityId = table.Column<int>(nullable: false),
                    ComodityCode = table.Column<string>(maxLength: 25, nullable: true),
                    ComodityName = table.Column<string>(maxLength: 100, nullable: true),
                    SewingOutDate = table.Column<DateTimeOffset>(nullable: false),
                    IsDifferentSize = table.Column<bool>(nullable: false),
                    UId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconSewingOuts", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSubconSewingOutItems",
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
                    SewingOutId = table.Column<Guid>(nullable: false),
                    SewingInId = table.Column<Guid>(nullable: false),
                    SewingInItemId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    ProductCode = table.Column<string>(maxLength: 25, nullable: true),
                    ProductName = table.Column<string>(maxLength: 100, nullable: true),
                    DesignColor = table.Column<string>(maxLength: 2000, nullable: true),
                    SizeId = table.Column<int>(nullable: false),
                    SizeName = table.Column<string>(maxLength: 100, nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    RemainingQuantity = table.Column<double>(nullable: false),
                    UomId = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(maxLength: 25, nullable: true),
                    Color = table.Column<string>(nullable: true),
                    BasicPrice = table.Column<double>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    UId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconSewingOutItems", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentSubconSewingOutItems_GarmentSubconSewingOuts_SewingOutId",
                        column: x => x.SewingOutId,
                        principalTable: "GarmentSubconSewingOuts",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSubconSewingOutDetails",
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
                    SewingOutItemId = table.Column<Guid>(nullable: false),
                    SizeId = table.Column<int>(nullable: false),
                    SizeName = table.Column<string>(maxLength: 100, nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    UomId = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(maxLength: 25, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconSewingOutDetails", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentSubconSewingOutDetails_GarmentSubconSewingOutItems_SewingOutItemId",
                        column: x => x.SewingOutItemId,
                        principalTable: "GarmentSubconSewingOutItems",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconSewingOutDetails_SewingOutItemId",
                table: "GarmentSubconSewingOutDetails",
                column: "SewingOutItemId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconSewingOutItems_SewingOutId",
                table: "GarmentSubconSewingOutItems",
                column: "SewingOutId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconSewingOuts_SewingOutNo",
                table: "GarmentSubconSewingOuts",
                column: "SewingOutNo",
                unique: true,
                filter: "[Deleted]=(0)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentSubconSewingOutDetails");

            migrationBuilder.DropTable(
                name: "GarmentSubconSewingOutItems");

            migrationBuilder.DropTable(
                name: "GarmentSubconSewingOuts");
        }
    }
}
