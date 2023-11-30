using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class addtableSubconLoadingOut : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GarmentSubconLoadingOuts",
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
                    LoadingOutNo = table.Column<string>(maxLength: 25, nullable: true),
                    LoadingInId = table.Column<Guid>(nullable: false),
                    LoadingInNo = table.Column<string>(maxLength: 25, nullable: true),
                    UnitFromId = table.Column<int>(nullable: false),
                    UnitFromCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitFromName = table.Column<string>(maxLength: 100, nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    UnitCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitName = table.Column<string>(maxLength: 100, nullable: true),
                    RONo = table.Column<string>(maxLength: 25, nullable: true),
                    Article = table.Column<string>(maxLength: 50, nullable: true),
                    ComodityId = table.Column<int>(nullable: false),
                    ComodityName = table.Column<string>(maxLength: 500, nullable: true),
                    ComodityCode = table.Column<string>(maxLength: 100, nullable: true),
                    LoadingOutDate = table.Column<DateTimeOffset>(nullable: false),
                    UId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconLoadingOuts", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSubconLoadingOutItems",
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
                    LoadingOutId = table.Column<Guid>(nullable: false),
                    LoadingInItemId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    ProductCode = table.Column<string>(maxLength: 50, nullable: true),
                    ProductName = table.Column<string>(maxLength: 500, nullable: true),
                    DesignColor = table.Column<string>(maxLength: 2000, nullable: true),
                    SizeId = table.Column<int>(nullable: false),
                    SizeName = table.Column<string>(maxLength: 50, nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    UomId = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(maxLength: 50, nullable: true),
                    Color = table.Column<string>(maxLength: 1000, nullable: true),
                    RemainingQuantity = table.Column<double>(nullable: false),
                    BasicPrice = table.Column<double>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    UId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconLoadingOutItems", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentSubconLoadingOutItems_GarmentSubconLoadingOuts_LoadingOutId",
                        column: x => x.LoadingOutId,
                        principalTable: "GarmentSubconLoadingOuts",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconLoadingOutItems_LoadingOutId",
                table: "GarmentSubconLoadingOutItems",
                column: "LoadingOutId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconLoadingOuts_LoadingOutNo",
                table: "GarmentSubconLoadingOuts",
                column: "LoadingOutNo",
                unique: true,
                filter: "[Deleted]=(0)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentSubconLoadingOutItems");

            migrationBuilder.DropTable(
                name: "GarmentSubconLoadingOuts");
        }
    }
}
