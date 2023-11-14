using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TitlesOrganizer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ExistingUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_BookSeries_BookSeriesId",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "BookSeriesId",
                table: "Books",
                newName: "SeriesId");

            migrationBuilder.RenameIndex(
                name: "IX_Books_BookSeriesId",
                table: "Books",
                newName: "IX_Books_SeriesId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "BookSeries",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_BookSeries_SeriesId",
                table: "Books",
                column: "SeriesId",
                principalTable: "BookSeries",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_BookSeries_SeriesId",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "SeriesId",
                table: "Books",
                newName: "BookSeriesId");

            migrationBuilder.RenameIndex(
                name: "IX_Books_SeriesId",
                table: "Books",
                newName: "IX_Books_BookSeriesId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "BookSeries",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_BookSeries_BookSeriesId",
                table: "Books",
                column: "BookSeriesId",
                principalTable: "BookSeries",
                principalColumn: "Id");
        }
    }
}
