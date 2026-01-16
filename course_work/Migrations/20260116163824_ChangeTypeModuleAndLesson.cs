using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace course_work.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTypeModuleAndLesson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "PreviewImage",
                keyValue: null,
                column: "PreviewImage",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "PreviewImage",
                table: "Modules",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "longblob",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Lessons",
                keyColumn: "preview_image",
                keyValue: null,
                column: "preview_image",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "preview_image",
                table: "Lessons",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "BLOB",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "PreviewImage",
                table: "Modules",
                type: "longblob",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<byte[]>(
                name: "preview_image",
                table: "Lessons",
                type: "BLOB",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
