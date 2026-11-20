using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Domain.Entities;

namespace Tabeeb.Infrastructure.DbConfiguration
{
    public static class OfferConfig
    {
        public static void RunConfig(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Offer>(opt =>
            {
                opt.Property(a => a.Percentage).HasColumnType("decimal(3,2)");
                opt.HasOne(a => a.doctorClinic)
                   .WithOne(a => a.Offer)
                    .HasForeignKey<DoctorClinic>(a => a.OfferId)
                    .HasConstraintName("FK_Offer_Doctor")
                    .OnDelete(DeleteBehavior.NoAction);

                opt.HasMany(a => a.Reservations)
                   .WithOne(a => a.Offer)
                   .HasForeignKey(a => a.OfferId)
                   .HasConstraintName("FK_Reservation_Offer")
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
