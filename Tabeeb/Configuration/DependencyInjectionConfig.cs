using Tabeeb.Application.Common.Services;
using Tabeeb.Infrastructure.DbContexts;
using Tabeeb.Infrastructure.Services;

namespace Tabeeb.Configuration
{
    public static  class DependencyInjectionConfig
    {
        public static void ConfigDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<IAppDbContext, AppDbContext>();
            services.AddScoped<IJWTService, JWTService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IInvoiceGenerator, InvoiceGenerator>();
            services.AddScoped<IQrCodeGernerator, QrCodeGenerator>();
            services.AddHttpContextAccessor();
        }
    }
}
