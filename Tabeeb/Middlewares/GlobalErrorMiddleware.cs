using Microsoft.Extensions.Localization;
using System.Text.Json;
using Tabeeb.Application.Common.Exceptions;
using Tabeeb.Application.Common.GenericReponse;
using Tabeeb.Resources;

namespace Tabeeb.Middlewares
{

    public class GlobalErrorMiddleware(RequestDelegate next, IStringLocalizer<SharedResource> localizer)
    {
        private readonly Dictionary<Type, Func<HttpContext, Exception, Task>> _exceptionHandlers;

        public async Task InvokeAsync(HttpContext context)
        {

            // Enable buffering at the start to capture the request body
            context.Request.EnableBuffering();

            try
            {
                await next(context);
            }catch(Exception ex)
            {
                await buildResponse(ex,context);
            }


        }

        private async Task buildResponse(Exception ex, HttpContext context)
        {
            context.Response.ContentType = "application/json";
           if(ex is ValidationException validation)
            {
                context.Response.StatusCode = 400;
                var response = new GenericReponse
                {
                    Message = validation.Message,
                    Errors = validation.Errors,
                    IsSuccess = false
                }; ;
                var result = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(result);


            }else if(ex is ItemNotFoundException notFoundException)
            {
                context.Response.StatusCode = 404;
                var response = new GenericReponse
                {
                    Message = localizer["NotFound"],
                    //Errors = validation.Errors,
                    IsSuccess = false
                }; ;
                var result = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(result);

            }
            else if (ex is UnAuthorizedException  unAuthorizedException)
            {
                context.Response.StatusCode = 401;
                var response = new GenericReponse
                {
                    Message = localizer["UnAuthorized"],
                    //Errors = validation.Errors,
                    IsSuccess = false
                }; ;
                var result = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(result);

            }
            else if (ex is forbiddenAccessException forbiddenAccessException)
            {
                context.Response.StatusCode = 403;
                var response = new GenericReponse
                {
                    Message = localizer["Forbidden"],
                    //Errors = validation.Errors,
                    IsSuccess = false
                }; ;
                var result = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(result);

            }
            else
            {
                context.Response.StatusCode = 400;
                var response = new GenericReponse
                {
                    Message = ex.Message,
                    //Errors = validation.Errors,
                    IsSuccess = false
                }; ;
                var result = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(result);
            }
        }
    }
}
