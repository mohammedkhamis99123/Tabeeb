using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Domain.Entities;

namespace Tabeeb.Infrastructure.DbConfiguration
{
    public  static class ReservationConfig
    {

        public static void RunConfig(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reservation>(opt =>
            {
                opt.ToTable("Reservation");
                opt.Property(a => a.Status).HasColumnType("tinyint");
                opt.Property(a => a.Type).HasColumnType("tinyint");
                opt.Property(A => A.TotalAmount).HasColumnType("decimal(5,2)");

                opt.HasOne(A => A.Clinics)
                          .WithMany(A => A.Reservations)
                          .HasForeignKey(a => a.ClinicId)
                          .HasConstraintName("FK_Reservation_Clinic")
                          .OnDelete(DeleteBehavior.NoAction);


                opt.HasOne(A => A.Customer)
                        .WithMany(A => A.Reservations)
                        .HasForeignKey(a => a.CustomerId)
                        .HasConstraintName("FK_Reservation_Customer")
                        .OnDelete(DeleteBehavior.NoAction);


                opt.HasOne(A => A.Doctor)
                        .WithMany(A => A.Reservations)
                        .HasForeignKey(a => a.DoctorID)
                        .HasConstraintName("FK_Reservation_Doctor")
                        .OnDelete(DeleteBehavior.NoAction);


                opt.HasOne(A => A.Offer)
                      .WithMany(A => A.Reservations)
                      .HasForeignKey(a => a.OfferId)
                      .HasConstraintName("FK_Reservation_Offer")
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.NoAction);



                opt.HasOne(a => a.Creator)
                  .WithMany(A => A.ReservationCreatedBy)
                  .HasForeignKey(a => a.CreatedBy)
                  .HasConstraintName("FK_Reservation_Create")
                  .OnDelete(DeleteBehavior.NoAction);

                opt.HasOne(a => a.Updater)
                  .WithMany(A => A.ReservationUpdatedBy)
                  .HasForeignKey(a => a.UpdatedBy)
                  .HasConstraintName("FK_Reservation_Update")
                  .OnDelete(DeleteBehavior.NoAction);

                opt.HasOne(a => a.Deleter)
                  .WithMany(A => A.ReservationDeletedBy)
                  .HasForeignKey(a => a.DeletedBy)
                  .HasConstraintName("FK_Reservation_Delete")
                  .OnDelete(DeleteBehavior.NoAction);

            });

        }
    }
}
