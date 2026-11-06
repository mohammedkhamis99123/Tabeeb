using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Areas.RolePermissions.Commands.UpdateRolePermissions;
using Tabeeb.Application.Common.Services;

namespace Tabeeb.Application.Areas.RolePermissions.Queries.GetRolePermissions
{
    public  class GetRolePermissions:IRequest<List<byte>>
    {
        public string RoleId {  get; set; }
    }


    public class GetRolePermissionsHandler
    (IAppDbContext appDbContext,
    ICurrentUserService currentUserService)
    : IRequestHandler<GetRolePermissions,List<byte>>
    {
        public async Task<List<byte>> Handle(GetRolePermissions request, CancellationToken cancellationToken)
        {
            var currentPermissions = await appDbContext
                 .RolePermissions
                 .Where(a => a.ClinicId == currentUserService.ClinicId && a.RoleId == request.RoleId)
                 .Select(a=>a.PermissionId)
                 .ToListAsync(); ;



            return currentPermissions;
        }
    }
}
