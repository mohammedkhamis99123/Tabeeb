using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tabeeb.Domain.Entities
{
    public  class RolePermissions
    {
        public string RoleId { get; set; } = null!;

        public byte PermissionId {  get; set; }

        public int ClinicId {  get; set; }


        public Clinic Clinic { get; set; }

        public Permission Permission { get; set; } = null!;

        public AspNetRole Role { get; set; } = null!;
    }
}
