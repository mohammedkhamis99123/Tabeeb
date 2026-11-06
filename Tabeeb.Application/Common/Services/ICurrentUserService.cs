using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Domain.Entities;
using Tabeeb.Domain.Enums;
using Permission = Tabeeb.Domain.Enums.Permission;

namespace Tabeeb.Application.Common.Services
{
    public  interface ICurrentUserService
    {
     
            string? UserId { get; }

            int? ClinicId { get; set; }

            int? DoctorId { get; }

            string? UserName { get; }

            int? EmployeeId { get; }
            IEnumerable<string> Roles { get; }

            bool IsAuthenticated { get; }
            int? CustomerId { get;}
         Task<bool> hasAnyPermissionOf(params Permission[] Permission);
        
    }
}
