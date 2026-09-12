using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DigitalBanking.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddLoanProductsAndSchemesSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "LoanProducts",
                columns: new[] { "Id", "IsActive", "ProductCode", "ProductName" },
                values: new object[,]
                {
                    { 1, true, "PL", "Personal Loan" },
                    { 2, true, "HL", "Home Loan" },
                    { 3, true, "VL", "Vehicle Loan" },
                    { 4, true, "BL", "Business Loan" }
                });

            migrationBuilder.InsertData(
                table: "LoanSchemes",
                columns: new[] { "Id", "InterestRate", "IsActive", "LoanProductId", "SchemeCode", "SchemeName" },
                values: new object[,]
                {
                    { 1, 10.50m, true, 1, "PL-SAL", "Salaried Personal Loan" },
                    { 2, 11.00m, true, 1, "PL-SELF", "Self Employed Personal Loan" },
                    { 3, 10.25m, true, 1, "PL-MED", "Medical Personal Loan" },
                    { 4, 11.50m, true, 1, "PL-TRV", "Travel Personal Loan" },
                    { 5, 8.50m, true, 2, "HL-PUR", "Home Purchase Loan" },
                    { 6, 8.25m, true, 2, "HL-CON", "Home Construction Loan" },
                    { 7, 8.75m, true, 2, "HL-REN", "Home Renovation Loan" },
                    { 8, 8.00m, true, 2, "HL-BT", "Home Balance Transfer" },
                    { 9, 9.00m, true, 3, "VL-NCAR", "New Car Loan" },
                    { 10, 10.50m, true, 3, "VL-UCAR", "Used Car Loan" },
                    { 11, 10.25m, true, 3, "VL-BIKE", "Two Wheeler Loan" },
                    { 12, 8.90m, true, 3, "VL-EV", "Electric Vehicle Loan" },
                    { 13, 12.00m, true, 4, "BL-WC", "Working Capital Loan" },
                    { 14, 11.50m, true, 4, "BL-MSME", "MSME Business Loan" },
                    { 15, 12.25m, true, 4, "BL-EQP", "Equipment Finance Loan" },
                    { 16, 13.00m, true, 4, "BL-STR", "Startup Business Loan" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LoanSchemes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "LoanSchemes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "LoanSchemes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "LoanSchemes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "LoanSchemes",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "LoanSchemes",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "LoanSchemes",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "LoanSchemes",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "LoanSchemes",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "LoanSchemes",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "LoanSchemes",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "LoanSchemes",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "LoanSchemes",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "LoanSchemes",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "LoanSchemes",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "LoanSchemes",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "LoanProducts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "LoanProducts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "LoanProducts",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "LoanProducts",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
