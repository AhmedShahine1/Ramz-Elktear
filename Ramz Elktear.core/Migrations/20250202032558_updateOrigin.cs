using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ramz_Elktear.core.Migrations
{
    /// <inheritdoc />
    public partial class updateOrigin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Origins_Images_ImageId",
                schema: "dbo",
                table: "Origins");

            migrationBuilder.DropIndex(
                name: "IX_Origins_ImageId",
                schema: "dbo",
                table: "Origins");

            migrationBuilder.DropColumn(
                name: "ImageId",
                schema: "dbo",
                table: "Origins");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageId",
                schema: "dbo",
                table: "Origins",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Origins_ImageId",
                schema: "dbo",
                table: "Origins",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Origins_Images_ImageId",
                schema: "dbo",
                table: "Origins",
                column: "ImageId",
                principalSchema: "dbo",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
