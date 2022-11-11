using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class add_FK_reprocessItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GarmentSubconReprocessItems_GarmentSubconReprocesses_ServiceSubconCuttingId",
                table: "GarmentSubconReprocessItems");

            migrationBuilder.DropIndex(
                name: "IX_GarmentSubconReprocessItems_ServiceSubconCuttingId",
                table: "GarmentSubconReprocessItems");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconReprocessItems_ReprocessId",
                table: "GarmentSubconReprocessItems",
                column: "ReprocessId");

            migrationBuilder.AddForeignKey(
                name: "FK_GarmentSubconReprocessItems_GarmentSubconReprocesses_ReprocessId",
                table: "GarmentSubconReprocessItems",
                column: "ReprocessId",
                principalTable: "GarmentSubconReprocesses",
                principalColumn: "Identity",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GarmentSubconReprocessItems_GarmentSubconReprocesses_ReprocessId",
                table: "GarmentSubconReprocessItems");

            migrationBuilder.DropIndex(
                name: "IX_GarmentSubconReprocessItems_ReprocessId",
                table: "GarmentSubconReprocessItems");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentSubconReprocessItems_ServiceSubconCuttingId",
                table: "GarmentSubconReprocessItems",
                column: "ServiceSubconCuttingId");

            migrationBuilder.AddForeignKey(
                name: "FK_GarmentSubconReprocessItems_GarmentSubconReprocesses_ServiceSubconCuttingId",
                table: "GarmentSubconReprocessItems",
                column: "ServiceSubconCuttingId",
                principalTable: "GarmentSubconReprocesses",
                principalColumn: "Identity",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
