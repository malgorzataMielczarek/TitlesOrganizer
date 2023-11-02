using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models.Abstract;

namespace TitlesOrganizer.Application.ViewModels.Base
{
    public static class MappingExtensions
    {
        public static IForListVM<T> Map<T>(this T item)
            where T : class, IBaseModel
        {
            return new BaseForListVM<T>() { Id = item.Id };
        }

        public static IForItemVM<T, TItem> MapForItem<T, TItem>(this T entity, TItem item)
            where T : class, IBaseModel
            where TItem : class, IBaseModel
        {
            return new BaseForItemVM<T, TItem>() { Id = entity.Id };
        }

        public static IQueryable<IForItemVM<T, TItem>> MapForItem<T, TItem>(this IQueryable<T> sortedList, TItem item)
            where T : class, IBaseModel
            where TItem : class, IBaseModel
        {
            return sortedList.Select(it => it.MapForItem(item));
        }

        public static TList MapForItemToDoubleList<T, TItem, TList>(this IQueryable<T> sortedList, TItem item, Paging paging, Filtering filtering)
            where T : class, IBaseModel
            where TItem : class, IBaseModel
            where TList : class, IDoubleListForItemVM<T, TItem>, new()
        {
            var query = sortedList.MapForItem(item);
            var selectedValues = query.Where(it => it.IsForItem).ToList();
            var notSelectedValues = query.Where(it => !it.IsForItem && it.Description.Contains(filtering.SearchString));
            paging.Count = notSelectedValues.Count();
            var limitedList = notSelectedValues
                .Skip(paging.PageSize * (paging.CurrentPage - 1))
                .Take(paging.PageSize)
                .ToList();

            return new TList()
            {
                Item = item.Map(),
                SelectedValues = selectedValues,
                Values = limitedList,
                Paging = paging,
                Filtering = filtering
            };
        }

        public static TList MapForItemToList<T, TItem, TList>(this IQueryable<T> sortedList, TItem item, Paging paging, Filtering filtering)
            where T : class, IBaseModel
            where TItem : class, IBaseModel
            where TList : class, IListForItemVM<T, TItem>, new()
        {
            var values = sortedList
                .MapForItem(item)
                .Where(it => it.IsForItem || it.Description.Contains(filtering.SearchString))
                .OrderByDescending(it => it.IsForItem);
            paging.Count = values.Count();
            var limitedList = values
                .Skip(paging.PageSize * (paging.CurrentPage - 1))
                .Take(paging.PageSize)
                .ToList();

            return new TList()
            {
                Item = item.Map(),
                Values = limitedList,
                Paging = paging,
                Filtering = filtering
            };
        }

        public static IQueryable<IForListVM<T>> Map<T>(this IQueryable<T> items) where T : class, IBaseModel
        {
            return items.Select(it => it.Map());
        }

        public static List<IForListVM<T>> Map<T>(this IEnumerable<T> items) where T : class, IBaseModel
        {
            return items.Select(it => it.Map()).ToList();
        }

        public static IListVM<T> MapToList<T>(this IQueryable<T> sortedItems, Paging paging, Filtering filtering)
            where T : class, IBaseModel
        {
            var queryable = sortedItems
                .Map()
                .Where(a => a.Description.Contains(filtering.SearchString));
            paging.Count = queryable.Count();
            var limitedList = queryable
                .Skip(paging.PageSize * (paging.CurrentPage - 1))
                .Take(paging.PageSize)
                .ToList();

            return new BaseListVM<T>()
            {
                Values = limitedList,
                Paging = paging,
                Filtering = filtering
            };
        }

        public static IQueryable<IForListVM<T>> MapToList<T>(this IQueryable<T> sortedItems, ref Paging paging)
            where T : class, IBaseModel
        {
            paging.Count = sortedItems.Count();
            var limitedList = sortedItems
                .Skip(paging.PageSize * (paging.CurrentPage - 1))
                .Take(paging.PageSize)
                .Map();

            return limitedList;
        }

        public static List<IForListVM<T>> MapToList<T>(this IEnumerable<T> sortedItems, ref Paging paging)
            where T : class, IBaseModel
        {
            paging.Count = sortedItems.Count();
            var limitedList = sortedItems
                .Skip(paging.PageSize * (paging.CurrentPage - 1))
                .Take(paging.PageSize)
                .Map();

            return limitedList;
        }
    }
}