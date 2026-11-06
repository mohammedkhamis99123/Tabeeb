using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Domain.Entities;

namespace Tabeeb.Infrastructure.DbConfiguration
{
    public  class PatientVitalConfig
    {
       public static void RunConfig(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PatientVital>(opt =>
            {
                opt.ToTable("PatientVital");
                opt.Property(a => a.BloodSugar).HasColumnType("decimal(5,5)");
                opt.Property(a => a.BMI).HasColumnType("decimal(5,5)");
                opt.Property(a => a.Weight).HasColumnType("tinyint");
                opt.Property(a => a.Height).HasColumnType("tinyint");
                opt.Property(a => a.Temperature).HasColumnType("decimal(2,2)");
                opt.Property(a => a.SystolicBP).HasColumnType("tinyint");
                opt.Property(a => a.DiastolicBP).HasColumnType("tinyint");
                opt.Property(a => a.OxygenSaturation).HasColumnType("int");
                opt.Property(a => a.HeartRate).HasColumnType("int");

                opt.HasOne(a => a.Reservation)
                   .WithOne(a => a.PatientVital)
                   .HasForeignKey<PatientVital>(a => a.ReservationId)
                   .HasConstraintName("FK_PatientVital_Reservation")
                   .OnDelete(DeleteBehavior.NoAction); ;


                opt.HasOne(a => a.Creator)
              .WithMany(A => A.PatientVitalCreatedBy)
              .HasForeignKey(a => a.CreatedBy)
              .HasConstraintName("FK_PatientVital_Create")
              .OnDelete(DeleteBehavior.NoAction);

                opt.HasOne(a => a.Updater)
                  .WithMany(A => A.PatientVitalUpdatedBy)
                  .HasForeignKey(a => a.UpdatedBy)
                  .HasConstraintName("FK_PatientVital_Update")
                  .OnDelete(DeleteBehavior.NoAction);

                opt.HasOne(a => a.Deleter)
                  .WithMany(A => A.PatientVitalDeletedBy)
                  .HasForeignKey(a => a.DeletedBy)
                  .HasConstraintName("FK_PatientVital_Delete")
                  .OnDelete(DeleteBehavior.NoAction);

            });
        }
    }
}
