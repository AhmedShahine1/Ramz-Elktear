using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ramz_Elktear.core.Migrations
{
    /// <inheritdoc />
    public partial class addInstallmentRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InstallmentRequests",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CarId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BankId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    JobId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    InsurancePercentageId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    InstallmentPlanId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MonthlyIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MonthlyObligations = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InstallmentPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstallmentRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstallmentRequests_Banks_BankId",
                        column: x => x.BankId,
                        principalSchema: "dbo",
                        principalTable: "Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstallmentRequests_Cars_CarId",
                        column: x => x.CarId,
                        principalSchema: "dbo",
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstallmentRequests_InstallmentPlans_InstallmentPlanId",
                        column: x => x.InstallmentPlanId,
                        principalSchema: "dbo",
                        principalTable: "InstallmentPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstallmentRequests_InsurancePercentages_InsurancePercentageId",
                        column: x => x.InsurancePercentageId,
                        principalSchema: "dbo",
                        principalTable: "InsurancePercentages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstallmentRequests_Jobs_JobId",
                        column: x => x.JobId,
                        principalSchema: "dbo",
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstallmentRequests_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InstallmentRequests_BankId",
                schema: "dbo",
                table: "InstallmentRequests",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_InstallmentRequests_CarId",
                schema: "dbo",
                table: "InstallmentRequests",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_InstallmentRequests_InstallmentPlanId",
                schema: "dbo",
                table: "InstallmentRequests",
                column: "InstallmentPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_InstallmentRequests_InsurancePercentageId",
                schema: "dbo",
                table: "InstallmentRequests",
                column: "InsurancePercentageId");

            migrationBuilder.CreateIndex(
                name: "IX_InstallmentRequests_JobId",
                schema: "dbo",
                table: "InstallmentRequests",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_InstallmentRequests_UserId",
                schema: "dbo",
                table: "InstallmentRequests",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InstallmentRequests",
                schema: "dbo");
        }
    }
}
