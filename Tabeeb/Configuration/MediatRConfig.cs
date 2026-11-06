using FluentValidation;
using MediatR;
using Tabeeb.Application.Common.Behaviour;

namespace Tabeeb.Configuration
{
    public static class MediatRConfig
    {

        public static void ConfigMediatR(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(Tabeeb.Application.Class1).Assembly);

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(Tabeeb.Application.Class1).Assembly);
                // الاستخدام الصحيح للـ Generic Pipeline Behavior
                cfg.AddOpenBehavior(typeof(ValidationBehaviour<,>));
            });
        }
    }
}
