using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ramz_Elktear.core.Migrations
{
    /// <inheritdoc />
    public partial class updateEngineModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EnginePositions_Images_ImageId",
                schema: "dbo",
                table: "EnginePositions");

            migrationBuilder.DropForeignKey(
                name: "FK_EngineSizes_Images_ImageId",
                schema: "dbo",
                table: "EngineSizes");

            migrationBuilder.AlterColumn<string>(
                name: "ImageId",
                schema: "dbo",
                table: "EngineSizes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ImageId",
                schema: "dbo",
                table: "EnginePositions",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_EnginePositions_Images_ImageId",
                schema: "dbo",
                table: "EnginePositions",
                column: "ImageId",
                principalSchema: "dbo",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EngineSizes_Images_ImageId",
                schema: "dbo",
                table: "EngineSizes",
                column: "ImageId",
                principalSchema: "dbo",
                principalTable: "Images",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EnginePositions_Images_ImageId",
                schema: "dbo",
                table: "EnginePositions");

            migrationBuilder.DropForeignKey(
                name: "FK_EngineSizes_Images_ImageId",
                schema: "dbo",
                table: "EngineSizes");

            migrationBuilder.AlterColumn<string>(
                name: "ImageId",
                schema: "dbo",
                table: "EngineSizes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ImageId",
                schema: "dbo",
                table: "EnginePositions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EnginePositions_Images_ImageId",
                schema: "dbo",
                table: "EnginePositions",
                column: "ImageId",
                principalSchema: "dbo",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EngineSizes_Images_ImageId",
                schema: "dbo",
                table: "EngineSizes",
                column: "ImageId",
                principalSchema: "dbo",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
