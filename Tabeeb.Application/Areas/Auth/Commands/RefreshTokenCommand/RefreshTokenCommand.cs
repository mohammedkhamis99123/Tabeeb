using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Tabeeb.Application.Areas.Auth.Commands.LoginCommand.LoginCommand;
using Tabeeb.Application.Common.Exceptions;
using Tabeeb.Application.Common.Services;
using Tabeeb.Domain.Entities;
using Tabeeb.Resources;

namespace Tabeeb.Application.Areas.Auth.Commands.RefreshTokenCommand
{
    public class RefreshTokenCommand : IRequest<LoginResultDto>
    {
        public string RefreshToken { get; set; } = string.Empty;
    }

    public class RefreshTokenCommandCommandHandler(
       IAppDbContext appDbContext,
       IJWTService jWTService,
       UserManager<AppUser> userManager,
       SignInManager<AppUser> signInManager,
       IStringLocalizer<SharedResource> localizer,
       RoleManager<AspNetRole> roleManager
   ) : IRequestHandler<RefreshTokenCommand, LoginResultDto>
    {
        public async Task<LoginResultDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            // البحث عن المستخدم باستخدام الـ Refresh Token القادم من الطلب وبشرط عدم انتهاء صلاحيته
            var user = await appDbContext.Users
                .IgnoreQueryFilters()
                .Include(a => a.Doctor)
                    .ThenInclude(d => d!.DoctorClinic)
                .Include(a => a.Employee)
                .Where(a => (a.Doctor == null || a.Doctor.DeletedAt == null) &&
                            (a.Employee == null || a.Employee.DeletedAt == null))
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.RefreshToken == request.RefreshToken &&
                                          a.RefreshTokenExpiredAt.HasValue &&
                                          a.RefreshTokenExpiredAt > DateTime.UtcNow, cancellationToken);

            if (user != null)
            {
                var token = await jWTService.GenerateTokenAsync(user, user.ClinicId);
                var roles = await userManager.GetRolesAsync(user);

                var roleIds = await roleManager.Roles
                    .Where(r => roles.Contains(r.Name!))
                    .Select(r => r.Id)
                    .ToListAsync(cancellationToken);

                var permissions = await appDbContext.RolePermissions
                    .Where(a => roleIds.Contains(a.RoleId))
                    .Select(a => (int)a.PermissionId)
                    .Distinct()
                    .ToArrayAsync(cancellationToken);

                return new LoginResultDto
                {
                    Token = token.AccessToken,
                    RefreshTokenEndAt = token.RefreshTokenExpiresAt, // تأكد من الاسم لديك
                    RefreshtokenToken = token.RefreshToken,
                    TokenEndAt = token.AccessTokenExpiresAt,
                    RedirectToChooseClinic = false,
                    RedirectToChooseRole = false,
                    DoctorID = user.Doctor?.Id,
                    EmployeeID = user.Employee?.Id,
                    ClinicID = user.ClinicId,
                    Permissions = permissions,
                    Roles = roles.ToArray()
                };
            }
            else
            {
                throw new UnAuthorizedException();
            }
        }
    }
}