using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Domain.Entities;

namespace Tabeeb.Infrastructure.DbConfiguration
{
    public static class ClinicImageConfig
    {
        public static void RunConfig(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClinicImage>(opt =>
            {
                opt.ToTable("ClinicImage");
                opt.Property(a => a.ImageType).HasColumnType("tinyint");

                opt.HasOne(a => a.Clinics)
                  .WithMany(a => a.Images)
                  .HasForeignKey(a => a.ClinicId)
                  .HasConstraintName("FK_ClinicImage_Clinic")
                  .OnDelete(DeleteBehavior.NoAction);


                opt.HasOne(a => a.Creator)
                  .WithMany(A => A.ClinicImageCreatedBy)
                  .HasForeignKey(a => a.CreatedBy)
                  .HasConstraintName("FK_ClinicImage_Create")
                  .OnDelete(DeleteBehavior.NoAction);

                opt.HasOne(a => a.Updater)
                  .WithMany(A => A.ClinicImageUpdatedBy)
                  .HasForeignKey(a => a.UpdatedBy)
                  .HasConstraintName("FK_ClinicImage_Update")
                  .OnDelete(DeleteBehavior.NoAction);

                opt.HasOne(a => a.Deleter)
                  .WithMany(A => A.ClinicImageDeletedBy)
                  .HasForeignKey(a => a.DeletedBy)
                  .HasConstraintName("FK_ClinicImage_Delete")
                  .OnDelete(DeleteBehavior.NoAction);

            });
        }
    }
}
