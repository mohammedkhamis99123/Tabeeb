using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Domain.Entities;

namespace Tabeeb.Infrastructure.DbConfiguration
{
    public static  class DoctorConfig
    {
        public static void RunConfig(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Doctor>(opt =>
            {
                opt.ToTable("Doctor");
                opt.Property(a => a.ArName).HasMaxLength(150);
                opt.Property(A => A.EnName).HasMaxLength(150).IsUnicode(false);
            
               
                opt.HasOne(a => a.Specialization)
               .WithMany(a => a.Doctors)
                .HasForeignKey(a => a.SpecializationId)
                .HasConstraintName("FK_Doctor_Specialization")
                .OnDelete(DeleteBehavior.NoAction);

                opt.HasOne(a => a.AppUser)
                   .WithOne(A => A.Doctor)
                   .HasForeignKey<Doctor>(a => a.UserId)
                   .HasConstraintName("FK_Customer_Doctor")
                   .OnDelete(DeleteBehavior.NoAction);

                opt.HasOne(a => a.Creator)
                      .WithMany(A => A.DoctorCreatedBy)
                      .HasForeignKey(a => a.CreatedBy)
                      .HasConstraintName("FK_Doctor_Create")
                      .OnDelete(DeleteBehavior.NoAction);

                opt.HasOne(a => a.Updater)
                  .WithMany(A => A.DoctorUpdatedBy)
                  .HasForeignKey(a => a.UpdatedBy)
                  .HasConstraintName("FK_Doctor_Update")
                  .OnDelete(DeleteBehavior.NoAction);

                opt.HasOne(a => a.Deleter)
                  .WithMany(A => A.DoctorDeletedBy)
                  .HasForeignKey(a => a.DeletedBy)
                  .HasConstraintName("FK_Doctor_Delete")
                  .OnDelete(DeleteBehavior.NoAction);



                opt.HasIndex(a => new { a.IsActive, a.DeletedAt });



            });

        }
    }
}
