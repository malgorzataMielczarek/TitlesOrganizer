using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models.Abstract;

namespace TitlesOrganizer.Application.ViewModels.Base
{
    public static class MappingExtensions
    {
        public static TList MapForItemToDoubleList<T, TItem, TList>(this IQueryable<IForItemVM<T, TItem>> sortedList, IForListVM<TItem> item, Paging paging, Filtering filtering)
            where T : class, IBaseModel
            where TItem : class, IBaseModel
            where TList : class, IDoubleListForItemVM<T, TItem>, new()
        {
            var selectedValues = sortedList.Where(it => it.IsForItem).ToList();
            var notSelectedValues = sortedList.Where(it => !it.IsForItem && it.Description.Contains(filtering.SearchString));
            var limitedList = notSelectedValues.SkipAndTake(ref paging).ToList();

            return new TList()
            {
                Item = item,
                SelectedValues = selectedValues,
                Values = limitedList,
                Paging = paging,
                Filtering = filtering
            };
        }

        public static TList MapForItemToList<T, TItem, TList>(this IQueryable<IForItemVM<T, TItem>> sortedList, IForListVM<TItem> item, Paging paging, Filtering filtering)
            where T : class, IBaseModel
            where TItem : class, IBaseModel
            where TList : class, IListForItemVM<T, TItem>, new()
        {
            var values = sortedList
                .Where(it => it.IsForItem || it.Description.Contains(filtering.SearchString))
                .OrderByDescending(it => it.IsForItem);
            var limitedList = values.SkipAndTake(ref paging).ToList();

            return new TList()
            {
                Item = item,
                Values = limitedList,
                Paging = paging,
                Filtering = filtering
            };
        }

        public static TList MapToList<T, TList>(this IQueryable<IForListVM<T>> sortedItems, Paging paging, Filtering filtering)
            where T : class, IBaseModel
            where TList : class, IListVM<T>, new()
        {
            var queryable = sortedItems
                .Where(a => a.Description.Contains(filtering.SearchString));
            var limitedList = queryable.SkipAndTake(ref paging).ToList();

            return new TList()
            {
                Values = limitedList,
                Paging = paging,
                Filtering = filtering
            };
        }

        public static TList MapToList<T, TList>(this IEnumerable<IForListVM<T>> sortedItems, Paging paging, Filtering filtering)
            where T : class, IBaseModel
            where TList : class, IListVM<T>, new()
        {
            var queryable = sortedItems
                .Where(a => a.Description.Contains(filtering.SearchString));
            var limitedList = queryable.SkipAndTake(ref paging).ToList();

            return new TList()
            {
                Values = limitedList,
                Paging = paging,
                Filtering = filtering
            };
        }

        public static IQueryable<IForListVM<T>> MapToList<T>(this IQueryable<IForListVM<T>> sortedItems, ref Paging paging)
            where T : class, IBaseModel
        {
            return sortedItems.SkipAndTake(ref paging);
        }

        public static List<IForListVM<T>> MapToList<T>(this List<IForListVM<T>> sortedItems, ref Paging paging)
            where T : class, IBaseModel
        {
            return sortedItems.SkipAndTake(ref paging).ToList();
        }

        private static IQueryable<T> SkipAndTake<T>(this IQueryable<T> values, ref Paging paging)
        {
            if (values?.Any() ?? false)
            {
                paging.Count = values.Count();
                return values.Skip(paging.PageSize * (paging.CurrentPage - 1))
                .Take(paging.PageSize);
            }
            else
            {
                paging.CurrentPage = 1;
                paging.Count = 0;
                return new List<T>().AsQueryable();
            }
        }

        private static IEnumerable<T> SkipAndTake<T>(this IEnumerable<T> values, ref Paging paging)
        {
            if (values?.Any() ?? false)
            {
                paging.Count = values.Count();
                return values.Skip(paging.PageSize * (paging.CurrentPage - 1))
                .Take(paging.PageSize);
            }
            else
            {
                paging.CurrentPage = 1;
                paging.Count = 0;
                return new List<T>();
            }
        }
    }
}