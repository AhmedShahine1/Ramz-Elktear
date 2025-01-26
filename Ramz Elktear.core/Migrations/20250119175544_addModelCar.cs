using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ramz_Elktear.core.Migrations
{
    /// <inheritdoc />
    public partial class addModelCar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_CarModel_ModelId",
                schema: "dbo",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Cars_CarId",
                schema: "dbo",
                table: "Images");

            migrationBuilder.DropTable(
                name: "CarDetails",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_Images_CarId",
                schema: "dbo",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "CarId",
                schema: "dbo",
                table: "Images");

            migrationBuilder.AlterColumn<string>(
                name: "ModelId",
                schema: "dbo",
                table: "Cars",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_CarModel_ModelId",
                schema: "dbo",
                table: "Cars",
                column: "ModelId",
                principalSchema: "dbo",
                principalTable: "CarModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_CarModel_ModelId",
                schema: "dbo",
                table: "Cars");

            migrationBuilder.AddColumn<string>(
                name: "CarId",
                schema: "dbo",
                table: "Images",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ModelId",
                schema: "dbo",
                table: "Cars",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "CarDetails",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Acceleration = table.Column<double>(type: "float", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Colors = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dimensions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Engine = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FuelConsumption = table.Column<double>(type: "float", nullable: false),
                    FuelType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrls = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KeyFeatures = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfSeats = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SafetySystems = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StorageCapacity = table.Column<int>(type: "int", nullable: false),
                    Transmission = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransmissionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarDetails", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Images_CarId",
                schema: "dbo",
                table: "Images",
                column: "CarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_CarModel_ModelId",
                schema: "dbo",
                table: "Cars",
                column: "ModelId",
                principalSchema: "dbo",
                principalTable: "CarModel",
                principalColumn: "Id");

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
