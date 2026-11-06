using Microsoft.AspNetCore.Mvc;
using Tabeeb.Application.Common.GenericReponse;
using Tabeeb.Application.Common.Pagination;

namespace Tabeeb.Extensions
{
    public static class GenericReponseExtensions
    {
        public static ActionResult ToGenericReponse<T>(this IEnumerable<T> obj)
            where T : class
        {
            return new OkObjectResult(new GenericReponse { IsSuccess = true, Data = obj });
        }

        public static async Task<ActionResult> ToGenericReponse<T>(this Task<IEnumerable<T>> obj)
            where T : class
        {
            var result = await obj;
            return new OkObjectResult(new GenericReponse { IsSuccess = true, Data = result });
        }

        public static async Task<ActionResult> ToGenericReponse<T>(this Task<T> obj)
            where T : class?
        {
            var result = await obj;
            return new OkObjectResult(new GenericReponse { IsSuccess = true, Data = result });
        }

        public static async Task<ActionResult> ToGenericReponse<T>(this Task<PaginatedList<T>> obj)
            where T : class
        {
            var result = await obj;
            return new OkObjectResult(new GenericReponse { IsSuccess = true, Data = result });
        }

        public static async Task<ActionResult> ToGenericReponse(this Task<int> value)
        {
            var result = await value;
            return new OkObjectResult(new GenericReponse { IsSuccess = true, Data = result });
        }

        public static async Task<ActionResult> ToGenericReponse(this Task<short> value)
        {
            var result = await value;
            return new OkObjectResult(new GenericReponse { IsSuccess = true, Data = result });
        }

        public static async Task<ActionResult> ToGenericReponse(this Task<decimal> value)
        {
            var result = await value;
            return new OkObjectResult(new GenericReponse { IsSuccess = true, Data = result });
        }

        public static async Task<ActionResult> ToGenericReponse(this Task value)
        {
            await value;

            return new OkObjectResult(new GenericReponse { IsSuccess = true, Data = "" });
        }

        public static async Task<ActionResult> ToGenericReponse(this Task<GenericReponse> obj)
        {
            var result = await obj;
            return new OkObjectResult(result);
        }
    }

}
