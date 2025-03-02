using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ramz_Elktear.core.Migrations
{
    /// <inheritdoc />
    public partial class updateOption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Options_Images_ImageId",
                schema: "dbo",
                table: "Options");

            migrationBuilder.DropIndex(
                name: "IX_Options_ImageId",
                schema: "dbo",
                table: "Options");

            migrationBuilder.DropColumn(
                name: "ImageId",
                schema: "dbo",
                table: "Options");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageId",
                schema: "dbo",
                table: "Options",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Options_ImageId",
                schema: "dbo",
                table: "Options",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Options_Images_ImageId",
                schema: "dbo",
                table: "Options",
                column: "ImageId",
                principalSchema: "dbo",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
