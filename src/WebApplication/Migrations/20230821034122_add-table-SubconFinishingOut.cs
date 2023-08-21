using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class addtableSubconFinishingOut : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GarmentSubconFinishingOuts",
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
                    FinishingOutNo = table.Column<string>(maxLength: 25, nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    UnitCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitName = table.Column<string>(maxLength: 100, nullable: true),
                    FinishingTo = table.Column<string>(maxLength: 100, nullable: true),
                    UnitToId = table.Column<int>(nullable: false),
                    UnitToCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitToName = table.Column<string>(maxLength: 100, nullable: true),
                    RONo = table.Column<string>(maxLength: 25, nullable: true),
                    Article = table.Column<string>(maxLength: 50, nullable: true),
                    ComodityId = table.Column<int>(nullable: false),
                    ComodityCode = table.Column<string>(maxLength: 25, nullable: true),
                    ComodityName = table.Column<string>(maxLength: 100, nullable: true),
                    FinishingOutDate = table.Column<DateTimeOffset>(nullable: false),
                    IsDifferentSize = table.Column<bool>(nullable: false),
                    UId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconFinishingOuts", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSubconFinishingOutItems",
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
                    FinishingOutId = table.Column<Guid>(nullable: false),
                    FinishingInId = table.Column<Guid>(nullable: false),
                    FinishingInItemId = table.Column<Guid>(nullable: false),
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
                    table.PrimaryKey("PK_GarmentSubconFinishingOutItems", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentSubconFinishingOutItems_GarmentSubconFinishingOuts_FinishingOutId",
                        column: x => x.FinishingOutId,
                        principalTable: "GarmentSubconFinishingOuts",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSubconFinishingOutDetails",
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
                    FinishingOutItemId = table.Column<Guid>(nullable: false),
                    SizeId = table.Column<int>(nullable: false),
                    SizeName = table.Column<string>(maxLength: 100, nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    UomId = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(maxLength: 25, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconFinishingOutDetails", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentSubconFinishingOutDetails_GarmentSubconFinishingOutItems_FinishingOutItemId",
                        column: x => x.FinishingOutItemId,
                        principalTable: "GarmentSubconFinishingOutItems",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconFinishingOutDetails_FinishingOutItemId",
                table: "GarmentSubconFinishingOutDetails",
                column: "FinishingOutItemId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconFinishingOutItems_FinishingOutId",
                table: "GarmentSubconFinishingOutItems",
                column: "FinishingOutId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconFinishingOuts_FinishingOutNo",
                table: "GarmentSubconFinishingOuts",
                column: "FinishingOutNo",
                unique: true,
                filter: "[Deleted]=(0)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentSubconFinishingOutDetails");

            migrationBuilder.DropTable(
                name: "GarmentSubconFinishingOutItems");

            migrationBuilder.DropTable(
                name: "GarmentSubconFinishingOuts");
        }
    }
}
