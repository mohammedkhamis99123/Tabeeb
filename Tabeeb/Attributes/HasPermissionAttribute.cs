using Microsoft.AspNetCore.Mvc.Filters;
using Tabeeb.Application.Common.Exceptions;
using Tabeeb.Application.Common.Services;
using Tabeeb.Domain.Entities;
using Tabeeb.Domain.Enums;

namespace Tabeeb.Attributes
{
    public class HasPermissionAttribute(params Domain.Enums.Permission[] permissions) : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var service = context.HttpContext.RequestServices.GetRequiredService<ICurrentUserService>();

            if (!await service.hasAnyPermissionOf(permissions))
                throw new UnAuthorizedException();
            

        }

     
    }
}
