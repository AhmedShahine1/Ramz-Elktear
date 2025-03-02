using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ramz_Elktear.core.Migrations
{
    /// <inheritdoc />
    public partial class deleteCity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Cities_CityId",
                schema: "dbo",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_CityId",
                schema: "dbo",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "CityId",
                schema: "dbo",
                table: "Booking");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CityId",
                schema: "dbo",
                table: "Booking",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Booking_CityId",
                schema: "dbo",
                table: "Booking",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Cities_CityId",
                schema: "dbo",
                table: "Booking",
                column: "CityId",
                principalSchema: "dbo",
                principalTable: "Cities",
                principalColumn: "Id");
        }
    }
}
