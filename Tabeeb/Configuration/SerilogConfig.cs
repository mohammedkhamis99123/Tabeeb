using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Tabeeb.Configuration
{
    public static class SerilogConfig
    {
        /// <summary>
        /// إعداد Serilog المبدئي وقراءة الإعدادات من appsettings.json
        /// </summary>
        public static void AddSerilogConfiguration(this IHostBuilder host)
        {
            host.UseSerilog((context, services, configuration) => configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services))
                ;
        }

        /// <summary>
        /// تفعيل الـ Middleware الخاص بتسجيل الـ HTTP Requests
        /// </summary>
        public static void UseSerilogConfiguration(this WebApplication app)
        {
            // هذا السطر يقوم بتسجيل كل API Request تلقائياً (المسار، الوقت المستغرق، الـ Status Code)
            app.UseSerilogRequestLogging();
        }
    }
}