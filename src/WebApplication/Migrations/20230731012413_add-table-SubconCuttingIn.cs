using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class addtableSubconCuttingIn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GarmentSubconCuttingIns",
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
                    CutInNo = table.Column<string>(maxLength: 25, nullable: true),
                    CuttingType = table.Column<string>(maxLength: 25, nullable: true),
                    CuttingFrom = table.Column<string>(maxLength: 25, nullable: true),
                    RONo = table.Column<string>(maxLength: 25, nullable: true),
                    Article = table.Column<string>(maxLength: 50, nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    UnitCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitName = table.Column<string>(maxLength: 100, nullable: true),
                    CuttingInDate = table.Column<DateTimeOffset>(nullable: false),
                    FC = table.Column<double>(nullable: false),
                    UId = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconCuttingIns", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSubconCuttingInItems",
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
                    CutInId = table.Column<Guid>(nullable: false),
                    PreparingId = table.Column<Guid>(nullable: false),
                    SewingOutId = table.Column<Guid>(nullable: false),
                    SewingOutNo = table.Column<string>(maxLength: 50, nullable: true),
                    UENId = table.Column<int>(nullable: false),
                    UENNo = table.Column<string>(maxLength: 100, nullable: true),
                    UId = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconCuttingInItems", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentSubconCuttingInItems_GarmentSubconCuttingIns_CutInId",
                        column: x => x.CutInId,
                        principalTable: "GarmentSubconCuttingIns",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSubconCuttingInDetails",
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
                    CutInItemId = table.Column<Guid>(nullable: false),
                    PreparingItemId = table.Column<Guid>(nullable: false),
                    SewingOutItemId = table.Column<Guid>(nullable: false),
                    SewingOutDetailId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    ProductCode = table.Column<string>(maxLength: 25, nullable: true),
                    ProductName = table.Column<string>(maxLength: 100, nullable: true),
                    DesignColor = table.Column<string>(maxLength: 2000, nullable: true),
                    FabricType = table.Column<string>(maxLength: 25, nullable: true),
                    PreparingQuantity = table.Column<double>(nullable: false),
                    PreparingUomId = table.Column<int>(nullable: false),
                    PreparingUomUnit = table.Column<string>(maxLength: 10, nullable: true),
                    CuttingInQuantity = table.Column<int>(nullable: false),
                    CuttingInUomId = table.Column<int>(nullable: false),
                    CuttingInUomUnit = table.Column<string>(maxLength: 10, nullable: true),
                    RemainingQuantity = table.Column<double>(nullable: false),
                    BasicPrice = table.Column<double>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    FC = table.Column<double>(nullable: false),
                    Color = table.Column<string>(maxLength: 1000, nullable: true),
                    UId = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconCuttingInDetails", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentSubconCuttingInDetails_GarmentSubconCuttingInItems_CutInItemId",
                        column: x => x.CutInItemId,
                        principalTable: "GarmentSubconCuttingInItems",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconCuttingInDetails_CutInItemId",
                table: "GarmentSubconCuttingInDetails",
                column: "CutInItemId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconCuttingInItems_CutInId",
                table: "GarmentSubconCuttingInItems",
                column: "CutInId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconCuttingIns_CutInNo",
                table: "GarmentSubconCuttingIns",
                column: "CutInNo",
                unique: true,
                filter: "[Deleted]=(0)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentSubconCuttingInDetails");

            migrationBuilder.DropTable(
                name: "GarmentSubconCuttingInItems");

            migrationBuilder.DropTable(
                name: "GarmentSubconCuttingIns");
        }
    }
}
