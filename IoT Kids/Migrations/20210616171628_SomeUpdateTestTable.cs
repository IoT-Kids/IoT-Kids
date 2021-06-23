using Microsoft.EntityFrameworkCore.Migrations;

namespace IoT_Kids.Migrations
{
    public partial class SomeUpdateTestTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Test_LessonId",
                table: "Test",
                column: "LessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Test_Lesson_LessonId",
                table: "Test",
                column: "LessonId",
                principalTable: "Lesson",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Test_Lesson_LessonId",
                table: "Test");

            migrationBuilder.DropIndex(
                name: "IX_Test_LessonId",
                table: "Test");
        }
    }
}
