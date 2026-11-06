using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tabeeb.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class HandleOffers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RolePermissions",
                table: "RolePermissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Offer",
                table: "Offer");

            migrationBuilder.RenameTable(
                name: "Offer",
                newName: "Offers");

            migrationBuilder.RenameIndex(
                name: "IX_Offer_UpdatedBy",
                table: "Offers",
                newName: "IX_Offers_UpdatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Offer_IsActive_DeletedAt",
                table: "Offers",
                newName: "IX_Offers_IsActive_DeletedAt");

            migrationBuilder.RenameIndex(
                name: "IX_Offer_DoctorId",
                table: "Offers",
                newName: "IX_Offers_DoctorId");

            migrationBuilder.RenameIndex(
                name: "IX_Offer_DeletedBy",
                table: "Offers",
                newName: "IX_Offers_DeletedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Offer_CreatedBy",
                table: "Offers",
                newName: "IX_Offers_CreatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Offer_ClinicId",
                table: "Offers",
                newName: "IX_Offers_ClinicId");

            migrationBuilder.AddColumn<string>(
                name: "ArDescription",
                table: "SubscriptionPackage",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EnDescription",
                table: "SubscriptionPackage",
                type: "varchar(500)",
                unicode: false,
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "SubscriptionPackage",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ClinicId",
                table: "RolePermissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OfferId",
                table: "Reservation",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RolePermissions",
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId", "ClinicId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Offers",
                table: "Offers",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_ClinicId",
                table: "RolePermissions",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_OfferId",
                table: "Reservation",
                column: "OfferId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_Offer",
                table: "Reservation",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermission_Clinic",
                table: "RolePermissions",
                column: "ClinicId",
                principalTable: "Clinic",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_Offer",
                table: "Reservation");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermission_Clinic",
                table: "RolePermissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RolePermissions",
                table: "RolePermissions");

            migrationBuilder.DropIndex(
                name: "IX_RolePermissions_ClinicId",
                table: "RolePermissions");

            migrationBuilder.DropIndex(
                name: "IX_Reservation_OfferId",
                table: "Reservation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Offers",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "ArDescription",
                table: "SubscriptionPackage");

            migrationBuilder.DropColumn(
                name: "EnDescription",
                table: "SubscriptionPackage");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "SubscriptionPackage");

            migrationBuilder.DropColumn(
                name: "ClinicId",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "OfferId",
                table: "Reservation");

            migrationBuilder.RenameTable(
                name: "Offers",
                newName: "Offer");

            migrationBuilder.RenameIndex(
                name: "IX_Offers_UpdatedBy",
                table: "Offer",
                newName: "IX_Offer_UpdatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Offers_IsActive_DeletedAt",
                table: "Offer",
                newName: "IX_Offer_IsActive_DeletedAt");

            migrationBuilder.RenameIndex(
                name: "IX_Offers_DoctorId",
                table: "Offer",
                newName: "IX_Offer_DoctorId");

            migrationBuilder.RenameIndex(
                name: "IX_Offers_DeletedBy",
                table: "Offer",
                newName: "IX_Offer_DeletedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Offers_CreatedBy",
                table: "Offer",
                newName: "IX_Offer_CreatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Offers_ClinicId",
                table: "Offer",
                newName: "IX_Offer_ClinicId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RolePermissions",
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Offer",
                table: "Offer",
                column: "Id");
        }
    }
}
