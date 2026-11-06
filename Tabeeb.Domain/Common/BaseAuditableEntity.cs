using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata;
using Tabeeb.Domain.Common;
using Tabeeb.Domain.Common.Tabeeb.Domain.Common;
using Tabeeb.Domain.Entities;

namespace Tabeeb.Domain.Common
{
    public class BaseAuditableEntity<T> : BaseEntity<T>, IBaseAuditableEntity
    {
        public AppUser? Creator  {get;set;} 
        public AppUser? Updater {get;set;} 
        public AppUser? Deleter {get;set;} 
        public DateTime? CreatedAt {get;set;} 
        public string? CreatedBy {get;set;} 
        public DateTime? UpdatedAt {get;set;} 
        public string? UpdatedBy {get;set;} 
        public DateTime? DeletedAt  {get;set;} 
        public string? DeletedBy  {get;set;} 
    }
}
