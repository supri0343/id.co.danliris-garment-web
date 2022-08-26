using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class add_SubconCustomInDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GarmentServiceSubconCustomsInDetails",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    SubconCustomsInItemId = table.Column<Guid>(nullable: false),
                    SubconCustomsOutId = table.Column<Guid>(nullable: false),
                    CustomsOutNo = table.Column<string>(maxLength: 50, nullable: true),
                    CustomsOutQty = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentServiceSubconCustomsInDetails", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentServiceSubconCustomsInDetails_GarmentServiceSubconCustomsInItems_SubconCustomsInItemId",
                        column: x => x.SubconCustomsInItemId,
                        principalTable: "GarmentServiceSubconCustomsInItems",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GarmentServiceSubconCustomsInDetails_SubconCustomsInItemId",
                table: "GarmentServiceSubconCustomsInDetails",
                column: "SubconCustomsInItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentServiceSubconCustomsInDetails");
        }
    }
}
