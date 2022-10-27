using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Add_TotalNW_TotalGW_InvoicePackingList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "UomSatuanId",
            //    table: "GarmentSubconDeliveryLetterOutItems");

            migrationBuilder.AddColumn<double>(
                name: "TotalGW",
                table: "SubconInvoicePackingListItems",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalNW",
                table: "SubconInvoicePackingListItems",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalGW",
                table: "SubconInvoicePackingListItems");

            migrationBuilder.DropColumn(
                name: "TotalNW",
                table: "SubconInvoicePackingListItems");

            //migrationBuilder.AddColumn<int>(
            //    name: "UomSatuanId",
            //    table: "GarmentSubconDeliveryLetterOutItems",
            //    nullable: false,
            //    defaultValue: 0);
        }
    }
}
