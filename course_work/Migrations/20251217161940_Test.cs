using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace course_work.Migrations
{
    /// <inheritdoc />
    public partial class Test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "UserType",
                columns: new[] { "id", "name" },
                values: new object[] { 1, "Читатель" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserType",
                keyColumn: "id",
                keyValue: 1);
        }
    }
}
