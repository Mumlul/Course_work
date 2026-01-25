using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace course_work.Migrations
{
    /// <inheritdoc />
    public partial class changetestresult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "answers_json",
                table: "Test_results");

            migrationBuilder.DropColumn(
                name: "attempt_number",
                table: "Test_results");

            migrationBuilder.DropColumn(
                name: "started_at",
                table: "Test_results");

            migrationBuilder.DropColumn(
                name: "time_spent_seconds",
                table: "Test_results");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "answers_json",
                table: "Test_results",
                type: "json",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "attempt_number",
                table: "Test_results",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "started_at",
                table: "Test_results",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "time_spent_seconds",
                table: "Test_results",
                type: "int",
                nullable: true);
        }
    }
}
