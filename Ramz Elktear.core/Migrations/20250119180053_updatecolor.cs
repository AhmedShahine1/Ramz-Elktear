using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ramz_Elktear.core.Migrations
{
    /// <inheritdoc />
    public partial class updatecolor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarColor_Cars_CarId",
                schema: "dbo",
                table: "CarColor");

            migrationBuilder.DropIndex(
                name: "IX_CarColor_CarId",
                schema: "dbo",
                table: "CarColor");

            migrationBuilder.DropColumn(
                name: "CarId",
                schema: "dbo",
                table: "CarColor");

            migrationBuilder.CreateTable(
                name: "CarCarColor",
                schema: "dbo",
                columns: table => new
                {
                    AvailableColorsId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CarsId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarCarColor", x => new { x.AvailableColorsId, x.CarsId });
                    table.ForeignKey(
                        name: "FK_CarCarColor_CarColor_AvailableColorsId",
                        column: x => x.AvailableColorsId,
                        principalSchema: "dbo",
                        principalTable: "CarColor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarCarColor_Cars_CarsId",
                        column: x => x.CarsId,
                        principalSchema: "dbo",
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarCarColor_CarsId",
                schema: "dbo",
                table: "CarCarColor",
                column: "CarsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarCarColor",
                schema: "dbo");

            migrationBuilder.AddColumn<string>(
                name: "CarId",
                schema: "dbo",
                table: "CarColor",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CarColor_CarId",
                schema: "dbo",
                table: "CarColor",
                column: "CarId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarColor_Cars_CarId",
                schema: "dbo",
                table: "CarColor",
                column: "CarId",
                principalSchema: "dbo",
                principalTable: "Cars",
                principalColumn: "Id");
        }
    }
}
