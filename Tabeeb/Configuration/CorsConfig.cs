namespace Tabeeb.Configuration
{
    public static class CorsConfig
    {

        public static void ConfigCors(this IServiceCollection services)
        {services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod() // يسمح بـ PUT, POST, OPTIONS, إلخ
                          .AllowAnyHeader();
                });
            });
        }

    }
}
