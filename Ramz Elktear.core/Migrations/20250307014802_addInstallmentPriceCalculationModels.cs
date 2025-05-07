using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ramz_Elktear.core.Migrations
{
    /// <inheritdoc />
    public partial class addInstallmentPriceCalculationModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Sector",
                schema: "dbo",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsAccapted",
                schema: "dbo",
                table: "Banks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "InstallmentPlans",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DownPaymentPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FinalPaymentPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsFinalPaymentBalloon = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstallmentPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InsurancePercentages",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MinAge = table.Column<int>(type: "int", nullable: false),
                    MaxAge = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsurancePercentages", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InstallmentPlans",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "InsurancePercentages",
                schema: "dbo");

            migrationBuilder.DropColumn(
                name: "Sector",
                schema: "dbo",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "IsAccapted",
                schema: "dbo",
                table: "Banks");
        }
    }
}
