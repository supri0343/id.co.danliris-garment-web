using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class changeDataTypeComodityIdgarmentSampleReceiptBuyerItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ComodityId",
                table: "GarmentSampleReceiptFromBuyerItems",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ComodityId",
                table: "GarmentSampleReceiptFromBuyerItems",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
