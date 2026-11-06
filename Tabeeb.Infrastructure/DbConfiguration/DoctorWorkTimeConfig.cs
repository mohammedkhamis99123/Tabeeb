using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Domain.Entities;

namespace Tabeeb.Infrastructure.DbConfiguration
{
    public static class DoctorWorkTimeConfig
    {
        public static void RunConfig(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DoctorWorkTime>(opt =>
            {
                opt.Property(a => a.DayOfWeek).HasColumnType("tinyint");
                opt.HasOne(A => A.Doctor)
                  .WithMany(a => a.DoctorWorkTimes)
                    .HasForeignKey(A => A.DoctorID)
                    .HasConstraintName("FK_DoctorWorkTime_Doctor")
                    .OnDelete(DeleteBehavior.NoAction);

                opt.HasOne(A => A.clinic)
                  .WithMany(a => a.WorkTimes)
                    .HasForeignKey(A => A.ClinicId)
                    .HasConstraintName("FK_DoctorWorkTime_Clinic")
                    .OnDelete(DeleteBehavior.NoAction);

                opt.HasOne(a => a.Creator)
                  .WithMany(A => A.DoctorWorkTimeCreatedBy)
                  .HasForeignKey(a => a.CreatedBy)
                  .HasConstraintName("FK_DoctorWorkTime_Create")
                  .OnDelete(DeleteBehavior.NoAction);

                opt.HasOne(a => a.Updater)
                  .WithMany(A => A.DoctorWorkTimeUpdatedBy)
                  .HasForeignKey(a => a.UpdatedBy)
                  .HasConstraintName("FK_DoctorWorkTime_Update")
                  .OnDelete(DeleteBehavior.NoAction);

                opt.HasOne(a => a.Deleter)
                  .WithMany(A => A.DoctorWorkTimeDeletedBy)
                  .HasForeignKey(a => a.DeletedBy)
                  .HasConstraintName("FK_DoctorWorkTime_Delete")
                  .OnDelete(DeleteBehavior.NoAction);




            });
        }
    }
}
