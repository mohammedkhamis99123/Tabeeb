using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tabeeb.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInvoiceIdToSubscriptionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "InvoiceId",
                table: "Subscription",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_InvoiceId",
                table: "Subscription",
                column: "InvoiceId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscription_Invoice",
                table: "Subscription",
                column: "InvoiceId",
                principalTable: "SubscriptionInvoices",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscription_Invoice",
                table: "Subscription");

            migrationBuilder.DropIndex(
                name: "IX_Subscription_InvoiceId",
                table: "Subscription");

            migrationBuilder.DropColumn(
                name: "InvoiceId",
                table: "Subscription");
        }
    }
}
