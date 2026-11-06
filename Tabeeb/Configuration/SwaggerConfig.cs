using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using Tabeeb.Filters;

namespace Tabeeb.Configuration
{
    public static class SwaggerConfig
    {
        public static void ConfigSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(opt =>
            {

                // معلومات عامة عن الـ API الخاص بـ Shefaa
                opt.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Tabeeb Clinic Management API",
                    Description = "API endpoints for managing multi-tenant clinics, appointments, and billing."
                });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter Bearer Token",
                    Scheme = "Bearer"

                });

                opt.AddSecurityRequirement(
               new OpenApiSecurityRequirement
               {
                         {
                             new OpenApiSecurityScheme
                             {
                                 Reference = new OpenApiReference { Id = "Bearer", Type = ReferenceType.SecurityScheme },
                             },
                             new List<string>()
                         },
               }
           );
                opt.OperationFilter<SwaggerHeaderLanguageFilter>();
                //opt.OperationFilter<SwaggerFormListFilter>();

            });

        }
    }
    }


            


    

