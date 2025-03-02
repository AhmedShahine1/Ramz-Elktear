using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ramz_Elktear.core.Migrations
{
    /// <inheritdoc />
    public partial class addimages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CarId",
                schema: "dbo",
                table: "Images",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_CarId",
                schema: "dbo",
                table: "Images",
                column: "CarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Cars_CarId",
                schema: "dbo",
                table: "Images",
                column: "CarId",
                principalSchema: "dbo",
                principalTable: "Cars",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Cars_CarId",
                schema: "dbo",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_CarId",
                schema: "dbo",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "CarId",
                schema: "dbo",
                table: "Images");
        }
    }
}
