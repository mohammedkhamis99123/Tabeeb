using Microsoft.EntityFrameworkCore.Migrations;
using Tabeeb.Domain.Enums;

#nullable disable

namespace Tabeeb.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPermissionsOfSubscriptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "ArName", "EnName" },
                values: new object[,]
                {
     

        { (byte)Permission.AddSubscription, "اضافة اشتراك", "Add Subscription" },
        { (byte)Permission.CancelSubscription, "الغاء اشتراك", "Cancel Subscription" },
        { (byte)Permission.RenewSubscription, "تجديد اشتراك", "Renew Subscription" },
        { (byte)Permission.ShowSubscriptionInvoice, "عرض فاتورة الاشتراك", "Show Subscription Invoice" },
        { (byte)Permission.ShowSubscriptionHistory, "عرض سجل الاشتراكات", "Show Subscription History" },
                });


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValues: new object[]
                {
            (byte)Permission. AddSubscription,
            (byte)Permission.RenewSubscription,
            (byte)Permission.CancelSubscription,
            (byte)Permission.ShowSubscriptionHistory,
            (byte)Permission.ShowSubscriptionInvoice
                });
        }
    }
}
