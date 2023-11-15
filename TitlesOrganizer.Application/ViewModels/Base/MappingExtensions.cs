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

        public static IEnumerable<T> SkipAndTake<T>(this IEnumerable<T> values, ref Paging paging)
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