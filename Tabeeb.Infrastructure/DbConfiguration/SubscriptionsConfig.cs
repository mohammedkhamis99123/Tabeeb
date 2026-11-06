using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Domain.Entities;

namespace Tabeeb.Infrastructure.DbConfiguration
{
    public  static  class SubscriptionsConfig
    {
        public static void RunConfig(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subscriptions>(opt =>
            {
                //opt.HasKey(a => new { a.ClinicId, a.PackageID });

                opt.ToTable("Subscription");
                opt.HasOne(a => a.Package)
                   .WithMany(a => a.Subscriptions)
                   .HasForeignKey(A => A.PackageID)
                   .HasConstraintName("FK_Package_Subscription")
                   .OnDelete(DeleteBehavior.NoAction);

                opt.HasOne(A => A.Invoice)
                    .WithOne(a => a.Subscriptions)
                    .HasForeignKey<Subscriptions>(a => a.InvoiceId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .HasConstraintName("FK_Subscription_Invoice");
                    


                opt.HasOne(a => a.Clinics)
                 .WithMany(a => a.Subscriptions)
                 .HasForeignKey(A => A.ClinicId)
                 .HasConstraintName("FK_Clinic_Subscription")
                 .OnDelete(DeleteBehavior.NoAction);

                opt.HasOne(a => a.Creator)
               .WithMany(A => A.SubscriptionsCreatedBy)
               .HasForeignKey(a => a.CreatedBy)
               .HasConstraintName("FK_Subscription_Create")
               .OnDelete(DeleteBehavior.NoAction);

                opt.HasOne(a => a.Updater)
                  .WithMany(A => A.SubscriptionsUpdatedBy)
                  .HasForeignKey(a => a.UpdatedBy)
                  .HasConstraintName("FK_Subscription_Update")
                  .OnDelete(DeleteBehavior.NoAction);

                opt.HasOne(a => a.Deleter)
                  .WithMany(A => A.SubscriptionsDeletedBy)
                  .HasForeignKey(a => a.DeletedBy)
                  .HasConstraintName("FK_Subscription_Delete")
                  .OnDelete(DeleteBehavior.NoAction);

            });
        }
    }
}
