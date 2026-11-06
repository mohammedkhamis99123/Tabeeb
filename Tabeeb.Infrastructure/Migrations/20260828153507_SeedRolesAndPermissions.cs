using Microsoft.EntityFrameworkCore.Migrations;
using Tabeeb.Domain.Enums;

#nullable disable

namespace Tabeeb.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedRolesAndPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
           table: "Permissions",
           columns: new[] { "Id", "ArName", "EnName" },
           values: new object[,]
           {
                 { (byte)Permission.AllPermission, "كل الصلاحيات", "All Permission" },
                 { (byte)Permission.ViewCountry, "عرض الدول", "View Country" },
                 { (byte)Permission.AddCountry, "اضافة دولة", "Add Country" },
                 { (byte)Permission.UpdateCountry, "تعديل دولة", "Update Country" },
                 { (byte)Permission.DeleteCountry, "حذف دولة", "Delete Country" },
                 { (byte)Permission.ViewCity, "عرض المدن", "View City" },
                 { (byte)Permission.AddCity, "اضافة مدينة", "Add City" },
                 { (byte)Permission.UpdateCity, "تعديل مدينة", "Update City" },
                 { (byte)Permission.DeleteCity, "حذف مدينة", "Delete City" },
                 { (byte)Permission.ViewClinic, "عرض العيادات", "View Clinic" },
                 { (byte)Permission.AddClinic, "اضافة عيادة", "Add Clinic" },
                 { (byte)Permission.UpdateClinic, "تعديل عيادة", "Update Clinic" },
                 { (byte)Permission.DeleteClinic, "حذف عيادة", "Delete Clinic" },
                 { (byte)Permission.ViewClinicDoctors, "عرض اطباء العيادة", "View Clinic Doctors" },
                 { (byte)Permission.UpdateClinicDoctors, "تحديث اطباء العيادة", "Update Clinic Doctors" },
                 { (byte)Permission.ViewClinicImages, "عرض صور العيادة", "View Clinic Images" },
                 { (byte)Permission.UpdateClinicImages, "تحديث صور العيادة", " Update Clinic Images" },
                    { (byte)Permission.ViewEmployee, "عرض الموظفين", "View Employee" },
                    { (byte)Permission.AddEmployee, "اضافة موظف", "Add Employee" },
                    { (byte)Permission.UpdateEmployee, "تعديل موظف", "Update Employee" },
                    { (byte)Permission.DeleteEmployee, "حذف موظف", "Delete Employee" },
                    { (byte)Permission.ViewCustomer, "عرض العملاء", "View Customer" },
                    { (byte)Permission.AddCustomer, "اضافة عميل", "Add Cutomer" },
                    { (byte)Permission.UpdateCustomer, "تعديل عميل", "Update Customer" },
                    { (byte)Permission.DeleteCustomer, "حذف عميل", "Delete Customer" },



           });
            migrationBuilder.InsertData(
       "AspNetRoles",
      columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
       values: new object[,]
   {
     {
           Guid.NewGuid().ToString(),
           Roles.SuperAdmin,
           "SUPERADMIN",
           Guid.NewGuid().ToString(),

       },
     {
           Guid.NewGuid().ToString(),
           Roles.Accountant,
           "Accountant",
           Guid.NewGuid().ToString(),

       },
     {
           Guid.NewGuid().ToString(),
           Roles.Employee,
           "Employee",
           Guid.NewGuid().ToString(),

       },
     {
           Guid.NewGuid().ToString(),
           Roles.Admin,
           "Admin",
           Guid.NewGuid().ToString(),

       },
     {
           Guid.NewGuid().ToString(),
           Roles.Doctor,
           "Doctor",
           Guid.NewGuid().ToString(),

       }, {
           Guid.NewGuid().ToString(),
           Roles.DoctorAssistant,
           "DoctorAssistant",
           Guid.NewGuid().ToString(),

       },
      {
            Guid.NewGuid().ToString(),
            Roles.Customer,
            "CUSTOMER",
            Guid.NewGuid().ToString(),

        },

   });

        }
   

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        { // إزالة البيانات المضافة حسَب الـ Primary Key لكل عنصر مفرد
            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: (byte)Permission.AllPermission);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: (byte)Permission.AddCountry);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: (byte)Permission.UpdateCountry);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: (byte)Permission.DeleteCountry);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: (byte)Permission.AddCity);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: (byte)Permission.UpdateCity);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: (byte)Permission.DeleteCity);


            migrationBuilder.DeleteData(
                      table: "AspNetRoles",
                      keyColumn: "Name",
                      keyValues: new object[]
                      {
     Roles.SuperAdmin,
     Roles.Admin,
     Roles.Accountant,
     Roles.Doctor,
     Roles.Employee,
     Roles.DoctorAssistant,
     Roles.Customer


                      });


        }
    }
}
