using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models.Abstract;

namespace TitlesOrganizer.Application.ViewModels.Base
{
    public static class MappingExtensions
    {
        public static TUpdate Map<T, TUpdate>(this T item)
            where T : class, IBaseModel
            where TUpdate : class, IUpdateVM<T>, new()
        {
            return new TUpdate() { Id = item.Id };
        }

        public static IForListVM<T> Map<T>(this T item)
            where T : class, IBaseModel
        {
            return new BaseForListVM<T>() { Id = item.Id };
        }

        public static TDetails MapToDetails<T, TDetails>(this T item)
            where T : class, IBaseModel
            where TDetails : class, IDetailsVM<T>, new()
        {
            return new TDetails() { Id = item.Id };
        }

        public static IForItemVM<T, TItem> MapForItem<T, TItem>(this T item)
            where T : class, IBaseModel
            where TItem : class, IBaseModel
        {
            return new BaseForItemVM<T, TItem>() { Id = item.Id };
        }

        public static IQueryable<IForListVM<T>> Map<T>(this IQueryable<T> items) where T : class, IBaseModel
        {
            return items.Select(it => it.Map());
        }

        public static List<IForListVM<T>> Map<T>(this ICollection<T> items) where T : class, IBaseModel
        {
            return items.Select(it => it.Map()).ToList();
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
    }
}