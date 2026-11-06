using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Tabeeb.Application;
using Tabeeb.Application.Common.Services;
using Tabeeb.Domain.Entities;
using Tabeeb.Resources;

namespace Tabeeb.Infrastructure.Services
{
    public class JWTService : IJWTService
    {
        private readonly JwtSettings _settings;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly UserManager<AppUser> _userManager;

        public JWTService(
            IOptions<JwtSettings> settings,
            IStringLocalizer<SharedResource> localizer,
            UserManager<AppUser> userManager)
        {
            _settings = settings.Value;
            _localizer = localizer;
            _userManager = userManager;
        }

        public async Task<JwtResponse> GenerateTokenAsync(AppUser user, int? clinicId = null)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Name, user.UserName!),
                new(ClaimTypes.Email, user.Email!),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            claims.Add(new Claim("UserID", user.Id.ToString()));

            // إضافة Claims الطبيب والعيادة
            if (user.Doctor != null)
            {
                claims.Add(new Claim("DoctorId", user.Doctor.Id.ToString()));

                var resolvedClinicId = clinicId
                    ?? user.Doctor.DoctorClinic?.FirstOrDefault()?.ClinicId;

                if (resolvedClinicId.HasValue)
                {
                    claims.Add(new Claim("ClinicId", resolvedClinicId.Value.ToString()));
                }
            }

            if (user.Customer != null)
            {
                claims.Add(new Claim("CustomerId", user.Customer.Id.ToString()));
            }

            if (user.Employee != null)
            {
                claims.Add(new Claim("EmployeeId", user.Employee.Id.ToString()));
                claims.Add(new Claim("ClinicId", user.Employee.ClinicId.ToString()));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var accessTokenExpiresAt = DateTime.UtcNow.AddMinutes(_settings.AccessTokenExpirationMinutes);
            var refreshTokenExpiresAt = DateTime.UtcNow.AddDays(_settings.RefreshTokenExpirationDays);

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: accessTokenExpiresAt,
                signingCredentials: credentials);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            var refreshToken = Guid.NewGuid().ToString("N");

            // ✅ الحل المباشر لمعالجة خطأ الـ Tracking:
            // تحديث حقول الـ Refresh Token فقط في قاعدة البيانات دون عمل Update لشجرة الـ Entities الكاملة
            await _userManager.Users
                .Where(u => u.Id == user.Id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(u => u.RefreshToken, refreshToken)
                    .SetProperty(u => u.RefreshTokenExpiredAt, refreshTokenExpiresAt));

            // تحديث الكائن في الـ Memory ليبقى متسقاً
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiredAt = refreshTokenExpiresAt;

            return new JwtResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiresAt = accessTokenExpiresAt,
                RefreshTokenExpiresAt = refreshTokenExpiresAt
            };
        }

        public async Task<JwtResponse> RefreshTokenAsync(AppUser user)
        {
            if (user.RefreshTokenExpiredAt <= DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException(_localizer["unAuthorized"]);
            }

            return await GenerateTokenAsync(user);
        }
    }
}