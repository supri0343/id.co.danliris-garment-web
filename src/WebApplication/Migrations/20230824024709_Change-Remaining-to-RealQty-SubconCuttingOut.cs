using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class ChangeRemainingtoRealQtySubconCuttingOut : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RemainingQuantity",
                table: "GarmentSubconCuttingOutDetails",
                newName: "RealQtyOut");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RealQtyOut",
                table: "GarmentSubconCuttingOutDetails",
                newName: "RemainingQuantity");
        }
    }
}
