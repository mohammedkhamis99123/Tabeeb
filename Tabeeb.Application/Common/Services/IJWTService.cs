using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Domain.Entities;

namespace Tabeeb.Application.Common.Services
{
    public  interface IJWTService
    {
        Task<JwtResponse> GenerateTokenAsync(AppUser user, int? ClinicID);

        Task<JwtResponse> RefreshTokenAsync(AppUser user);

    }

    public class JwtResponse
    {
        public string AccessToken { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;

        public DateTime AccessTokenExpiresAt { get; set; }

        public DateTime RefreshTokenExpiresAt { get; set; }
    }
}
