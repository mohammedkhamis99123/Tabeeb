using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Domain.Entities;

namespace Tabeeb.Infrastructure.DbConfiguration
{
    public static  class ClinicDoctorConfig
    {
        public static void RunConfig(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<DoctorClinic>(opt =>
            {
                opt.ToTable("DoctorClinic");
                opt.HasKey(a => new { a.ClinicId, a.DoctorId });
                opt.HasOne(a => a.Doctor)
                   .WithMany(a => a.DoctorClinic)
                   .HasForeignKey(a => a.DoctorId)
                   .HasConstraintName("FK_DoctorClinic_Doctor")
                   .OnDelete(DeleteBehavior.NoAction);


                opt.HasOne(a => a.Clinic)
                  .WithMany(a => a.Doctors)
                  .HasForeignKey(a => a.ClinicId)
                  .HasConstraintName("FK_DoctorClinic_Clinic")
                  .OnDelete(DeleteBehavior.NoAction);
            });

        }
    }
}
