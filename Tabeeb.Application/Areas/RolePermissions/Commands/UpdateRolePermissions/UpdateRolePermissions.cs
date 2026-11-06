using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Common.Services;
using Tabeeb.Domain.Enums;

namespace Tabeeb.Application.Areas.RolePermissions.Commands.UpdateRolePermissions
{
    public  class UpdateRolePermissions:IRequest
    {
        public string  RoleId { get; set; }

        public List<Permission> Permissions { get; set;} = new List<Permission>();
    }

    public class UpdateRolePermissionsHandler
        (IAppDbContext appDbContext,
        ICurrentUserService currentUserService)
        : IRequestHandler<UpdateRolePermissions>
    {
        public async Task Handle(UpdateRolePermissions request, CancellationToken cancellationToken)
        {
            var currentPermissions = await appDbContext
                 .RolePermissions
                 .Where(a => a.ClinicId == currentUserService.ClinicId && a.RoleId == request.RoleId)
                 .ToListAsync(); ;


            appDbContext.RolePermissions.RemoveRange(currentPermissions);

            List<Domain.Entities.RolePermissions> roles = new List<Domain.Entities.RolePermissions>();
            foreach (var permission in request.Permissions)
            {

                var permissions = new Domain.Entities.RolePermissions
                {
                    ClinicId = currentUserService.ClinicId.Value,
                    PermissionId = (byte)permission,
                    RoleId = request.RoleId
                };

                roles.Add(permissions);
            }
                
            appDbContext.RolePermissions.AddRange(roles);

            await appDbContext.SaveChangesAsync();
        }
    }

}
