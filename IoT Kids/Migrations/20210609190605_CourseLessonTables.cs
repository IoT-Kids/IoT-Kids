using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IoT_Kids.Migrations
{
    public partial class CourseLessonTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Image = table.Column<byte[]>(nullable: true),
                    Lang = table.Column<string>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    Ref01 = table.Column<int>(nullable: false),
                    Ref02 = table.Column<int>(nullable: false),
                    Ref03 = table.Column<string>(nullable: true),
                    Refo4 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lesson",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseID = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    VideoURL = table.Column<string>(nullable: true),
                    SampleVideo = table.Column<bool>(nullable: false),
                    Index = table.Column<int>(nullable: false),
                    HasTest = table.Column<bool>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    Ref01 = table.Column<int>(nullable: false),
                    Ref02 = table.Column<int>(nullable: false),
                    Ref03 = table.Column<string>(nullable: true),
                    Refo4 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lesson", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lesson_Course_CourseID",
                        column: x => x.CourseID,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lesson_CourseID",
                table: "Lesson",
                column: "CourseID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lesson");

            migrationBuilder.DropTable(
                name: "Course");
        }
    }
}
