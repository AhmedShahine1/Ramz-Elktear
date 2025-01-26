using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ramz_Elktear.core.Migrations
{
    /// <inheritdoc />
    public partial class addBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_CarCategory_CategoryId",
                schema: "dbo",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_Cars_CarModel_ModelId",
                schema: "dbo",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "FullName",
                schema: "dbo",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "ModelId",
                schema: "dbo",
                table: "Cars",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CategoryId",
                schema: "dbo",
                table: "Cars",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "Cities",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Booking",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CarId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booking", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Booking_Cars_CarId",
                        column: x => x.CarId,
                        principalSchema: "dbo",
                        principalTable: "Cars",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Booking_Cities_CityId",
                        column: x => x.CityId,
                        principalSchema: "dbo",
                        principalTable: "Cities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Booking_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Booking_CarId",
                schema: "dbo",
                table: "Booking",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_CityId",
                schema: "dbo",
                table: "Booking",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_UserId",
                schema: "dbo",
                table: "Booking",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_CarCategory_CategoryId",
                schema: "dbo",
                table: "Cars",
                column: "CategoryId",
                principalSchema: "dbo",
                principalTable: "CarCategory",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_CarModel_ModelId",
                schema: "dbo",
                table: "Cars",
                column: "ModelId",
                principalSchema: "dbo",
                principalTable: "CarModel",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_CarCategory_CategoryId",
                schema: "dbo",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_Cars_CarModel_ModelId",
                schema: "dbo",
                table: "Cars");

            migrationBuilder.DropTable(
                name: "Booking",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Cities",
                schema: "dbo");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                schema: "dbo",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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

            migrationBuilder.AlterColumn<string>(
                name: "CategoryId",
                schema: "dbo",
                table: "Cars",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_CarCategory_CategoryId",
                schema: "dbo",
                table: "Cars",
                column: "CategoryId",
                principalSchema: "dbo",
                principalTable: "CarCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
    }
}
