using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tabeeb.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class HandleReservationTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "ReservationNo",
                table: "Reservation");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "Reservation",
                newName: "TotalBeforeDiscount");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAfterDiscount",
                table: "Reservation",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);
                        migrationBuilder.AddForeignKey(
                name: "FK_Reservation_Offer",
                table: "Reservation",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
