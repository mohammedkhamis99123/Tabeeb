using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tabeeb.Domain.Entities
{
    public  class AspNetRole:IdentityRole
    {

        public virtual ICollection<RolePermissions> RolePermissions { get; set; }
    }
}
