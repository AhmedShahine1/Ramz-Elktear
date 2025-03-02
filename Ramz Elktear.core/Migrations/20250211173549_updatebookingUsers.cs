using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ramz_Elktear.core.Migrations
{
    /// <inheritdoc />
    public partial class updatebookingUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Users_UserId",
                schema: "dbo",
                table: "Booking");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "dbo",
                table: "Booking",
                newName: "SellerId");

            migrationBuilder.RenameIndex(
                name: "IX_Booking_UserId",
                schema: "dbo",
                table: "Booking",
                newName: "IX_Booking_SellerId");

            migrationBuilder.AddColumn<string>(
                name: "BuyerId",
                schema: "dbo",
                table: "Booking",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Booking_BuyerId",
                schema: "dbo",
                table: "Booking",
                column: "BuyerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Users_BuyerId",
                schema: "dbo",
                table: "Booking",
                column: "BuyerId",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Users_SellerId",
                schema: "dbo",
                table: "Booking",
                column: "SellerId",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Users_BuyerId",
                schema: "dbo",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Users_SellerId",
                schema: "dbo",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_BuyerId",
                schema: "dbo",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "BuyerId",
                schema: "dbo",
                table: "Booking");

            migrationBuilder.RenameColumn(
                name: "SellerId",
                schema: "dbo",
                table: "Booking",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Booking_SellerId",
                schema: "dbo",
                table: "Booking",
                newName: "IX_Booking_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Users_UserId",
                schema: "dbo",
                table: "Booking",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
