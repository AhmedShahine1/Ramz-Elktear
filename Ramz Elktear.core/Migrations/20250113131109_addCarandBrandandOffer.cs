using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ramz_Elktear.core.Migrations
{
    /// <inheritdoc />
    public partial class addCarandBrandandOffer : Migration
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

            migrationBuilder.CreateTable(
                name: "Brands",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogoId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Brands_Images_LogoId",
                        column: x => x.LogoId,
                        principalSchema: "dbo",
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cars",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BrandsId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Engine = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Transmission = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Acceleration = table.Column<double>(type: "float", nullable: false),
                    FuelConsumption = table.Column<double>(type: "float", nullable: false),
                    Dimensions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StorageCapacity = table.Column<int>(type: "int", nullable: false),
                    KeyFeatures = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SafetySystems = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfSeats = table.Column<int>(type: "int", nullable: false),
                    FuelType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransmissionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Colors = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cars_Brands_BrandsId",
                        column: x => x.BrandsId,
                        principalSchema: "dbo",
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Offers",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CarId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NewPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImageId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Offers_Cars_CarId",
                        column: x => x.CarId,
                        principalSchema: "dbo",
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Images_CarId",
                schema: "dbo",
                table: "Images",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_Brands_LogoId",
                schema: "dbo",
                table: "Brands",
                column: "LogoId");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_BrandsId",
                schema: "dbo",
                table: "Cars",
                column: "BrandsId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_CarId",
                schema: "dbo",
                table: "Offers",
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

            migrationBuilder.DropTable(
                name: "Offers",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Cars",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Brands",
                schema: "dbo");

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
