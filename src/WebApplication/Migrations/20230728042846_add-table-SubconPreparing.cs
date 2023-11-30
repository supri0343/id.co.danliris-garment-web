using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class addtableSubconPreparing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GarmentSubconPreparings",
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
                    UENId = table.Column<int>(nullable: false),
                    UENNo = table.Column<string>(maxLength: 100, nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    UnitCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitName = table.Column<string>(maxLength: 100, nullable: true),
                    ProductOwnerId = table.Column<int>(nullable: false),
                    ProductOwnerCode = table.Column<string>(maxLength: 100, nullable: true),
                    ProductOwnerName = table.Column<string>(maxLength: 500, nullable: true),
                    ProcessDate = table.Column<DateTimeOffset>(nullable: true),
                    RONo = table.Column<string>(maxLength: 100, nullable: true),
                    Article = table.Column<string>(maxLength: 500, nullable: true),
                    IsCuttingIn = table.Column<bool>(nullable: false),
                    UId = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconPreparings", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSubconPreparingItems",
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
                    UENItemId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    ProductCode = table.Column<string>(maxLength: 25, nullable: true),
                    ProductName = table.Column<string>(maxLength: 100, nullable: true),
                    DesignColor = table.Column<string>(maxLength: 2000, nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    UomId = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(maxLength: 100, nullable: true),
                    FabricType = table.Column<string>(maxLength: 100, nullable: true),
                    RemainingQuantity = table.Column<double>(nullable: false),
                    BasicPrice = table.Column<double>(nullable: false),
                    GarmentSubconPreparingId = table.Column<Guid>(nullable: false),
                    UId = table.Column<string>(maxLength: 255, nullable: true),
                    ROSource = table.Column<string>(maxLength: 100, nullable: true),
                    BeacukaiNo = table.Column<string>(maxLength: 20, nullable: true),
                    BeacukaiDate = table.Column<DateTimeOffset>(nullable: false),
                    BeacukaiType = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconPreparingItems", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentSubconPreparingItems_GarmentSubconPreparings_GarmentSubconPreparingId",
                        column: x => x.GarmentSubconPreparingId,
                        principalTable: "GarmentSubconPreparings",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconPreparingItems_GarmentSubconPreparingId",
                table: "GarmentSubconPreparingItems",
                column: "GarmentSubconPreparingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentSubconPreparingItems");

            migrationBuilder.DropTable(
                name: "GarmentSubconPreparings");
        }
    }
}
