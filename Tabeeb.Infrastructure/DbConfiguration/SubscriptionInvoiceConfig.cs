using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Domain.Entities;

namespace Tabeeb.Infrastructure.DbConfiguration
{
    public static class SubscriptionInvoiceConfig
    {
        public static void RunConfig(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SubscriptionInvoices>(opt =>
            {
                opt.ToTable("SubscriptionInvoices");
                opt.Property(a => a.TotalAmount).HasColumnType("decimal(18,2)");
                opt.Property(a => a.PaidAmount).HasColumnType("decimal(18,2)");

                opt.HasOne(a => a.Package)
                        .WithMany(A => A.Invoices)
                        .HasConstraintName("FK_Invoice_Package")
                        .HasForeignKey(a => a.PackageId)
                        .OnDelete(DeleteBehavior.NoAction);


                opt.HasOne(a => a.Clinic)
                    .WithMany(A => A.Invoices)
                    .HasConstraintName("FK_Invoice_Clinic")
                    .HasForeignKey(a => a.ClinicId)
                    .OnDelete(DeleteBehavior.NoAction);


                opt.HasOne(a => a.Creator)
               .WithMany(A => A.SubscriptionInvoiceCreatedBy)
               .HasForeignKey(a => a.CreatedBy)
               .HasConstraintName("FK_SubscriptionInvoice_Create")
               .OnDelete(DeleteBehavior.NoAction);

                opt.HasOne(a => a.Updater)
                  .WithMany(A => A.SubscriptionInvoiceUpdatedBy)
                  .HasForeignKey(a => a.UpdatedBy)
                  .HasConstraintName("FK_SubscriptionInvoice_Update")
                  .OnDelete(DeleteBehavior.NoAction);

                opt.HasOne(a => a.Deleter)
                  .WithMany(A => A.SubscriptionInvoiceDeletedBy)
                  .HasForeignKey(a => a.DeletedBy)
                  .HasConstraintName("FK_SubscriptionInvoice_Delete")
                  .OnDelete(DeleteBehavior.NoAction);


            });
        }
    }
}
