using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tabeeb.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class HandleReservationTablesLimits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TotalBeforeDiscount",
                table: "Reservation",
                type: "decimal(8,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalAfterDiscount",
                table: "Reservation",
                type: "decimal(8,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TotalBeforeDiscount",
                table: "Reservation",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalAfterDiscount",
                table: "Reservation",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)");
        }
    }
}
