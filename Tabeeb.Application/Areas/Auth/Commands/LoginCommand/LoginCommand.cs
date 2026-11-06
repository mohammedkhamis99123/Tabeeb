using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Tabeeb.Application.Areas.Auth.Commands.LoginCommand.LoginCommand;
using Tabeeb.Application.Areas.Clinic.Queries;
using Tabeeb.Application.Common.Exceptions;
using Tabeeb.Application.Common.Services;
using Tabeeb.Domain.Entities;
using Tabeeb.Resources;

namespace Tabeeb.Application.Areas.Auth.Commands.LoginCommand
{
    public class Login : IRequest<LoginResultDto>
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool RememberMe { get; set; }
    }

    public class LoginCommandHandler(
        IAppDbContext appDbContext,
        IJWTService jWTService,
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IStringLocalizer<SharedResource> localizer,
        RoleManager<AspNetRole> roleManager
    ) : IRequestHandler<Login, LoginResultDto>
    {
        public async Task<LoginResultDto> Handle(Login request, CancellationToken cancellationToken)
        {
            // 1. جلب المستخدم مع التضمينات اللازمة والتحقق من عدم الحذف Soft Delete
            var user = await appDbContext.Users
                .IgnoreQueryFilters()
                .Include(a => a.Doctor)
                    .ThenInclude(d => d!.DoctorClinic)
                       .ThenInclude(a=>a.Clinic)
                .Include(a => a.Employee)
                .Where(a => (a.Doctor == null || a.Doctor.DeletedAt == null) &&
                            (a.Employee == null || a.Employee.DeletedAt == null))
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Email == request.Email, cancellationToken);

            if (user is null)
                throw new ItemNotFoundException(localizer["UserNotFound"]);

            // 2. التحقق من كلمة المرور
            var result = await signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, false);

            if (!result.Succeeded)
                throw new Exception(localizer["InvalidUserNameOrPassword"]);

            // 3. فحص الأدوار (Roles)
            var roles = await userManager.GetRolesAsync(user);



            // سيناريو: دور واحد وهو طبيب
            if (roles.Contains("Doctor") && user.Doctor != null)
            {
                var doctorClinics = user.Doctor.DoctorClinic?.ToList() ?? new();

                if (doctorClinics.Count > 1)
                {
                  var res = await BuildLoginResultDto(user, roles, null, cancellationToken);
                  res.RedirectToChooseClinic = true;
                  var clinics = user.Doctor.DoctorClinic
                                   .Select(a => a.Clinic)
                                    .Select(A => new ClinicDto
                                    {
                                        ArName = A.ArName,
                                        EnName = A.EnName,
                                        Id = A.Id
                                    }).ToList();
                    res.Clinics = clinics;


                                
                
                  return res;

                }

                int? singleClinicId = doctorClinics.FirstOrDefault()?.ClinicId;
                return await BuildLoginResultDto(user, roles, singleClinicId, cancellationToken);
            }
            var clinicId = user.ClinicId;
            // سيناريو: دور واحد فقط وغير طبيب (مثل الموظف/الاستقبال)
            return await BuildLoginResultDto(user, roles, clinicId, cancellationToken);
        }

        private async Task<LoginResultDto> BuildLoginResultDto(AppUser user, IList<string> roleNames, int? clinicId, CancellationToken cancellationToken)
        {

            var token = await jWTService.GenerateTokenAsync(user, clinicId);

            // جلب الـ RoleIds المطابقة لأسماء الأدوار
         var roleIds = await roleManager.Roles
                .Where(r => roleNames.Contains(r.Name!))
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
                ClinicID = clinicId,
                Permissions = permissions,
                Roles = roleNames.ToArray(),

               
            };
        }
    }
}