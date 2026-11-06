using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Domain.Entities;

namespace Tabeeb.Infrastructure.DbConfiguration
{
    public static  class PrescriptionConfig
    {
        public static void RunConfig(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Prescription>(opt =>
            {
                opt.ToTable("Prescription");
                opt.Property(a => a.Diagnosis).HasMaxLength(500);
                opt.Property(a => a.Notes).HasMaxLength(500);
                opt.HasOne(a => a.Reservation)
                   .WithOne(a => a.Prescription)
                   .HasForeignKey<Prescription>(a => a.ReservationId)
                   .HasConstraintName("FK_Prescription_Reservation")
                   .OnDelete(DeleteBehavior.NoAction);




                opt.HasOne(a => a.Creator)
                  .WithMany(A => A.PrescriptionCreatedBy)
                  .HasForeignKey(a => a.CreatedBy)
                  .HasConstraintName("FK_Prescription_Create")
                  .OnDelete(DeleteBehavior.NoAction);

                opt.HasOne(a => a.Updater)
                  .WithMany(A => A.PrescriptionUpdatedBy)
                  .HasForeignKey(a => a.UpdatedBy)
                  .HasConstraintName("FK_Prescription_Update")
                  .OnDelete(DeleteBehavior.NoAction);

                opt.HasOne(a => a.Deleter)
                  .WithMany(A => A.PrescriptionDeletedBy)
                  .HasForeignKey(a => a.DeletedBy)
                  .HasConstraintName("FK_Prescription_Delete")
                  .OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}
