using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Domain.Entities;

namespace Tabeeb.Infrastructure.DbConfiguration
{
    public static   class PermissionConfig

    {

        public static void RunConfig(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Permission>(opt =>
            {
                //opt.Property(A => A.Id).ValueGeneratedOnAdd();
                opt.Property(A => A.ArName).HasMaxLength(150);
                opt.Property(a => a.EnName).HasMaxLength(150).IsUnicode(false);
            });

        }
    }
}
