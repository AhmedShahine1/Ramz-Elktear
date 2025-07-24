using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ramz_Elktear.core.Migrations
{
    /// <inheritdoc />
    public partial class AddColorToImageCar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ColorsId",
                schema: "dbo",
                table: "CarImages",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CarImages_ColorsId",
                schema: "dbo",
                table: "CarImages",
                column: "ColorsId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarImages_Colors_ColorsId",
                schema: "dbo",
                table: "CarImages",
                column: "ColorsId",
                principalSchema: "dbo",
                principalTable: "Colors",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarImages_Colors_ColorsId",
                schema: "dbo",
                table: "CarImages");

            migrationBuilder.DropIndex(
                name: "IX_CarImages_ColorsId",
                schema: "dbo",
                table: "CarImages");

            migrationBuilder.DropColumn(
                name: "ColorsId",
                schema: "dbo",
                table: "CarImages");
        }
    }
}
