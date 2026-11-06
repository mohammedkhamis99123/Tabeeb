using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Common.Services;
using Tabeeb.Domain.Entities;
using Tabeeb.Infrastructure.DbConfiguration;
using Tabeeb.Infrastructure.Interceptors;

namespace Tabeeb.Infrastructure.DbContexts
{
    public class AppDbContext : IdentityDbContext<AppUser, AspNetRole,
        string,
        IdentityUserClaim<string>,
        IdentityUserRole<string>,
        IdentityUserLogin<string>,
        IdentityRoleClaim<string>,
        IdentityUserToken<string>>, IAppDbContext
    {
        private readonly ICurrentUserService currentUserService;
        private readonly Auditingnterceptor auditingnterceptor;

        public AppDbContext(
       DbContextOptions<AppDbContext> options,
       ICurrentUserService? currentUserService = null,
       Auditingnterceptor? auditingInterceptor = null)
       : base(options)
        {
            this.currentUserService = currentUserService;
            this.auditingnterceptor = auditingInterceptor;
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            GeneralConfig.RunConfig(builder);
            RuntQueryFilters(builder);
            

        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.AddInterceptors(auditingnterceptor);
        }
        private void RuntQueryFilters(ModelBuilder builder)
        {
            builder.Entity<Reservation>().HasQueryFilter(a => a.DeletedAt == null 
            &&a.DoctorID==currentUserService.DoctorId 
            && a.ClinicId==currentUserService.ClinicId) ;
            builder.Entity<PatientVital>().HasQueryFilter(a => a.DeletedAt == null);
            builder.Entity<Doctor>().HasQueryFilter(a => a.DeletedAt == null/*l && a.DoctorClinic.Any(a=>a.ClinicId==currentUserService.ClinicId)*/);
            builder.Entity<Customer>().HasQueryFilter(a => a.DeletedAt == null && a.ClinicCustomer.Any(a => a.ClinicId == currentUserService.ClinicId));
            builder.Entity<Employee>().HasQueryFilter(a => a.DeletedAt == null && a.ClinicId==currentUserService.ClinicId);
            builder.Entity<City>().HasQueryFilter(a => a.DeletedAt == null);
            builder.Entity<Country>().HasQueryFilter(a => a.DeletedAt == null);
            builder.Entity<Specialization>().HasQueryFilter(a => a.DeletedAt == null );
            builder.Entity<ClinicImage>().HasQueryFilter(a=>a.DeletedAt == null );
            builder.Entity<Clinic>().HasQueryFilter(a => a.DeletedAt == null);
            builder.Entity<Offer>().HasQueryFilter(a => a.DeletedAt == null);
            builder.Entity<DoctorWorkTime>().HasQueryFilter(a => a.DeletedAt == null);
            builder.Entity<Prescription>().HasQueryFilter(a => a.DeletedAt == null);
            builder.Entity<SubscriptionPackage>().HasQueryFilter(a => a.DeletedAt == null);
            builder.Entity<Evaluation>().HasQueryFilter(a => a.DeletedAt == null);


        }

        public DbSet<Reservation> Reservations {get;set;}
        public DbSet<PatientVital> PatientVitals {get;set;}
        public DbSet<Permission> Permissions {get;set;}
        public DbSet<RolePermissions> RolePermissions {get;set;}
        public DbSet<Doctor> Doctors {get;set;}
        public DbSet<Customer> Customers {get;set;}
        public DbSet<Employee> Employees {get;set;}
        public DbSet<City> Cities {get;set;}
        public DbSet<Country> Countries {get;set;}
        public DbSet<Specialization> Specializations {get;set;}
        public DbSet<ClinicCustomer> ClinicCustomers {get;set;}
        public DbSet<ClinicImage> ClinicImages {get;set;}
        public DbSet<Clinic> Clinics {get;set;}
        public DbSet<DoctorWorkTime> DoctorWorkTimes {get;set;}
        public DbSet<Prescription> Prescriptions {get;set;}
        public DbSet<SubscriptionPackage> SubscriptionPackages {get;set;}
        public DbSet<Subscriptions> Subscriptions {get;set;}
        public DbSet<DoctorClinic> doctorClinics { get ; set; }
        public DbSet<Evaluation> Evaluations { get; set; }
        public DbSet<AppUser> Users { get; set; }
        
        public  DbSet<Offer> Offers { get; set; }

        public DbSet<SubscriptionInvoices> SubscriptionInvoices { get; set; }



        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
