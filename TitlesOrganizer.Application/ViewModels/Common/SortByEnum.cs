using System.ComponentModel;

namespace TitlesOrganizer.Application.ViewModels.Common
{
    public enum SortByEnum
    {
        [Description("A-Z")]
        Ascending = 1,

        [Description("Z-A")]
        Descending = 2
    }

    public static class SortByExtensions
    {
        public static IOrderedQueryable<T> Sort<T, TKey>(this IQueryable<T> listToSort, SortByEnum sortBy, System.Linq.Expressions.Expression<Func<T, TKey>> selector)
        {
            return sortBy switch
            {
                SortByEnum.Descending => listToSort.OrderByDescending(selector),
                _ => listToSort.OrderBy(selector)
            };
        }

        public static string SortByEnumToName(this SortByEnum sortByEnum)
        {
            return sortByEnum switch
            {
                SortByEnum.Ascending => "A-Z",
                SortByEnum.Descending => "Z-A",
                _ => ""
            };
        }

        public static SortByEnum SortByNameToEnum(this string sortByName)
        {
            return sortByName switch
            {
                "A-Z" => SortByEnum.Ascending,
                "Z-A" => SortByEnum.Descending,
                _ => SortByEnum.Ascending
            };
        }
    }
}