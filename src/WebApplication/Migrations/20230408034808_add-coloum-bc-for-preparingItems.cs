using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class addcoloumbcforpreparingItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BCDate",
                table: "GarmentPreparingItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BCNo",
                table: "GarmentPreparingItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BCType",
                table: "GarmentPreparingItems",
                nullable: true);

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "BCDate",
            //    table: "GarmentAvalProductItems",
            //    nullable: true,
            //    oldClrType: typeof(DateTime));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BCDate",
                table: "GarmentPreparingItems");

            migrationBuilder.DropColumn(
                name: "BCNo",
                table: "GarmentPreparingItems");

            migrationBuilder.DropColumn(
                name: "BCType",
                table: "GarmentPreparingItems");

            //    migrationBuilder.AlterColumn<DateTime>(
            //        name: "BCDate",
            //        table: "GarmentAvalProductItems",
            //        nullable: false,
            //        oldClrType: typeof(DateTime),
            //        oldNullable: true);
        }
    }
}
