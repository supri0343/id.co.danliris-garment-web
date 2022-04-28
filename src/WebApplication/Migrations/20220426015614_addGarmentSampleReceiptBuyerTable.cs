using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class addGarmentSampleReceiptBuyerTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GarmentSampleReceiptFromBuyers",
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
                    SaveAs = table.Column<string>(nullable: true),
                    ReceiptDate = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSampleReceiptFromBuyers", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSampleReceiptFromBuyerItems",
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
                    ReceiptId = table.Column<Guid>(nullable: false),
                    InvoiceNo = table.Column<string>(nullable: true),
                    BuyerAgentId = table.Column<int>(nullable: false),
                    BuyerAgentCode = table.Column<string>(nullable: true),
                    BuyerAgentName = table.Column<string>(nullable: true),
                    RONo = table.Column<string>(nullable: true),
                    Article = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Style = table.Column<string>(nullable: true),
                    ComodityId = table.Column<string>(nullable: true),
                    ComodityCode = table.Column<string>(maxLength: 50, nullable: true),
                    ComodityName = table.Column<string>(maxLength: 100, nullable: true),
                    Colour = table.Column<string>(nullable: true),
                    SizeId = table.Column<int>(nullable: false),
                    SizeName = table.Column<string>(maxLength: 100, nullable: true),
                    ReceiptQuantity = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSampleReceiptFromBuyerItems", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentSampleReceiptFromBuyerItems_GarmentSampleReceiptFromBuyers_ReceiptId",
                        column: x => x.ReceiptId,
                        principalTable: "GarmentSampleReceiptFromBuyers",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSampleReceiptFromBuyerItems_ReceiptId",
                table: "GarmentSampleReceiptFromBuyerItems",
                column: "ReceiptId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSampleReceiptFromBuyers_Identity",
                table: "GarmentSampleReceiptFromBuyers",
                column: "Identity",
                filter: "[Deleted]=(0)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentSampleReceiptFromBuyerItems");

            migrationBuilder.DropTable(
                name: "GarmentSampleReceiptFromBuyers");
        }
    }
}
