using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Domain.Entities;

namespace Tabeeb.Infrastructure.DbConfiguration
{
    public  class SubscriptionPackageConfig
    {
        public static void RunConfig(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SubscriptionPackage>(opt =>
            {
                opt.ToTable("SubscriptionPackage");
                opt.Property(A => A.Id).ValueGeneratedOnAdd();

                opt.Property(a => a.ArName).HasMaxLength(200);
                opt.Property(a => a.EnName).HasMaxLength(200).IsUnicode(false);


                opt.Property(a => a.ArDescription).HasMaxLength(500);
                opt.Property(a => a.EnDescription).HasMaxLength(500).IsUnicode(false);

                opt.HasOne(a => a.Creator)
               .WithMany(A => A.SubscriptionPackageCreatedBy)
               .HasForeignKey(a => a.CreatedBy)
               .HasConstraintName("FK_SubscriptionPackage_Create")
               .OnDelete(DeleteBehavior.NoAction);

                opt.HasOne(a => a.Updater)
                  .WithMany(A => A.SubscriptionPackageUpdatedBy)
                  .HasForeignKey(a => a.UpdatedBy)
                  .HasConstraintName("FK_SubscriptionPackage_Update")
                  .OnDelete(DeleteBehavior.NoAction);

                opt.HasOne(a => a.Deleter)
                  .WithMany(A => A.SubscriptionPackageDeletedBy)
                  .HasForeignKey(a => a.DeletedBy)
                  .HasConstraintName("FK_SubscriptionPackage_Delete")
                  .OnDelete(DeleteBehavior.NoAction);



            });
        }
    }
}
