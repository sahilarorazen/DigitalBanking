using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalBanking.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerEmailToLoanApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerEmail",
                table: "LoanApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerEmail",
                table: "LoanApplications");
        }
    }
}
