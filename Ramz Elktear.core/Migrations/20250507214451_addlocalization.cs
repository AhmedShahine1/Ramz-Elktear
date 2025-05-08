using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ramz_Elktear.core.Migrations
{
    /// <inheritdoc />
    public partial class addlocalization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LocalizationResources",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResourceKey = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ResourceGroup = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalizationResources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LocalizationChangeLogs",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResourceId = table.Column<int>(type: "int", nullable: false),
                    CultureCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    OldValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalizationChangeLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LocalizationChangeLogs_LocalizationResources_ResourceId",
                        column: x => x.ResourceId,
                        principalSchema: "dbo",
                        principalTable: "LocalizationResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LocalizationValues",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResourceId = table.Column<int>(type: "int", nullable: false),
                    CultureCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalizationValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LocalizationValues_LocalizationResources_ResourceId",
                        column: x => x.ResourceId,
                        principalSchema: "dbo",
                        principalTable: "LocalizationResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LocalizationChangeLogs_ResourceId",
                schema: "dbo",
                table: "LocalizationChangeLogs",
                column: "ResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_LocalizationResources_ResourceGroup",
                schema: "dbo",
                table: "LocalizationResources",
                column: "ResourceGroup");

            migrationBuilder.CreateIndex(
                name: "IX_LocalizationResources_ResourceKey",
                schema: "dbo",
                table: "LocalizationResources",
                column: "ResourceKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LocalizationValues_ResourceId_CultureCode",
                schema: "dbo",
                table: "LocalizationValues",
                columns: new[] { "ResourceId", "CultureCode" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LocalizationChangeLogs",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "LocalizationValues",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "LocalizationResources",
                schema: "dbo");
        }
    }
}
