using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class addtableSubconLoadingIns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GarmentSubconLoadingIns",
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
                    LoadingNo = table.Column<string>(maxLength: 25, nullable: true),
                    CuttingOutId = table.Column<Guid>(nullable: false),
                    CuttingOutNo = table.Column<string>(maxLength: 25, nullable: true),
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
                    LoadingDate = table.Column<DateTimeOffset>(nullable: false),
                    IsApproved = table.Column<bool>(nullable: false),
                    UId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconLoadingIns", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSubconLoadingInItems",
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
                    LoadingId = table.Column<Guid>(nullable: false),
                    CuttingOutDetailId = table.Column<Guid>(nullable: false),
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
                    table.PrimaryKey("PK_GarmentSubconLoadingInItems", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentSubconLoadingInItems_GarmentSubconLoadingIns_LoadingId",
                        column: x => x.LoadingId,
                        principalTable: "GarmentSubconLoadingIns",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconLoadingInItems_LoadingId",
                table: "GarmentSubconLoadingInItems",
                column: "LoadingId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconLoadingIns_LoadingNo",
                table: "GarmentSubconLoadingIns",
                column: "LoadingNo",
                unique: true,
                filter: "[Deleted]=(0)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentSubconLoadingInItems");

            migrationBuilder.DropTable(
                name: "GarmentSubconLoadingIns");
        }
    }
}
