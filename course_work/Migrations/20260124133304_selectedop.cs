using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace course_work.Migrations
{
    /// <inheritdoc />
    public partial class selectedop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_selected",
                table: "Test_question_options",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_selected",
                table: "Test_question_options");
        }
    }
}
