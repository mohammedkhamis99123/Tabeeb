using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Domain.Entities;

namespace Tabeeb.Infrastructure.DbConfiguration
{
    public static  class CityConfig
    {
        public static void Runconfig(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>(opt =>
            {
                opt.ToTable("City");
                opt.Property(A => A.ArName).HasMaxLength(200);
                opt.Property(A => A.EnName).HasMaxLength(200).IsUnicode(false);
                opt.HasOne(a => a.Country)
                .WithMany(a => a.Cities)
                .HasForeignKey(A => A.CountryId)
                .HasConstraintName("FK_City_Country")
                .OnDelete(DeleteBehavior.NoAction);

                opt.HasOne(a => a.Creator)
                  .WithMany(A => A.CityCreatedBy)
                  .HasForeignKey(a => a.CreatedBy)
                  .HasConstraintName("FK_City_Create")
                  .OnDelete(DeleteBehavior.NoAction);

                opt.HasOne(a => a.Updater)
                  .WithMany(A => A.CityUpdatedBy)
                  .HasForeignKey(a => a.UpdatedBy)
                  .HasConstraintName("FK_City_Update")
                  .OnDelete(DeleteBehavior.NoAction);

                opt.HasOne(a => a.Deleter)
                  .WithMany(A => A.CityDeletedBy)
                  .HasForeignKey(a => a.DeletedBy)
                  .HasConstraintName("FK_City_Delete")
                  .OnDelete(DeleteBehavior.NoAction);


                opt.HasIndex(a => new { a.IsActive, a.DeletedAt });


            });
        }
    }
}
