using System.Linq.Expressions;
using Tabeeb.Application.Common.Pagination;

namespace Tabeeb.Application.Common.Extensions;

public static class IQuerableExtensions
{
    public static async Task<PaginatedList<T>> PaginateAsync<T>(this IQueryable<T> Querable,int page,int pagesize)
    {
        return await PaginatedList<T>.CreateAsync(Querable,page,pagesize);
    }


    public static IQueryable<T> OrderData<T>(
  this IQueryable<T> query,
  string propertyName,
  string direction)
    {
        if (string.IsNullOrWhiteSpace(propertyName))
            return query;

        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.PropertyOrField(parameter, propertyName);
        var lambda = Expression.Lambda(property, parameter);

        var methodName = direction.ToLower()=="desc" ? "OrderByDescending" : "OrderBy";

        var expression = Expression.Call(
            typeof(Queryable),
            methodName,
            new[] { typeof(T), property.Type },
            query.Expression,
            Expression.Quote(lambda));

        return query.Provider.CreateQuery<T>(expression);
    }
}
