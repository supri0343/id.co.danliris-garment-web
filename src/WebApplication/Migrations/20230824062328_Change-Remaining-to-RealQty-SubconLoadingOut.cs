using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class ChangeRemainingtoRealQtySubconLoadingOut : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RemainingQuantity",
                table: "GarmentSubconLoadingOutItems",
                newName: "RealQtyOut");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RealQtyOut",
                table: "GarmentSubconLoadingOutItems",
                newName: "RemainingQuantity");
        }
    }
}
