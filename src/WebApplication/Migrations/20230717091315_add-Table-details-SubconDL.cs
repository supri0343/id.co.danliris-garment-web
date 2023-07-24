using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class addTabledetailsSubconDL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UENId",
                table: "GarmentSubconDeliveryLetterOutItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UENNo",
                table: "GarmentSubconDeliveryLetterOutItems",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GarmentSubconDeliveryLetterOutDetails",
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
                    SubconDeliveryLetterOutItemId = table.Column<Guid>(nullable: false),
                    UENItemId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    ProductCode = table.Column<string>(maxLength: 25, nullable: true),
                    ProductName = table.Column<string>(maxLength: 100, nullable: true),
                    ProductRemark = table.Column<string>(maxLength: 2000, nullable: true),
                    DesignColor = table.Column<string>(maxLength: 2000, nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    UomId = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(maxLength: 50, nullable: true),
                    UomOutId = table.Column<int>(nullable: false),
                    UomOutUnit = table.Column<string>(maxLength: 50, nullable: true),
                    FabricType = table.Column<string>(maxLength: 255, nullable: true),
                    UENId = table.Column<int>(nullable: false),
                    UENNo = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentSubconDeliveryLetterOutDetails", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_GarmentSubconDeliveryLetterOutDetails_GarmentSubconDeliveryLetterOutItems_SubconDeliveryLetterOutItemId",
                        column: x => x.SubconDeliveryLetterOutItemId,
                        principalTable: "GarmentSubconDeliveryLetterOutItems",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconDeliveryLetterOutDetails_SubconDeliveryLetterOutItemId",
                table: "GarmentSubconDeliveryLetterOutDetails",
                column: "SubconDeliveryLetterOutItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentSubconDeliveryLetterOutDetails");

            migrationBuilder.DropColumn(
                name: "UENId",
                table: "GarmentSubconDeliveryLetterOutItems");

            migrationBuilder.DropColumn(
                name: "UENNo",
                table: "GarmentSubconDeliveryLetterOutItems");
        }
    }
}
