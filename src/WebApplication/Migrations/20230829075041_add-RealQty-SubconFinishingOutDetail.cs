using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class addRealQtySubconFinishingOutDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RemainingQuantity",
                table: "GarmentSubconFinishingOutItems",
                newName: "RealQtyOut");

            migrationBuilder.AddColumn<double>(
                name: "RealQtyOut",
                table: "GarmentSubconFinishingOutDetails",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RealQtyOut",
                table: "GarmentSubconFinishingOutDetails");

            migrationBuilder.RenameColumn(
                name: "RealQtyOut",
                table: "GarmentSubconFinishingOutItems",
                newName: "RemainingQuantity");
        }
    }
}
