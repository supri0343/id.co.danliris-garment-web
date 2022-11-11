using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class add_Unit_ReprocessDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "GarmentSubconReprocessDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnitCode",
                table: "GarmentSubconReprocessDetails",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "GarmentSubconReprocessDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UnitName",
                table: "GarmentSubconReprocessDetails",
                maxLength: 25,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Remark",
                table: "GarmentSubconReprocessDetails");

            migrationBuilder.DropColumn(
                name: "UnitCode",
                table: "GarmentSubconReprocessDetails");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "GarmentSubconReprocessDetails");

            migrationBuilder.DropColumn(
                name: "UnitName",
                table: "GarmentSubconReprocessDetails");
        }
    }
}
