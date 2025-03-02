using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ramz_Elktear.core.Migrations
{
    /// <inheritdoc />
    public partial class addImageCar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "ImageCars",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CarId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ImageId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageCars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImageCars_Cars_CarId",
                        column: x => x.CarId,
                        principalSchema: "dbo",
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImageCars_CarId",
                schema: "dbo",
                table: "ImageCars",
                column: "CarId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImageCars",
                schema: "dbo");

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
    }
}
