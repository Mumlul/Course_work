using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace course_work.Migrations
{
    /// <inheritdoc />
    public partial class Fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "passing_score",
                table: "Tests",
                type: "int",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint unsigned");

            migrationBuilder.AlterColumn<double>(
                name: "score",
                table: "Test_results",
                type: "double",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<int>(
                name: "progress_percent",
                table: "Course_students",
                type: "int",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint unsigned");

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "Course_reviews",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Course_complaints",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    course_id = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    complaint_text = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_resolved = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course_complaints", x => x.id);
                    table.ForeignKey(
                        name: "FK_Course_complaints_Courses_course_id",
                        column: x => x.course_id,
                        principalTable: "Courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Course_complaints_User_user_id",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "User_complaints",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    from_user_id = table.Column<int>(type: "int", nullable: false),
                    to_user_id = table.Column<int>(type: "int", nullable: false),
                    complaint_text = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_resolved = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_complaints", x => x.id);
                    table.ForeignKey(
                        name: "FK_User_complaints_User_from_user_id",
                        column: x => x.from_user_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_complaints_User_to_user_id",
                        column: x => x.to_user_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Course_reviews_UserId1",
                table: "Course_reviews",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Course_complaints_course_id",
                table: "Course_complaints",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "IX_Course_complaints_user_id",
                table: "Course_complaints",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_User_complaints_from_user_id_to_user_id",
                table: "User_complaints",
                columns: new[] { "from_user_id", "to_user_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_complaints_to_user_id",
                table: "User_complaints",
                column: "to_user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Course_reviews_User_UserId1",
                table: "Course_reviews",
                column: "UserId1",
                principalTable: "User",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Course_reviews_User_UserId1",
                table: "Course_reviews");

            migrationBuilder.DropTable(
                name: "Course_complaints");

            migrationBuilder.DropTable(
                name: "User_complaints");

            migrationBuilder.DropIndex(
                name: "IX_Course_reviews_UserId1",
                table: "Course_reviews");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Course_reviews");

            migrationBuilder.AlterColumn<byte>(
                name: "passing_score",
                table: "Tests",
                type: "tinyint unsigned",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "score",
                table: "Test_results",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double");

            migrationBuilder.AlterColumn<byte>(
                name: "progress_percent",
                table: "Course_students",
                type: "tinyint unsigned",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
