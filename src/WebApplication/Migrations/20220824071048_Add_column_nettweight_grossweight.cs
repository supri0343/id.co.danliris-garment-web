using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Add_column_nettweight_grossweight : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "UomSatuanId",
            //    table: "GarmentSubconDeliveryLetterOutItems");

            //migrationBuilder.DropColumn(
            //    name: "BuyerCode",
            //    table: "GarmentSubconContracts");

            //migrationBuilder.DropColumn(
            //    name: "BuyerId",
            //    table: "GarmentSubconContracts");

            //migrationBuilder.DropColumn(
            //    name: "BuyerName",
            //    table: "GarmentSubconContracts");

            migrationBuilder.AddColumn<double>(
                name: "GrossWeight",
                table: "GarmentSubconContracts",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "NettWeight",
                table: "GarmentSubconContracts",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GrossWeight",
                table: "GarmentSubconContracts");

            migrationBuilder.DropColumn(
                name: "NettWeight",
                table: "GarmentSubconContracts");

            //migrationBuilder.AddColumn<int>(
            //    name: "UomSatuanId",
            //    table: "GarmentSubconDeliveryLetterOutItems",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AddColumn<string>(
            //    name: "BuyerCode",
            //    table: "GarmentSubconContracts",
            //    maxLength: 25,
            //    nullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "BuyerId",
            //    table: "GarmentSubconContracts",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AddColumn<string>(
            //    name: "BuyerName",
            //    table: "GarmentSubconContracts",
            //    maxLength: 255,
            //    nullable: true);
        }
    }
}
