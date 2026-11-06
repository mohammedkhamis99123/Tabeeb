using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Areas.Auth.Commands.LoginCommand.LoginCommand;
using Tabeeb.Application.Common.Services;
using Tabeeb.Domain.Entities;

namespace Tabeeb.Application.Areas.Auth.Commands.SelectClinicCommand
{
    public  class SelectClinicCommand:IRequest<LoginResultDto>
    {
        public int ClinicId {  get; set; }
    }

    public class SelectClinicCommandHandler
          (IAppDbContext appDbContext,
           ICurrentUserService currentUserService,
           IJWTService jWTService,
           RoleManager<AspNetRole> roleManager,
           UserManager<AppUser> userManager)
        : IRequestHandler<SelectClinicCommand,LoginResultDto>
    {
        public async Task<LoginResultDto> Handle(SelectClinicCommand request, CancellationToken cancellationToken)
        {
            var user = await appDbContext.Users
              .IgnoreQueryFilters()
              .Include(a => a.Doctor)
                  .ThenInclude(d => d!.DoctorClinic)
                     .ThenInclude(a => a.Clinic)
              .Include(a => a.Employee)
              .Where(a => (a.Doctor == null || a.Doctor.DeletedAt == null) &&
                          (a.Employee == null || a.Employee.DeletedAt == null))
              .AsNoTracking()
              .FirstOrDefaultAsync(a => a.Id == currentUserService.UserId, cancellationToken);
            var token = await jWTService.GenerateTokenAsync(user, request.ClinicId);

            var roles = await userManager.GetRolesAsync(user);


            // جلب الـ RoleIds المطابقة لأسماء الأدوار
            var roleIds = await roleManager.Roles
                .Where(r => roles.Contains(r.Name!))
                .Select(r => r.Id)
                .ToListAsync(cancellationToken);

            // جلب جميع الصلاحيات المطابقة للـ Roles المحددة
            var permissions = await appDbContext.RolePermissions
                .Where(a => roleIds.Contains(a.RoleId))
                .Select(a => (int)a.PermissionId)
                .Distinct()
                .ToArrayAsync(cancellationToken);

            return new LoginResultDto
            {
                Token = token.AccessToken,
                RefreshTokenEndAt = token.RefreshTokenExpiresAt,
                RefreshtokenToken = token.RefreshToken,
                TokenEndAt = token.AccessTokenExpiresAt,
                RedirectToChooseClinic = false,
                RedirectToChooseRole = false,
                DoctorID = user.Doctor?.Id,
                EmployeeID = user.Employee?.Id,
                ClinicID = request.ClinicId,
                Permissions = permissions,
                Roles = roles.ToArray(),


            };




        }
    }
}
