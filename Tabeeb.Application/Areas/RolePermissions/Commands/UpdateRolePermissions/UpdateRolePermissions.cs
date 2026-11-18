using MediatR;
using Microsoft.AspNetCore.Identity;
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

        public List<Domain.Enums.Permission> Permissions { get; set;} = new List<Domain.Enums.Permission>();
    }

    public class UpdateRolePermissionsHandler
        (IAppDbContext appDbContext,
        ICurrentUserService currentUserService,
        RoleManager<Domain.Entities.AspNetRole> roleManager)
        : IRequestHandler<UpdateRolePermissions>
    {
        public async Task Handle(UpdateRolePermissions request, CancellationToken cancellationToken)
        {
            var role = await roleManager.FindByNameAsync(request.RoleId);
            var currentPermissions = await appDbContext
                 .RolePermissions
                 .Where(a => a.ClinicId == currentUserService.ClinicId && a.RoleId == role.Id)
                 .ToListAsync(); ;


            appDbContext.RolePermissions.RemoveRange(currentPermissions);

            List<Domain.Entities.RolePermissions> roles = new List<Domain.Entities.RolePermissions>();
            foreach (var permission in request.Permissions)
            {

                var permissions = new Domain.Entities.RolePermissions
                {
                    ClinicId = currentUserService.ClinicId.Value,
                    PermissionId = (byte)permission,
                    RoleId = role.Id
                };

                roles.Add(permissions);
            }
                
            appDbContext.RolePermissions.AddRange(roles);

            await appDbContext.SaveChangesAsync();
        }
    }

}
