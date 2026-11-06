using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Domain.Entities;

namespace Tabeeb.Infrastructure.DbConfiguration
{
    public static class ClinicCustomerConfig
    {
        public static void RunConfig(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClinicCustomer>(opt =>
            {
                opt.ToTable("ClinicCustomer");
                opt.HasKey(a => new { a.ClinicId, a.CustomerId });
                opt.HasOne(A => A.Customer)
                 .WithMany(a => a.ClinicCustomer)
                 .HasForeignKey(a => a.CustomerId)
                 .HasConstraintName("FK_ClinicCustomer_Customer")
                 .OnDelete(DeleteBehavior.NoAction);

                opt.HasOne(A => A.Clinics)
                .WithMany(a => a.Customers)
                .HasForeignKey(a => a.ClinicId)
                .HasConstraintName("FK_ClinicCustomer_Clinic")
                .OnDelete(DeleteBehavior.NoAction);


             //   opt.HasOne(A => A.Doctor)
             //.WithMany(C => C.Customers)
             //.HasForeignKey(a => a.DoctorId)
             //.HasConstraintName("FK_ClinicCustomer_Doctor")
             //.OnDelete(DeleteBehavior.NoAction);
            });
        }
    }

}
