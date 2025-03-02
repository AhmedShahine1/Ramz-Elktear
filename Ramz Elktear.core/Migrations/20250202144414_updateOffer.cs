using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ramz_Elktear.core.Migrations
{
    /// <inheritdoc />
    public partial class updateOffer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                schema: "dbo",
                table: "Offers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameEn",
                schema: "dbo",
                table: "Offers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameAr",
                schema: "dbo",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "NameEn",
                schema: "dbo",
                table: "Offers");
        }
    }
}
