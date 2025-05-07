using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ramz_Elktear.core.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInstallmentRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InstallmentRequests_InstallmentPlans_InstallmentPlanId",
                schema: "dbo",
                table: "InstallmentRequests");

            migrationBuilder.DropTable(
                name: "InstallmentPlans",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_InstallmentRequests_InstallmentPlanId",
                schema: "dbo",
                table: "InstallmentRequests");

            migrationBuilder.DropColumn(
                name: "InstallmentPlanId",
                schema: "dbo",
                table: "InstallmentRequests");

            migrationBuilder.AddColumn<bool>(
                name: "IsConvertable",
                schema: "dbo",
                table: "Jobs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Percentage",
                schema: "dbo",
                table: "Jobs",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsConvertable",
                schema: "dbo",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "Percentage",
                schema: "dbo",
                table: "Jobs");

            migrationBuilder.AddColumn<string>(
                name: "InstallmentPlanId",
                schema: "dbo",
                table: "InstallmentRequests",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

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

            migrationBuilder.CreateIndex(
                name: "IX_InstallmentRequests_InstallmentPlanId",
                schema: "dbo",
                table: "InstallmentRequests",
                column: "InstallmentPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_InstallmentRequests_InstallmentPlans_InstallmentPlanId",
                schema: "dbo",
                table: "InstallmentRequests",
                column: "InstallmentPlanId",
                principalSchema: "dbo",
                principalTable: "InstallmentPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
