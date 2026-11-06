using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Domain.Entities;

namespace Tabeeb.Infrastructure.DbConfiguration
{
    public static class RolePermissionsConfig
    {
        public static void RunConfig(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RolePermissions>(opt =>
            {
                opt.ToTable(nameof(RolePermissions));

                opt.HasKey(a => new { a.PermissionId, a.RoleId ,a.ClinicId});
                opt.HasOne(A => A.Permission)
                  .WithMany(a => a.RolePermissions)
                  .HasForeignKey(a => a.PermissionId)
                  .HasConstraintName("FK_RolePermission_Permission")
                  .OnDelete(DeleteBehavior.Cascade);

                opt.HasOne(A => A.Clinic)
                  .WithMany(a => a.RolePermissions)
                  .HasForeignKey(a => a.ClinicId)
                  .HasConstraintName("FK_RolePermission_Clinic")
                  .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
