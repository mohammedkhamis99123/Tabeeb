using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using System.Globalization;
using Tabeeb.Resources;

namespace Tabeeb.Configuration
{
    public static class LocalizationConfig
    {
        public static void AddLocalizationConfig(this IServiceCollection services)
        {
            services.AddControllers()
                 .AddDataAnnotationsLocalization(options =>
                 {
                     // 2. إجبار DataAnnotations (التحقق من المدخلات DTOs) على استخدام كلاس SharedResource المشتركة
                     options.DataAnnotationLocalizerProvider = (type, factory) =>
                         factory.Create(typeof(SharedResource));
                 }); ;

            services.AddLocalization(options => options.ResourcesPath = "");
        }

        public static void ConfigureLocalization(this WebApplication app)
        {
            var supportedCultures = new List<CultureInfo> { new CultureInfo("ar"), new CultureInfo("en-US") };

            // fix decimal binding from differnt cultures
            supportedCultures.ForEach(culture => culture.NumberFormat.NumberDecimalSeparator = ".");

            var requestLocaleOptions = new RequestLocalizationOptions
            {
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures,
                DefaultRequestCulture = new RequestCulture("en-US"),
                ApplyCurrentCultureToResponseHeaders = true,
            };

            requestLocaleOptions.RequestCultureProviders.Clear();

            // إضافة الـ AcceptLanguageHeaderProvider فقط كمصدر وحيد لقراءة اللغة
            requestLocaleOptions.RequestCultureProviders.Add(new AcceptLanguageHeaderRequestCultureProvider());

            app.UseRequestLocalization(requestLocaleOptions);
        }
    }
}
