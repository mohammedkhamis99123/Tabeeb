using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tabeeb.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditingForSubscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Subscription",
                table: "Subscription");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "Subscription",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Subscription",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Subscription",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Subscription",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Subscription",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Subscription",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Subscription",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subscription",
                table: "Subscription",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_ClinicId",
                table: "Subscription",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_CreatedBy",
                table: "Subscription",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_DeletedBy",
                table: "Subscription",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_UpdatedBy",
                table: "Subscription",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscription_Create",
                table: "Subscription",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscription_Delete",
                table: "Subscription",
                column: "DeletedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscription_Update",
                table: "Subscription",
                column: "UpdatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscription_Create",
                table: "Subscription");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscription_Delete",
                table: "Subscription");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscription_Update",
                table: "Subscription");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subscription",
                table: "Subscription");

            migrationBuilder.DropIndex(
                name: "IX_Subscription_ClinicId",
                table: "Subscription");

            migrationBuilder.DropIndex(
                name: "IX_Subscription_CreatedBy",
                table: "Subscription");

            migrationBuilder.DropIndex(
                name: "IX_Subscription_DeletedBy",
                table: "Subscription");

            migrationBuilder.DropIndex(
                name: "IX_Subscription_UpdatedBy",
                table: "Subscription");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Subscription");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Subscription");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Subscription");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Subscription");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Subscription");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Subscription");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Subscription");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subscription",
                table: "Subscription",
                columns: new[] { "ClinicId", "PackageID" });
        }
    }
}
