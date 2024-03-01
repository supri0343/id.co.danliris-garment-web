using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class RenameColoumSubconExpenditureGoodReturnExpenditureNotoPackingOutNo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExpenditureNo",
                table: "GarmentSubconExpenditureGoodReturns",
                newName: "PackingOutNo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PackingOutNo",
                table: "GarmentSubconExpenditureGoodReturns",
                newName: "ExpenditureNo");
        }
    }
}
