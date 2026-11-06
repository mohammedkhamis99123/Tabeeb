using Tabeeb.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tabeeb.Domain.Entities
{
    public class Permission:BaseEntity<byte>
    {
        public string ArName { get; set; } = null!;

       public string EnName {  get; set; } = null!;

        public virtual ICollection<RolePermissions> RolePermissions { get; set; }

       
    }
}
