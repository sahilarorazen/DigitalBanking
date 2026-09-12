using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalBanking.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddProductAndSchemeToLoanApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "LoanApplications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SchemeId",
                table: "LoanApplications",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "SchemeId",
                table: "LoanApplications");
        }
    }
}
