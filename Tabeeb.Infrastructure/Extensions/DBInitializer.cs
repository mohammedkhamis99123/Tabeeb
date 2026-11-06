using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tabeeb.Domain.Entities;
using Tabeeb.Infrastructure.DbContexts;

namespace Tabeeb.Infrastructure.DbConfiguration
{
    public static class DbInitializer
    {
        public static async Task SeedDataAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AspNetRole>>();

            // 1. تطبيق المايجريشنز المعلقة عند التشغيل تلقائياً
            if ((await context.Database.GetPendingMigrationsAsync()).Any())
            {
                await context.Database.MigrateAsync();
            }

          

            // 3. إضافة كافة البيانات الأساسية والأطباء والعيادات
            await SeedAllEntitiesAsync(context, userManager);
        }

  

        private static async Task SeedAllEntitiesAsync(AppDbContext context, UserManager<AppUser> userManager)
        {
         
            if (await context.Clinics.IgnoreQueryFilters().AnyAsync())
            {
                return;
            }

        
           // -------------------------------------------------------------
           var egypt = new Country { ArName = "مصر", EnName = "Egypt", ISOCode = "EG" };
            var saudi = new Country { ArName = "المملكة العربية السعودية", EnName = "KSA", ISOCode = "KSA" };

            await context.Countries.AddRangeAsync(egypt, saudi);
            context.SaveChanges();

            // نأخذ الـ IDs في متغيرات أرقام بسيطة (int / Guid)
            var egyptId = egypt.Id;
            var saudiId = saudi.Id;

            // تنظيف الـ Tracker تماماً لقطع أي روابط أوبجيكت بالذاكرة
            context.ChangeTracker.Clear();

            // -------------------------------------------------------------
            // 2. إضافة المدن باستخدام الـ IDs البسيطة فقط
            // -------------------------------------------------------------
            var cairo = new City { ArName = "القاهرة", EnName = "Cairo", CountryId = egyptId, IsActive = true };
            var giza = new City { ArName = "الجيزة", EnName = "Giza", CountryId = egyptId, IsActive = true };
            var riyadh = new City { ArName = "الرياض", EnName = "Riyadh", CountryId = saudiId, IsActive = true };

            await context.Cities.AddRangeAsync(cairo, giza, riyadh);
            context.SaveChanges();

            var cairoId = cairo.Id;
            var gizaId = giza.Id;

            context.ChangeTracker.Clear();

            // -------------------------------------------------------------
            // 3. إضافة التخصصات
            // -------------------------------------------------------------
            var spec1 = new Specialization { ArName = "طب وجراحة العيون", EnName = "Ophthalmology" };
            await context.Specializations.AddAsync(spec1);
            context.SaveChanges();

            var spec1Id = spec1.Id;

            context.ChangeTracker.Clear();

            // -------------------------------------------------------------
            // 4. إضافة العيادات باستخدام CityId
            // -------------------------------------------------------------
            var clinic1 = new Clinic
            {
                ArName = "مركز النور التخصصي للعيون",
                EnName = "El-nour Clinic",
                ArDescription = "مركز النور التخصصي للعيون",
                EnDescription = "Elnour-clinic",
                StreetArName = "شارع التحرير - الدقي",
                StreetEnName = "Tahrir",
                Langitude = "333.33",
                Latitude = "33.333",
                CityId = gizaId,
                IsActive = true
            };

            var clinic2 = new Clinic
            {
                ArName = "مستشفى الحياة التخصصي",
                EnName = "El-Hayat Clinic",
                ArDescription = "مستشفى الحياة التخصصي",
                EnDescription = "Hayat-clinic",
                StreetArName = "شارع عباس العقاد - مدينة نصر",
                StreetEnName = "Madinat Nasr",
                Langitude = "333.33",
                Latitude = "33.333",
                CityId = cairoId,
                IsActive = true
            };

            await context.Clinics.AddRangeAsync(clinic1, clinic2);
            context.SaveChanges();

            var clinic1Id = clinic1.Id;
            var clinic2Id = clinic2.Id;

            context.ChangeTracker.Clear();

           // 5. إنشاء المستخدمين (Identity)
           // -------------------------------------------------------------
           var adminUser = new AppUser { UserName = "admin@tabeeb.com", Email = "admin@tabeeb.com", EmailConfirmed = true, PhoneNumber = "01000000000",EnName="admin",ArName="مدير النظام" };
            if ((await userManager.CreateAsync(adminUser, "Admin@123456")).Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            var doc1User = new AppUser { UserName = "dr.ahmed@tabeeb.com", Email = "dr.ahmed@tabeeb.com", EmailConfirmed = true, PhoneNumber = "01111111111" ,EnName="Ahmed",ArName="احمد"};
            var doc2User = new AppUser { UserName = "dr.sara@tabeeb.com", Email = "dr.sara@tabeeb.com", EmailConfirmed = true, PhoneNumber = "01222222222" , EnName = "Sara",ArName="سارة" };

            var res1 = await userManager.CreateAsync(doc1User, "Doctor@123456");
            var res2 = await userManager.CreateAsync(doc2User, "Doctor@123456");

            if (res1.Succeeded && res2.Succeeded)
            {
                await userManager.AddToRoleAsync(doc1User, "Doctor");
                await userManager.AddToRoleAsync(doc2User, "Doctor");

                context.ChangeTracker.Clear();

                // -------------------------------------------------------------
                // 6. إضافة بيانات الأطباء
                // -------------------------------------------------------------
                var doctor1 = new Doctor { UserId = doc1User.Id, ArName = "د. أحمد علي محمود", EnName = "DR. Ahmed Ali", SpecializationId = 1, IsActive = true,  IdentityNumber="111111111"};
                var doctor2 = new Doctor { UserId = doc2User.Id, ArName = "د. ياسر فرج", EnName = "DR. Yasser Farag", SpecializationId = 1, IsActive = true, IdentityNumber = "111111111" };

                await context.Doctors.AddRangeAsync(doctor1, doctor2);
    context.SaveChanges();

                var doctor1Id = doctor1.Id;
                var doctor2Id = doctor2.Id;

                context.ChangeTracker.Clear();

                // -------------------------------------------------------------
                // 7. ربط الأطباء بالعيادات (DoctorClinic)
                // -------------------------------------------------------------
                var doctorClinics = new List<DoctorClinic>
        {
            new DoctorClinic { DoctorId = 1, ClinicId = 1 ,SessionDuration = 20 ,SessionPrice=200},
            new DoctorClinic { DoctorId = 2, ClinicId = 2 ,SessionDuration = 20 ,SessionPrice=200},
            new DoctorClinic { DoctorId = 1, ClinicId = 2 ,SessionDuration = 20 ,SessionPrice=200}
        };

                await context.Set<DoctorClinic>().AddRangeAsync(doctorClinics);
    context.SaveChanges();

                context.ChangeTracker.Clear();

                // -------------------------------------------------------------
                // 8. إضافة مواعيد العمل
                // -------------------------------------------------------------
                var workTimes = new List<DoctorWorkTime>
        {
            new DoctorWorkTime
            {
                DoctorID = doctor1Id,
                ClinicId = 1,
                StartAt = new TimeOnly(10, 0, 0),
                EndAt = new TimeOnly(16, 0, 0)
            }
        };

                await context.DoctorWorkTimes.AddRangeAsync(workTimes);
    context.SaveChanges();

                context.ChangeTracker.Clear();
            }
        }
    }
}