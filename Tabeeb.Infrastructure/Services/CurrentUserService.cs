using System;
using System.Collections.Generic;
using System.Text;

namespace Tabeeb.Infrastructure.Services
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System.Security.Claims;
    using Tabeeb.Application.Common.Services;
    using Tabeeb.Domain.Entities;
    using Tabeeb.Domain.Enums;
    using Permission = Domain.Enums.Permission;

    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IServiceScopeFactory scopeFactory;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor, IServiceScopeFactory scopeFactory)
        {
            _httpContextAccessor = httpContextAccessor;
            this.scopeFactory = scopeFactory;
        }

        private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

        public bool IsAuthenticated =>
            User?.Identity?.IsAuthenticated ?? false;

        public string? UserId =>
            User?.FindFirstValue(ClaimTypes.NameIdentifier);

        public string? UserName =>
            User?.FindFirstValue(ClaimTypes.Name);

        public int? ClinicId
        {
            get
            {
                var clinicId = User?.FindFirst("ClinicId")?.Value;

                if (int.TryParse(clinicId, out var id))
                    return id;

                return null;
            }
            set
            {
                ClinicId = value;
            }
        }

        public int? CustomerId =>
           int.TryParse(User?.FindFirst("CustomerId")?.Value, out var id)
               ? id
               : null;

        public int? EmployeeId =>
          int.TryParse(User?.FindFirst("EmployeeId")?.Value, out var id)
              ? id
              : null;
        public int? DoctorId =>
            int.TryParse(User?.FindFirst("DoctorId")?.Value, out var id)
                ? id
                : null;

        public IEnumerable<string> Roles =>
            User?.FindAll(ClaimTypes.Role)
                .Select(x => x.Value)
            ?? Enumerable.Empty<string>();



        public async Task<bool> hasAnyPermissionOf(params Permission[] permissions)
        {
            var userRoles = _httpContextAccessor.HttpContext?.User?.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            if (userRoles == null || !userRoles.Any())
                return false;


            var permissionBytes = permissions.Select(p => (byte)p).ToList();

           
            // 💡 بننشئ Scope مؤقت هنا بس عشان نجيب الـ DbContext للتحقق من الصلاحيات
            using var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<IAppDbContext>();

            return await dbContext.RolePermissions
               .Include(rp => rp.Role)
               .AnyAsync(rp => userRoles.Contains(rp.Role.Name)
                            && (permissionBytes.Contains(rp.PermissionId) || rp.PermissionId == 1));
        }
    }
}
