using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class ChangeRemainingtoRealQtySubconSewingOut : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RemainingQuantity",
                table: "GarmentSubconSewingOutItems",
                newName: "RealQtyOut");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RealQtyOut",
                table: "GarmentSubconSewingOutItems",
                newName: "RemainingQuantity");
        }
    }
}
