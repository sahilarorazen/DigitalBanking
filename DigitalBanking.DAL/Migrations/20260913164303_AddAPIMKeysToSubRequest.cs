using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalBanking.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddAPIMKeysToSubRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PrimaryKey",
                table: "SubscriptionRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondaryKey",
                table: "SubscriptionRequests",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrimaryKey",
                table: "SubscriptionRequests");

            migrationBuilder.DropColumn(
                name: "SecondaryKey",
                table: "SubscriptionRequests");
        }
    }
}
