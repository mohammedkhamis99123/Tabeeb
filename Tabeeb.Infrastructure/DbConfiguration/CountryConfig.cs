using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Domain.Entities;

namespace Tabeeb.Infrastructure.DbConfiguration
{
    public static   class CountryConfig
    {

        public static void RunConfig(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>(opt =>
            {
                opt.ToTable("Country");
                opt.Property(A => A.Id).ValueGeneratedOnAdd();
                opt.Property(a => a.ArName).HasMaxLength(200);
                opt.Property(a => a.EnName).HasMaxLength(200).IsUnicode(false);
                opt.Property(A => A.ISOCode).HasMaxLength(5).IsUnicode(false);
                opt.HasOne(a => a.Creator)
                 .WithMany(A => A.CountryCreatedBy)
                 .HasForeignKey(a => a.CreatedBy)
                 .HasConstraintName("FK_Country_Create")
                 .OnDelete(DeleteBehavior.NoAction);

                opt.HasOne(a => a.Updater)
                  .WithMany(A => A.CountryUpdatedBy)
                  .HasForeignKey(a => a.UpdatedBy)
                  .HasConstraintName("FK_Country_Update")
                  .OnDelete(DeleteBehavior.NoAction);

                opt.HasOne(a => a.Deleter)
                  .WithMany(A => A.CountryDeletedBy)
                  .HasForeignKey(a => a.DeletedBy)
                  .HasConstraintName("FK_Country_Delete")
                  .OnDelete(DeleteBehavior.NoAction);
            });
        }
      
    }
}
