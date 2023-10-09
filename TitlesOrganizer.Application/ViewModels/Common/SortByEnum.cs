using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace TitlesOrganizer.Application.ViewModels.Common
{
    public enum SortByEnum
    {
        [Display(Name = "[A-Z]")]
        Ascending = 0,

        [Display(Name = "[Z-A]")]
        Descending = 1
    }

    public static class SortByExtensions
    {
        public static IOrderedQueryable<T> Sort<T, TKey>(this IQueryable<T> listToSort, SortByEnum sortBy, Expression<Func<T, TKey>> selector)
        {
            return sortBy switch
            {
                SortByEnum.Descending => listToSort.OrderByDescending(selector),
                _ => listToSort.OrderBy(selector)
            };
        }

        public static IOrderedQueryable<T> Sort<T, TKey, TThenKey>(this IQueryable<T> listToSort, SortByEnum sortBy, Expression<Func<T, TKey>> selector, params Expression<Func<T, TThenKey>>[] thenSelectors)
        {
            var result = sortBy switch
            {
                SortByEnum.Descending => listToSort.OrderByDescending(selector),
                _ => listToSort.OrderBy(selector)
            };

            foreach (var thenSelector in thenSelectors)
            {
                result = sortBy switch
                {
                    SortByEnum.Descending => result.ThenByDescending(thenSelector),
                    _ => result.ThenBy(thenSelector)
                };
            }

            return result;
        }

        public static IOrderedQueryable<T> Sort<T, TKey, TThenKey>(this IQueryable<T> listToSort, SortByEnum sortBy, Expression<Func<T, TKey>> selector, params (SortByEnum SortBy, Expression<Func<T, TThenKey>> Selector)[] thenSort)
        {
            var result = sortBy switch
            {
                SortByEnum.Descending => listToSort.OrderByDescending(selector),
                _ => listToSort.OrderBy(selector)
            };

            foreach (var then in thenSort)
            {
                result = then.SortBy switch
                {
                    SortByEnum.Descending => result.ThenByDescending(then.Selector),
                    _ => result.ThenBy(then.Selector)
                };
            }

            return result;
        }
    }
}