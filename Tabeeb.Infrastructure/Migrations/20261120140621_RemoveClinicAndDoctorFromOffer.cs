using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tabeeb.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveClinicAndDoctorFromOffer : Migration
    {
        /// <inheritdoc />

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Remove old Offer relationships
            migrationBuilder.DropForeignKey(
                name: "FK_Offer_Clinic",
                table: "Offers");

            migrationBuilder.DropForeignKey(
                name: "FK_Offer_Doctor",
                table: "Offers");

            // Remove indexes if they exist
            migrationBuilder.DropIndex(
                name: "IX_Offers_ClinicId",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_Offers_DoctorId",
                table: "Offers");

            // Remove columns completely
            migrationBuilder.DropColumn(
                name: "ClinicId",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "Offers");
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offer_Doctor",
                table: "DoctorClinic");

            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Clinic_ClinicId",
                table: "Offers");

            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Doctor_DoctorId",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_DoctorClinic_OfferId",
                table: "DoctorClinic");

            migrationBuilder.DropColumn(
                name: "TotalAfterDiscount",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "OfferId",
                table: "DoctorClinic");

            migrationBuilder.RenameColumn(
                name: "TotalBeforeDiscount",
                table: "Reservation",
                newName: "TotalAmount");

            migrationBuilder.AddColumn<long>(
                name: "ReservationNo",
                table: "Reservation",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<int>(
                name: "DoctorId",
                table: "Offers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ClinicId",
                table: "Offers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Offer_Clinic",
                table: "Offers",
                column: "ClinicId",
                principalTable: "Clinic",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Offer_Doctor",
                table: "Offers",
                column: "DoctorId",
                principalTable: "Doctor",
                principalColumn: "Id");
        }
    }
}
