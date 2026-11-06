using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Domain.Entities;

namespace Tabeeb.Infrastructure.DbConfiguration
{
    public static   class CustomerConfig
    {
        public static void RunConfig(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(opt =>
            {
                opt.Property(a => a.ArName).HasMaxLength(150);
                opt.Property(A => A.EnName).HasMaxLength(150).IsUnicode(false);
                opt.Property(a => a.Address).HasMaxLength(250);
                opt.Property(a => a.PhoneNumber).HasMaxLength(15);
                opt.Property(A => A.Email).HasMaxLength(120).IsUnicode(false);

                opt.HasOne(a => a.AppUser)
                   .WithOne(A => A.Customer)
                   .HasForeignKey<Customer>(a => a.UserId)
                   .HasConstraintName("FK_Customer_User")
                   .OnDelete(DeleteBehavior.NoAction);

                opt.HasOne(a => a.Creator)
                   .WithMany(A => A.CustomerCreatedBy)
                   .HasForeignKey(a => a.CreatedBy)
                   .HasConstraintName("FK_Customer_Create")
                   .OnDelete(DeleteBehavior.NoAction);

                opt.HasOne(a => a.Updater)
                  .WithMany(A => A.CustomerUpdatedBy)
                  .HasForeignKey(a => a.UpdatedBy)
                  .HasConstraintName("FK_Customer_Update")
                  .OnDelete(DeleteBehavior.NoAction);

                opt.HasOne(a => a.Deleter)
                  .WithMany(A => A.CustomerDeletedBy)
                  .HasForeignKey(a => a.DeletedBy)
                  .HasConstraintName("FK_Customer_Delete")
                  .OnDelete(DeleteBehavior.NoAction);



            });
        }
    }
}
