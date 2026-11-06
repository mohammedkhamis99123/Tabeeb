using Microsoft.EntityFrameworkCore.Migrations;
using Tabeeb.Domain.Enums;

#nullable disable

namespace Tabeeb.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {


migrationBuilder.InsertData(
    table: "Permissions",
    columns: new[] { "Id", "ArName", "EnName" },
    values: new object[,]
    {
        { (byte)Permission.AddClinicDoctors, "إضافة أطباء العيادة", "Add Clinic Doctors" },
        { (byte)Permission.DeleteClinicDoctors, "حذف أطباء العيادة", "Delete Clinic Doctors" },

        { (byte)Permission.ViewDoctorWorkTime, "عرض أوقات عمل الطبيب", "View Doctor Work Time" },
        { (byte)Permission.UpdateDoctorWorkTime, "تحديث أوقات عمل الطبيب", "Update Doctor Work Time" },

        { (byte)Permission.ViewOffer, "عرض العروض", "View Offer" },
        { (byte)Permission.AddOffer, "إضافة عرض", "Add Offer" },
        { (byte)Permission.UpdateOffer, "تعديل عرض", "Update Offer" },
        { (byte)Permission.DeleteOffer, "حذف عرض", "Delete Offer" },

        { (byte)Permission.ViewRolePermissions, "عرض صلاحيات الأدوار", "View Role Permissions" },
        { (byte)Permission.UpdateRolePermissions, "تحديث صلاحيات الأدوار", "Update Role Permissions" },

        { (byte)Permission.ViewSpecialization, "عرض التخصصات", "View Specialization" },
        { (byte)Permission.AddSpecialization, "إضافة تخصص", "Add Specialization" },
        { (byte)Permission.UpdateSpecialization, "تعديل تخصص", "Update Specialization" },
        { (byte)Permission.DeleteSpecialization, "حذف تخصص", "Delete Specialization" },

        { (byte)Permission.ViewSubscriptionPackage, "عرض باقات الاشتراك", "View Subscription Package" },
        { (byte)Permission.AddSubscriptionPackage, "إضافة باقة اشتراك", "Add Subscription Package" },
        { (byte)Permission.UpdateSubscriptionPackage, "تعديل باقة اشتراك", "Update Subscription Package" },
        { (byte)Permission.DeleteSubscriptionPackage, "حذف باقة اشتراك", "Delete Subscription Package" },
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
            (byte)Permission.AddClinicDoctors,
            (byte)Permission.DeleteClinicDoctors,
            (byte)Permission.ViewDoctorWorkTime,
            (byte)Permission.UpdateDoctorWorkTime,
            (byte)Permission.ViewOffer,
            (byte)Permission.AddOffer,
            (byte)Permission.UpdateOffer,
            (byte)Permission.DeleteOffer,
            (byte)Permission.ViewRolePermissions,
            (byte)Permission.UpdateRolePermissions,
            (byte)Permission.ViewSpecialization,
            (byte)Permission.AddSpecialization,
            (byte)Permission.UpdateSpecialization,
            (byte)Permission.DeleteSpecialization,
            (byte)Permission.ViewSubscriptionPackage,
            (byte)Permission.AddSubscriptionPackage,
            (byte)Permission.UpdateSubscriptionPackage,
            (byte)Permission.DeleteSubscriptionPackage
                });
        }
    }
}
