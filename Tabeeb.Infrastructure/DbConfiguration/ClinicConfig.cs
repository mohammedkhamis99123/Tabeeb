using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Domain.Entities;

namespace Tabeeb.Infrastructure.DbConfiguration
{
    public static  class ClinicConfig
    {
        public static void RunConfig(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Clinic>(opt =>
            {
                opt.ToTable("Clinic");
                opt.Property(a => a.ArName).HasMaxLength(200);
                opt.Property(a => a.EnName).HasMaxLength(200).IsUnicode(false);
                opt.Property(a => a.ArDescription).HasMaxLength(200);
                opt.Property(a => a.EnDescription).HasMaxLength(200).IsUnicode(false);
                opt.Property(a => a.ProfileAr).HasMaxLength(100);
                opt.Property(a => a.ProfileEn).HasMaxLength(100).IsUnicode(false);
                opt.Property(a => a.StreetEnName).HasMaxLength(150).IsUnicode(false);
                opt.Property(a => a.StreetArName).HasMaxLength(150);
                opt.Property(a => a.Langitude).HasMaxLength(150);
                opt.Property(a => a.Latitude).HasMaxLength(150);

               

                opt.HasOne(a => a.City)
                  .WithMany(a => a.Clinics)
                  .HasForeignKey(A => A.CityId)
                  .HasConstraintName("FK_Clinic_City")
                  .OnDelete(DeleteBehavior.NoAction);

       
                opt.HasOne(a => a.Creator)
             .WithMany(A => A.ClinicCreatedBy)
             .HasForeignKey(a => a.CreatedBy)
             .HasConstraintName("FK_Clinic_Create")
             .OnDelete(DeleteBehavior.NoAction);

                opt.HasOne(a => a.Updater)
                  .WithMany(A => A.ClinicUpdatedBy)
                  .HasForeignKey(a => a.UpdatedBy)
                  .HasConstraintName("FK_Clinic_Update")
                  .OnDelete(DeleteBehavior.NoAction);

                opt.HasOne(a => a.Deleter)
                  .WithMany(A => A.ClinicDeletedBy)
                  .HasForeignKey(a => a.DeletedBy)
                  .HasConstraintName("FK_Clinic_Delete")
                  .OnDelete(DeleteBehavior.NoAction);

                opt.HasIndex(a => a.IsActive);


            });
        }
    }
}
