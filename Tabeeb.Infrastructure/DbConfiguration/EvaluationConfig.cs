using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Domain.Entities;

namespace Tabeeb.Infrastructure.DbConfiguration
{
    public static  class EvaluationConfig
    {
        public static void RunConfig(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Evaluation>(opt =>
            {
                opt.ToTable("Evaluation");
                opt.HasOne(a => a.Reservation)
                  .WithOne(A => A.Evaluation)
                  .HasForeignKey<Evaluation>(a => a.ReservationId)
                  .HasConstraintName("FK_REservation_Evaluation")
                  .OnDelete(DeleteBehavior.Cascade);

                opt.HasOne(a => a.Creator)
                .WithMany(A => A.EvaluationCreatedBy)
                .HasForeignKey(a => a.CreatedBy)
                .HasConstraintName("FK_Evaluation_Create")
                .OnDelete(DeleteBehavior.NoAction);

                opt.HasOne(a => a.Updater)
                  .WithMany(A => A.EvaluationUpdatedBy)
                  .HasForeignKey(a => a.UpdatedBy)
                  .HasConstraintName("FK_Evaluation_Update")
                  .OnDelete(DeleteBehavior.NoAction);

                opt.HasOne(a => a.Deleter)
                  .WithMany(A => A.EvaluationDeletedBy)
                  .HasForeignKey(a => a.DeletedBy)
                  .HasConstraintName("FK_Evaluation_Delete")
                  .OnDelete(DeleteBehavior.NoAction);

            });
        }
    }
}
