using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class AddColoumBCAvalProductItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
             name: "BCDate",
             table: "GarmentAvalProductItems",
             nullable: true,
             defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "BCNo",
                table: "GarmentAvalProductItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "POSerialNumber",
                table: "GarmentAvalProductItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BCType",
                table: "GarmentAvalProductItems",
                nullable: true);
        }


        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                 name: "BCDate",
                 table: "GarmentAvalProductItems");

            migrationBuilder.DropColumn(
                name: "BCNo",
                table: "GarmentAvalProductItems");

            migrationBuilder.DropColumn(
                name: "POSerialNumber",
                table: "GarmentAvalProductItems");

            migrationBuilder.DropColumn(
                name: "BCType",
                table: "GarmentAvalProductItems");

        }
    }
}
