using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Domain.Entities;

namespace Tabeeb.Infrastructure.DbConfiguration
{
    public static class EmployeeConfig
    {
        public static void RunConfig(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(opt =>
            {
                opt.ToTable("Employee");
                opt.Property(a => a.ArName).HasMaxLength(150);
                opt.Property(a => a.EnName).HasMaxLength(150).IsUnicode(false);
                opt.Property(a => a.Addrees).HasMaxLength(250);
                opt.HasOne(a => a.Doctor)
                 .WithMany(A => A.Employees)
                   .HasForeignKey(a => a.DoctorId)
                   .HasConstraintName("FK_Employee_Doctor")
                   .OnDelete(DeleteBehavior.NoAction)
                   .IsRequired(false);


                opt.HasOne(a => a.Clinics)
                .WithMany(A => A.Employees)
                  .HasForeignKey(a => a.ClinicId)
                  .HasConstraintName("FK_Employee_Clinic")
                  .OnDelete(DeleteBehavior.NoAction)
                  .IsRequired(true);


                opt.HasOne(a => a.AppUser)
                        .WithOne(A => A.Employee)
                        .HasForeignKey<Employee>(a => a.UserId)
                        .HasConstraintName("FK_Employee_User")
                        .OnDelete(DeleteBehavior.NoAction);



                opt.HasOne(a => a.Creator)
                .WithMany(A => A.EmployeeCreatedBy)
                .HasForeignKey(a => a.CreatedBy)
                .HasConstraintName("FK_Employee_Create")
                .OnDelete(DeleteBehavior.NoAction);

                opt.HasOne(a => a.Updater)
                  .WithMany(A => A.EmployeeUpdatedBy)
                  .HasForeignKey(a => a.UpdatedBy)
                  .HasConstraintName("FK_Employee_Update")
                  .OnDelete(DeleteBehavior.NoAction);

                opt.HasOne(a => a.Deleter)
                  .WithMany(A => A.EmployeeDeletedBy)
                  .HasForeignKey(a => a.DeletedBy)
                  .HasConstraintName("FK_Employee_Delete")
                  .OnDelete(DeleteBehavior.NoAction);


                opt.HasIndex(a => new { a.IsActive, a.DeletedAt });

            });
        }

    }
}
