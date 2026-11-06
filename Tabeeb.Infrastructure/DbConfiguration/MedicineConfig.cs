using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Domain.Entities;

namespace Tabeeb.Infrastructure.DbConfiguration
{
    public static  class MedicineConfig
    {
        public static void RunConfig(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Medicine>(opt =>
            {
                opt.ToTable("Medicine");
                opt.Property(a => a.Notes).HasMaxLength(500);
                opt.Property(A => A.Dose).HasMaxLength(200);
                opt.Property(A => A.Name).HasMaxLength(150);
                opt.HasOne(a => a.Prescription)
                   .WithMany(A => A.Medicines)
                     .HasForeignKey(a => a.PrescriptionId)
                     .HasConstraintName("FK_Medicine_Prescription")
                     .OnDelete(DeleteBehavior.NoAction);


                opt.HasOne(a => a.Creator)
                      .WithMany(A => A.MedicineCreatedBy)
                      .HasForeignKey(a => a.CreatedBy)
                      .HasConstraintName("FK_Medicine_Create")
                      .OnDelete(DeleteBehavior.NoAction);

                opt.HasOne(a => a.Updater)
                  .WithMany(A => A.MedicineUpdatedBy)
                  .HasForeignKey(a => a.UpdatedBy)
                  .HasConstraintName("FK_Medicine_Update")
                  .OnDelete(DeleteBehavior.NoAction);

                opt.HasOne(a => a.Deleter)
                  .WithMany(A => A.MedicineDeletedBy)
                  .HasForeignKey(a => a.DeletedBy)
                  .HasConstraintName("FK_Medicine_Delete")
                  .OnDelete(DeleteBehavior.NoAction);




            });
        }
    }
}
