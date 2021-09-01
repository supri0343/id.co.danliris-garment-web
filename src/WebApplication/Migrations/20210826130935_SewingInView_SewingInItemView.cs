using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class SewingInView_SewingInItemView : Migration
    {
        //Enhance Jason Aug 2021
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE VIEW [dbo].[SewingInItemHomeDataView]
                                AS
                                SELECT SewingInId, SUM(Quantity) AS TotalQuantity, SUM(RemainingQuantity) AS TotalRemainingQuantity, STRING_AGG(ProductCode, ',') AS Products
                                FROM  dbo.GarmentSewingInItems AS item
                                GROUP BY SewingInId");
            migrationBuilder.Sql(@"CREATE VIEW [dbo].[SewingInHomeListView]
                                AS
                                SELECT header.[Identity], header.RowVersion, header.SewingInNo, header.Article, detail.TotalQuantity, detail.TotalRemainingQuantity, header.RONo, header.UnitId, header.UnitCode, header.UnitName, header.SewingFrom, header.UnitFromId, header.UnitFromCode, header.UnitFromName, header.SewingInDate, detail.Products, header.CreatedDate, header.CreatedBy, header.ModifiedDate, header.ModifiedBy, header.Deleted, header.DeletedDate, header.DeletedBy
                                FROM  dbo.GarmentSewingIns AS header INNER JOIN
                                         dbo.SewingInItemHomeDataView AS detail ON header.[Identity] = detail.SewingInId
                                WHERE (header.Deleted = 0)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW [dbo].[SewingInHomeListView]");
            migrationBuilder.Sql(@"DROP VIEW [dbo].[SewingInItemHomeDataView]");
        }
    }
}
