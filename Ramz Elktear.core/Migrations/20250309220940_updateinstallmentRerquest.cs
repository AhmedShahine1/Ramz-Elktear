using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ramz_Elktear.core.Migrations
{
    /// <inheritdoc />
    public partial class updateinstallmentRerquest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                schema: "dbo",
                table: "InstallmentRequests",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "InstallmentMonths",
                schema: "dbo",
                table: "InstallmentRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "gender",
                schema: "dbo",
                table: "InstallmentRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InstallmentMonths",
                schema: "dbo",
                table: "InstallmentRequests");

            migrationBuilder.DropColumn(
                name: "gender",
                schema: "dbo",
                table: "InstallmentRequests");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                schema: "dbo",
                table: "InstallmentRequests",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
