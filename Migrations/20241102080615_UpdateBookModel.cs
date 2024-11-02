using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBookModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Year",
                table: "Books",
                newName: "year");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Books",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Books",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "DateAdded",
                table: "Books",
                newName: "dateadded");

            migrationBuilder.RenameColumn(
                name: "CoverPhoto",
                table: "Books",
                newName: "coverphoto");

            migrationBuilder.RenameColumn(
                name: "Author",
                table: "Books",
                newName: "author");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Books",
                newName: "id");

            migrationBuilder.AlterColumn<int>(
                name: "year",
                table: "Books",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "year",
                table: "Books",
                newName: "Year");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "Books",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Books",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "dateadded",
                table: "Books",
                newName: "DateAdded");

            migrationBuilder.RenameColumn(
                name: "coverphoto",
                table: "Books",
                newName: "CoverPhoto");

            migrationBuilder.RenameColumn(
                name: "author",
                table: "Books",
                newName: "Author");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Books",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Year",
                table: "Books",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
