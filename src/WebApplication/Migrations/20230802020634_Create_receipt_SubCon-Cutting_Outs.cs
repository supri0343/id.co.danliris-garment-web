using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Create_receipt_SubConCutting_Outs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GarmentSubconCuttingOuts",
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
                    CutOutNo = table.Column<string>(maxLength: 25, nullable: true),
                    CuttingOutType = table.Column<string>(maxLength: 25, nullable: true),
                    UnitFromId = table.Column<int>(nullable: false),
                    UnitFromCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitFromName = table.Column<string>(maxLength: 100, nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    UnitCode = table.Column<string>(maxLength: 25, nullable: true),
                    UnitName = table.Column<string>(maxLength: 100, nullable: true),
                    RONo = table.Column<string>(maxLength: 25, nullable: true),
                    Article = table.Column<string>(maxLength: 50, nullable: true),
                    ComodityId = table.Column<int>(nullable: false),
                    ComodityCode = table.Column<string>(maxLength: 25, nullable: true),
                    ComodityName = table.Column<string>(maxLength: 100, nullable: true),
                    CuttingOutDate = table.Column<DateTimeOffset>(nullable: false),
                    EPOId = table.Column<long>(nullable: false),
                    EPOItemId = table.Column<long>(nullable: false),
                    POSerialNumber = table.Column<string>(maxLength: 100, nullable: true),
                    UId = table.Column<string>(nullable: true),
                    IsUsed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconCuttingOuts", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSubconCuttingOutItems",
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
                    CutOutId = table.Column<Guid>(nullable: false),
                    CuttingInId = table.Column<Guid>(nullable: false),
                    CuttingInDetailId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    ProductCode = table.Column<string>(maxLength: 25, nullable: true),
                    ProductName = table.Column<string>(maxLength: 100, nullable: true),
                    DesignColor = table.Column<string>(maxLength: 2000, nullable: true),
                    TotalCuttingOut = table.Column<double>(nullable: false),
                    UId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconCuttingOutItems", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentSubconCuttingOutItems_GarmentSubconCuttingOuts_CutOutId",
                        column: x => x.CutOutId,
                        principalTable: "GarmentSubconCuttingOuts",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSubconCuttingOutDetails",
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
                    CutOutItemId = table.Column<Guid>(nullable: false),
                    SizeId = table.Column<int>(nullable: false),
                    SizeName = table.Column<string>(maxLength: 100, nullable: true),
                    CuttingOutQuantity = table.Column<double>(nullable: false),
                    CuttingOutUomId = table.Column<int>(nullable: false),
                    CuttingOutUomUnit = table.Column<string>(maxLength: 10, nullable: true),
                    Color = table.Column<string>(maxLength: 1000, nullable: true),
                    RemainingQuantity = table.Column<double>(nullable: false),
                    BasicPrice = table.Column<double>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    UId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconCuttingOutDetails", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentSubconCuttingOutDetails_GarmentSubconCuttingOutItems_CutOutItemId",
                        column: x => x.CutOutItemId,
                        principalTable: "GarmentSubconCuttingOutItems",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconCuttingOutDetails_CutOutItemId",
                table: "GarmentSubconCuttingOutDetails",
                column: "CutOutItemId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconCuttingOutItems_CutOutId",
                table: "GarmentSubconCuttingOutItems",
                column: "CutOutId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconCuttingOuts_CutOutNo",
                table: "GarmentSubconCuttingOuts",
                column: "CutOutNo",
                unique: true,
                filter: "[Deleted]=(0)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentSubconCuttingOutDetails");

            migrationBuilder.DropTable(
                name: "GarmentSubconCuttingOutItems");

            migrationBuilder.DropTable(
                name: "GarmentSubconCuttingOuts");
        }
    }
}
