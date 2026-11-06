using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Text.Json;
using Tabeeb.Application;

namespace Tabeeb.Configuration
{
    public static class JWTConfig
    {
        public static void ConfigJWT(this IServiceCollection services, IConfiguration configuration)
        {
            // 1. ربط الـ Options لـ Dependency Injection
            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

            // 2. قراءة القيم بأسلوب .Bind الصريح بدلاً من .Get() لمنع الـ Null
            var jwt = new JwtSettings();
            configuration.GetSection(JwtSettings.SectionName).Bind(jwt);

            // 3. التحقق من وجود المفتاح لمنع تشغيل التطبيق بقيم فارغة
            if (string.IsNullOrEmpty(jwt.SecretKey))
            {
                throw new InvalidOperationException(
                    $"JwtSettings path '{JwtSettings.SectionName}' is missing, unreadable, or SecretKey is empty in appsettings.json.");
            }

            // 4. إعداد الـ Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
            options.RequireHttpsMetadata = false; // اضبطها true في الـ Production
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwt.Issuer,

                ValidateAudience = true,
                ValidAudience = jwt.Audience,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwt.SecretKey)),

                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            options.Events = new JwtBearerEvents
            {
                OnChallenge = async context =>
                {
                    // إلغاء المعالجة الافتراضية للـ Middleware
                    context.HandleResponse();

                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";

                    // شكل الرسالة المخصصة (عدّلها حسب الـ DTO الخاص بـ ApiResponse في مشروعك)

                        string message;
                        string errorCode;

                    if (context.AuthenticateFailure is SecurityTokenExpiredException)
                    {
                        // التوكن منتهي الصلاحية فعلياً
                        message = "انتهت صلاحية جلسة الدخول (Token Expired). يرجى التجديد عبر Refresh Token.";
                        errorCode = "TOKEN_EXPIRED";
                    }
                    else if (context.AuthenticateFailure != null)
                    {
                        // التوكن موجود لكن غير صالح (Invalid Signature, Wrong Issuer, etc.)
                        message = $"التوكن غير صالح: {context.AuthenticateFailure.Message}";
                        errorCode = "INVALID_TOKEN";
                    }
                    else
                    {
                        // التوكن غير موجود تماماً في الـ Header
                        message = "عفواً، غير مصرح لك بالوصول. يرجى إرفاق Token صالح في الـ Header.";
                        errorCode = "NO_TOKEN_PROVIDED";
                    }
                    var response = new
                    {
                        statusCode = 401,
                        message = message,
                        isSuccess = false,
                        errorCode=errorCode,
                        data = (object?)null
                    };

                    var jsonOptions = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    };

                    await context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
                }

            };
            
            });
            services.AddAuthorization();
        }
    }
}