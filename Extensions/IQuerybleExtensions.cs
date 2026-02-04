using System.Linq.Expressions;

namespace angular_vega.Extensions
{
    public static class IQuerybleExtensions
    {
        public static IQueryable<T> ApplyOrdering<T>(this IQueryable<T> query, IQueryObject queryObj, Dictionary<string, Expression<Func<T, object>>> columnsMap)
        {
            if (String.IsNullOrEmpty(queryObj.SortBy) || !columnsMap.ContainsKey(queryObj.SortBy))
                return query;

            if (queryObj.IsSortAscending)
            {
                return query = query.OrderBy(columnsMap[queryObj.SortBy]);
            }
            else
            {
                return query = query.OrderByDescending(columnsMap[queryObj.SortBy]);
            }
        }

        public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, IQueryObject queryObj)
        {

            if (queryObj.Page == 0)
                queryObj.Page = 10;

            if (queryObj.PageSize == 0)
                queryObj.PageSize = 10;


            return query.Skip((queryObj.Page - 1) * queryObj.PageSize).Take(queryObj.PageSize);
        }
    }
}