using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Domain.Entities;

namespace Tabeeb.Infrastructure.DbConfiguration
{
    public static  class OfferConfig
    {
        public static void RunConfig(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Offer>(opt =>
            {
                opt.Property(a => a.Percentage).HasColumnType("decimal(3,2)");
                opt.HasOne(a => a.Doctor)
                   .WithMany(a => a.Offers)
                    .HasForeignKey(a => a.DoctorId)
                    .HasConstraintName("FK_Offer_Doctor")
                    .OnDelete(DeleteBehavior.NoAction);

                opt.HasOne(a => a.Clinic)
                  .WithMany(a => a.Offers)
                   .HasForeignKey(a => a.ClinicId)
                   .HasConstraintName("FK_Offer_Clinic")
                   .OnDelete(DeleteBehavior.NoAction);

                opt.HasOne(a => a.Creator)
                .WithMany(A => A.OfferCreatedBy)
                .HasForeignKey(a => a.CreatedBy)
                .HasConstraintName("FK_Offer_Create")
                .OnDelete(DeleteBehavior.NoAction);

                opt.HasOne(a => a.Updater)
                  .WithMany(A => A.OfferUpdatedBy)
                  .HasForeignKey(a => a.UpdatedBy)
                  .HasConstraintName("FK_Offer_Update")
                  .OnDelete(DeleteBehavior.NoAction);

                opt.HasOne(a => a.Deleter)
                  .WithMany(A => A.OfferDeletedBy)
                  .HasForeignKey(a => a.DeletedBy)
                  .HasConstraintName("FK_Offer_Delete")
                  .OnDelete(DeleteBehavior.NoAction);


                opt.HasIndex(a => new { a.IsActive, a.DeletedAt });
            });
        }
    }
}
