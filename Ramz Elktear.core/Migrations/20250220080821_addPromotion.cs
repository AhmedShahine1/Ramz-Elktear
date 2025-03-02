using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ramz_Elktear.core.Migrations
{
    /// <inheritdoc />
    public partial class addPromotion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "latitude",
                schema: "dbo",
                table: "Branches",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "longitude",
                schema: "dbo",
                table: "Branches",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Promotion",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ImageArId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ImageEnId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Promotion_Images_ImageArId",
                        column: x => x.ImageArId,
                        principalSchema: "dbo",
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Promotion_Images_ImageEnId",
                        column: x => x.ImageEnId,
                        principalSchema: "dbo",
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Promotion_ImageArId",
                schema: "dbo",
                table: "Promotion",
                column: "ImageArId");

            migrationBuilder.CreateIndex(
                name: "IX_Promotion_ImageEnId",
                schema: "dbo",
                table: "Promotion",
                column: "ImageEnId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Promotion",
                schema: "dbo");

            migrationBuilder.DropColumn(
                name: "latitude",
                schema: "dbo",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "longitude",
                schema: "dbo",
                table: "Branches");
        }
    }
}
