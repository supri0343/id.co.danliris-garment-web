using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class removeColoumIsPackingListPackingOutItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPackingList",
                table: "GarmentSubconPackingOutItems");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPackingList",
                table: "GarmentSubconPackingOutItems",
                nullable: false,
                defaultValue: false);
        }
    }
}
