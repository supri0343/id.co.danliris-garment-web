using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class add_buyer_SubconReprocessItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BuyerCode",
                table: "GarmentSubconReprocessItems",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BuyerId",
                table: "GarmentSubconReprocessItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BuyerName",
                table: "GarmentSubconReprocessItems",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyerCode",
                table: "GarmentSubconReprocessItems");

            migrationBuilder.DropColumn(
                name: "BuyerId",
                table: "GarmentSubconReprocessItems");

            migrationBuilder.DropColumn(
                name: "BuyerName",
                table: "GarmentSubconReprocessItems");
        }
    }
}
