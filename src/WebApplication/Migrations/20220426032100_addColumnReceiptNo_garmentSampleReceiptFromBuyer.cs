using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class addColumnReceiptNo_garmentSampleReceiptFromBuyer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GarmentSampleReceiptFromBuyers_Identity",
                table: "GarmentSampleReceiptFromBuyers");

            migrationBuilder.AddColumn<string>(
                name: "ReceiptNo",
                table: "GarmentSampleReceiptFromBuyers",
                maxLength: 25,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSampleReceiptFromBuyers_ReceiptNo",
                table: "GarmentSampleReceiptFromBuyers",
                column: "ReceiptNo",
                unique: true,
                filter: "[Deleted]=(0)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GarmentSampleReceiptFromBuyers_ReceiptNo",
                table: "GarmentSampleReceiptFromBuyers");

            migrationBuilder.DropColumn(
                name: "ReceiptNo",
                table: "GarmentSampleReceiptFromBuyers");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSampleReceiptFromBuyers_Identity",
                table: "GarmentSampleReceiptFromBuyers",
                column: "Identity",
                filter: "[Deleted]=(0)");
        }
    }
}
