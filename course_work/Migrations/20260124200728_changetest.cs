using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace course_work.Migrations
{
    /// <inheritdoc />
    public partial class changetest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "attempts_allowed",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "time_limit_minutes",
                table: "Tests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "attempts_allowed",
                table: "Tests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "Tests",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "time_limit_minutes",
                table: "Tests",
                type: "int",
                nullable: true);
        }
    }
}
