using Microsoft.EntityFrameworkCore;
using Tabeeb.Application.Common.Services;
using Tabeeb.Infrastructure.DbContexts;
using Tabeeb.Infrastructure.Interceptors;
using Tabeeb.Infrastructure.Services;

namespace Tabeeb.Configuration
{
    public static  class DbConfig
    {
        public static void ConfigDb(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddScoped<Auditingnterceptor>();

            // 2. تمرير الـ Interceptor لـ DbContext
            services.AddDbContext<AppDbContext>((sp, options) =>
            {
                var auditingInterceptor = sp.GetRequiredService<Auditingnterceptor>();

                options.UseSqlServer(configuration.GetConnectionString("Default"))
                       .AddInterceptors(auditingInterceptor); // 👈 إضافة الـ Interceptor هنا
            });

        }
    }
}
