using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Areas.RolePermissions.Commands.UpdateRolePermissions;
using Tabeeb.Application.Common.Services;
using Tabeeb.Domain.Entities;

namespace Tabeeb.Application.Areas.RolePermissions.Queries.GetRolePermissions
{
    public  class GetRolePermissions:IRequest<List<byte>>
    {
        public string RoleId {  get; set; }
    }


    public class GetRolePermissionsHandler
    (IAppDbContext appDbContext,
    ICurrentUserService currentUserService,
    RoleManager<AspNetRole> roleManager)
    : IRequestHandler<GetRolePermissions,List<byte>>
    {
        public async Task<List<byte>> Handle(GetRolePermissions request, CancellationToken cancellationToken)
        {
            var role = await roleManager.FindByNameAsync(request.RoleId);

            var currentPermissions = await appDbContext
                 .RolePermissions
                 .Where(a => a.ClinicId == currentUserService.ClinicId && a.RoleId == role.Id)
                 .Select(a=>a.PermissionId)
                 .ToListAsync(); ;



            return currentPermissions;
        }
    }
}
