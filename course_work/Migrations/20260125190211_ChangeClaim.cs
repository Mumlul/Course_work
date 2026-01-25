using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace course_work.Migrations
{
    /// <inheritdoc />
    public partial class ChangeClaim : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "fix_days",
                table: "User_complaints",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "in_progress",
                table: "User_complaints",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "fix_days",
                table: "Course_complaints",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "in_progress",
                table: "Course_complaints",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fix_days",
                table: "User_complaints");

            migrationBuilder.DropColumn(
                name: "in_progress",
                table: "User_complaints");

            migrationBuilder.DropColumn(
                name: "fix_days",
                table: "Course_complaints");

            migrationBuilder.DropColumn(
                name: "in_progress",
                table: "Course_complaints");
        }
    }
}
