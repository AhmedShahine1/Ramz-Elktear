using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ramz_Elktear.core.Migrations
{
    /// <inheritdoc />
    public partial class updateCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Images_ImageId",
                schema: "dbo",
                table: "Categories");

            migrationBuilder.AlterColumn<string>(
                name: "ImageId",
                schema: "dbo",
                table: "Categories",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Images_ImageId",
                schema: "dbo",
                table: "Categories",
                column: "ImageId",
                principalSchema: "dbo",
                principalTable: "Images",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Images_ImageId",
                schema: "dbo",
                table: "Categories");

            migrationBuilder.AlterColumn<string>(
                name: "ImageId",
                schema: "dbo",
                table: "Categories",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Images_ImageId",
                schema: "dbo",
                table: "Categories",
                column: "ImageId",
                principalSchema: "dbo",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
