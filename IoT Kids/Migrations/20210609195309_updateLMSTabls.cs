using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IoT_Kids.Migrations
{
    public partial class updateLMSTabls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Lesson",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Course",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Course",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "Lesson");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "Course");
        }
    }
}
