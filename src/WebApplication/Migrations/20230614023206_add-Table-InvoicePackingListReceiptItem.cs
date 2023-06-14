using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class addTableInvoicePackingListReceiptItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "POType",
                table: "SubconInvoicePackingList",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SubconInvoicePackingListReceiptItems",
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
                    InvoicePackingListId = table.Column<Guid>(nullable: false),
                    DLNo = table.Column<string>(maxLength: 50, nullable: true),
                    DLDate = table.Column<DateTimeOffset>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    ProductCode = table.Column<string>(maxLength: 25, nullable: true),
                    ProductName = table.Column<string>(maxLength: 100, nullable: true),
                    ProductRemark = table.Column<string>(maxLength: 255, nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    UomId = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(maxLength: 50, nullable: true),
                    TotalPrice = table.Column<double>(nullable: false),
                    PricePerDealUnit = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubconInvoicePackingListReceiptItems", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_SubconInvoicePackingListReceiptItems_SubconInvoicePackingList_InvoicePackingListId",
                        column: x => x.InvoicePackingListId,
                        principalTable: "SubconInvoicePackingList",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubconInvoicePackingListReceiptItems_InvoicePackingListId",
                table: "SubconInvoicePackingListReceiptItems",
                column: "InvoicePackingListId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubconInvoicePackingListReceiptItems");

            migrationBuilder.DropColumn(
                name: "POType",
                table: "SubconInvoicePackingList");
        }
    }
}
