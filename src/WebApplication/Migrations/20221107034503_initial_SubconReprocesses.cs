using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class initial_SubconReprocesses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GarmentSubconReprocesses",
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
                    ReprocessNo = table.Column<string>(maxLength: 20, nullable: true),
                    ReprocessType = table.Column<string>(maxLength: 50, nullable: true),
                    Date = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconReprocesses", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSubconReprocessItems",
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
                    ReprocessId = table.Column<Guid>(nullable: false),
                    ServiceSubconSewingId = table.Column<Guid>(nullable: false),
                    ServiceSubconSewingNo = table.Column<string>(maxLength: 25, nullable: true),
                    ServiceSubconSewingItemId = table.Column<Guid>(nullable: false),
                    ServiceSubconCuttingId = table.Column<Guid>(nullable: false),
                    ServiceSubconCuttingNo = table.Column<string>(maxLength: 25, nullable: true),
                    ServiceSubconCuttingItemId = table.Column<Guid>(nullable: false),
                    RONo = table.Column<string>(maxLength: 20, nullable: true),
                    Article = table.Column<string>(maxLength: 50, nullable: true),
                    ComodityId = table.Column<int>(nullable: false),
                    ComodityCode = table.Column<string>(maxLength: 255, nullable: true),
                    ComodityName = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconReprocessItems", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentSubconReprocessItems_GarmentSubconReprocesses_ServiceSubconCuttingId",
                        column: x => x.ServiceSubconCuttingId,
                        principalTable: "GarmentSubconReprocesses",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GarmentSubconReprocessDetails",
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
                    ReprocessItemId = table.Column<Guid>(nullable: false),
                    SizeId = table.Column<int>(nullable: false),
                    SizeName = table.Column<string>(nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    ReprocessQuantity = table.Column<double>(nullable: false),
                    UomId = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(maxLength: 20, nullable: true),
                    Color = table.Column<string>(maxLength: 2000, nullable: true),
                    DesignColor = table.Column<string>(maxLength: 2000, nullable: true),
                    ServiceSubconCuttingDetailId = table.Column<Guid>(nullable: false),
                    ServiceSubconCuttingSizeId = table.Column<Guid>(nullable: false),
                    ServiceSubconSewingDetailId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconReprocessDetails", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentSubconReprocessDetails_GarmentSubconReprocessItems_ReprocessItemId",
                        column: x => x.ReprocessItemId,
                        principalTable: "GarmentSubconReprocessItems",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconReprocessDetails_ReprocessItemId",
                table: "GarmentSubconReprocessDetails",
                column: "ReprocessItemId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconReprocesses_ReprocessNo",
                table: "GarmentSubconReprocesses",
                column: "ReprocessNo",
                unique: true,
                filter: "[Deleted]=(0)");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconReprocessItems_ServiceSubconCuttingId",
                table: "GarmentSubconReprocessItems",
                column: "ServiceSubconCuttingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentSubconReprocessDetails");

            migrationBuilder.DropTable(
                name: "GarmentSubconReprocessItems");

            migrationBuilder.DropTable(
                name: "GarmentSubconReprocesses");
        }
    }
}
