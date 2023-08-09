using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class addColumnSubconDLItemsSmallQtySmallUomforBBPanel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "SmallQuantity",
                table: "GarmentSubconDeliveryLetterOutItems",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "SmallUomUnit",
                table: "GarmentSubconDeliveryLetterOutItems",
                maxLength: 10,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SmallQuantity",
                table: "GarmentSubconDeliveryLetterOutItems");

            migrationBuilder.DropColumn(
                name: "SmallUomUnit",
                table: "GarmentSubconDeliveryLetterOutItems");
        }
    }
}
