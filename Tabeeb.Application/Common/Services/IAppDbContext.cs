using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Domain.Entities;

namespace Tabeeb.Application.Common.Services
{
   public interface IAppDbContext
    {

        DbSet<AppUser> Users { get; set; }

        DbSet<Reservation> Reservations { get; set; }
        DbSet<PatientVital> PatientVitals { get; set; }
        DbSet<Permission> Permissions { get; set; }

        DbSet<RolePermissions> RolePermissions { get; set; }

        DbSet<Doctor> Doctors { get; set; }

        DbSet<Customer> Customers { get; set; }

        DbSet<Employee> Employees { get; set; }

        DbSet<City> Cities { get; set; }

        DbSet<Country> Countries { get; set; }

        DbSet<Specialization> Specializations { get; set; }

        DbSet<ClinicCustomer> ClinicCustomers { get; set; }

        DbSet<ClinicImage> ClinicImages { get; set; }

        DbSet<Clinic> Clinics { get; set; }

        DbSet<DoctorWorkTime> DoctorWorkTimes { get; set; }

        DbSet<Prescription> Prescriptions { get; set; }


        DbSet<SubscriptionPackage> SubscriptionPackages { get; set; }

        DbSet<Subscriptions> Subscriptions { get; set; }


        DbSet<DoctorClinic> doctorClinics { get; set; }

        DbSet<Evaluation> Evaluations { get; set; }

        DbSet<Offer> Offers { get; set; }

        DbSet<SubscriptionInvoices> SubscriptionInvoices { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        DbSet<TEntity> Set<TEntity>()
            where TEntity : class;

        DatabaseFacade Database { get; }

        IModel Model { get; }



    }
}
