using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Domain.Entities;

namespace Tabeeb.Domain.Common
{
    namespace Tabeeb.Domain.Common
    {
         public interface IBaseAuditableEntity
        {
             AppUser? Creator { get; set; }

             AppUser? Updater { get; set; }


             AppUser? Deleter { get; set; }

             DateTime? CreatedAt { get; set; }

             string? CreatedBy { get; set; }


             DateTime? UpdatedAt { get; set; }

             string? UpdatedBy { get; set; }

             DateTime? DeletedAt { get; set; }

             string? DeletedBy { get; set; }

        }
    }
}
