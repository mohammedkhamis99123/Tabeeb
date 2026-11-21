using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tabeeb.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOfferRelationToClinicDoctor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
    name: "OfferId",
    table: "DoctorClinic",
    type: "int",
    nullable: true);
            migrationBuilder.CreateIndex(
            name: "IX_DoctorClinic_OfferId",
            table: "DoctorClinic",
            column: "OfferId",
            unique: true,
            filter: "[OfferId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Offer_Doctor",
                table: "DoctorClinic",
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
