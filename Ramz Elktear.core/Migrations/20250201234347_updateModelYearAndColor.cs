using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ramz_Elktear.core.Migrations
{
    /// <inheritdoc />
    public partial class updateModelYearAndColor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Colors_Images_ImageId",
                schema: "dbo",
                table: "Colors");

            migrationBuilder.DropForeignKey(
                name: "FK_ModelYears_Images_ImageId",
                schema: "dbo",
                table: "ModelYears");

            migrationBuilder.DropIndex(
                name: "IX_ModelYears_ImageId",
                schema: "dbo",
                table: "ModelYears");

            migrationBuilder.DropIndex(
                name: "IX_Colors_ImageId",
                schema: "dbo",
                table: "Colors");

            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                schema: "dbo",
                table: "ModelYears");

            migrationBuilder.DropColumn(
                name: "DescriptionEn",
                schema: "dbo",
                table: "ModelYears");

            migrationBuilder.DropColumn(
                name: "ImageId",
                schema: "dbo",
                table: "ModelYears");

            migrationBuilder.DropColumn(
                name: "NameAr",
                schema: "dbo",
                table: "ModelYears");

            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                schema: "dbo",
                table: "Colors");

            migrationBuilder.DropColumn(
                name: "DescriptionEn",
                schema: "dbo",
                table: "Colors");

            migrationBuilder.DropColumn(
                name: "ImageId",
                schema: "dbo",
                table: "Colors");

            migrationBuilder.RenameColumn(
                name: "NameEn",
                schema: "dbo",
                table: "ModelYears",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "NameEn",
                schema: "dbo",
                table: "Colors",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "NameAr",
                schema: "dbo",
                table: "Colors",
                newName: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "dbo",
                table: "ModelYears",
                newName: "NameEn");

            migrationBuilder.RenameColumn(
                name: "Value",
                schema: "dbo",
                table: "Colors",
                newName: "NameEn");

            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "dbo",
                table: "Colors",
                newName: "NameAr");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                schema: "dbo",
                table: "ModelYears",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEn",
                schema: "dbo",
                table: "ModelYears",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageId",
                schema: "dbo",
                table: "ModelYears",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                schema: "dbo",
                table: "ModelYears",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                schema: "dbo",
                table: "Colors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEn",
                schema: "dbo",
                table: "Colors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageId",
                schema: "dbo",
                table: "Colors",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ModelYears_ImageId",
                schema: "dbo",
                table: "ModelYears",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Colors_ImageId",
                schema: "dbo",
                table: "Colors",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Colors_Images_ImageId",
                schema: "dbo",
                table: "Colors",
                column: "ImageId",
                principalSchema: "dbo",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModelYears_Images_ImageId",
                schema: "dbo",
                table: "ModelYears",
                column: "ImageId",
                principalSchema: "dbo",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
