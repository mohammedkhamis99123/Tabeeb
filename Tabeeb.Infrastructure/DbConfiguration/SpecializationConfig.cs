using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Domain.Entities;

namespace Tabeeb.Infrastructure.DbConfiguration
{
    public static class SpecializationConfig
    {

        public static void RunConfig(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Specialization>(opt =>
            {
                opt.ToTable("Specialization");
                opt.Property(A => A.Id).ValueGeneratedOnAdd();

                opt.Property(a => a.ArName).HasMaxLength(150);
                opt.Property(a => a.EnName).HasMaxLength(150);

                opt.HasOne(a => a.Creator)
                      .WithMany(A => A.SpecializationCreatedBy)
                      .HasForeignKey(a => a.CreatedBy)
                      .HasConstraintName("FK_Specialization_Create")
                      .OnDelete(DeleteBehavior.NoAction);

                opt.HasOne(a => a.Updater)
                  .WithMany(A => A.SpecializationUpdatedBy)
                  .HasForeignKey(a => a.UpdatedBy)
                  .HasConstraintName("FK_Specialization_Update")
                  .OnDelete(DeleteBehavior.NoAction);

                opt.HasOne(a => a.Deleter)
                  .WithMany(A => A.SpecializationDeletedBy)
                  .HasForeignKey(a => a.DeletedBy)
                  .HasConstraintName("FK_Specialization_Delete")
                  .OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}
