using AutoMapper;
using TitlesOrganizer.Application.Mappings.Abstract;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Common;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models.Abstract;

namespace TitlesOrganizer.Application.Mappings.Common
{
    public class BaseMappings(IMapper _mapper) : IMappings
    {
        public static List<T> SkipAndTake<T>(IEnumerable<T> values, ref Paging paging)
        {
            if (values?.Any() ?? false)
            {
                paging.Count = values.Count();
                return values
                    .Skip(paging.PageSize * (paging.CurrentPage - 1))
                    .Take(paging.PageSize)
                    .ToList();
            }
            else
            {
                paging.CurrentPage = 1;
                paging.Count = 0;
                return new List<T>();
            }
        }

        public virtual IForListVM Map<T>(T entity)
                    where T : class, IBaseModel
        {
            return new ForListVM()
            {
                Id = entity.Id
            };
        }

        public virtual List<IForListVM> Map<T>(IEnumerable<T> entities)
            where T : class, IBaseModel
        {
            return Sort(entities.AsQueryable(), SortByEnum.Ascending)
                .Select(it => Map(it))
                .ToList();
        }

        public virtual IForItemVM Map<T, ItemT>(T entity, ItemT item)
            where T : class, IBaseModel
            where ItemT : class, IBaseModel
        {
            return new ForItemVM() { Id = entity.Id };
        }

        public virtual List<IForItemVM> Map<T, ItemT>(IQueryable<T> entities, ItemT item)
            where T : class, IBaseModel
            where ItemT : class, IBaseModel
        {
            return entities.Select(it => Map(it, item)).ToList();
        }

        public virtual IListVM Map<T>(IQueryable<T> entities, Paging paging, Filtering filtering)
            where T : class, IBaseModel
        {
            var values = Sort(entities, filtering.SortBy)
                .Select(it => Map(it))
                .Where(it => it.Description.Contains(filtering.SearchString))
                .ToList();
            var limitedList = SkipAndTake(values, ref paging);

            return new ListVM(limitedList, paging, filtering);
        }

        public virtual List<IForListVM> Map<T>(IEnumerable<T> entities, ref Paging paging)
            where T : class, IBaseModel
        {
            var list = Map(entities);
            return SkipAndTake(list, ref paging);
        }

        public virtual IPartialListVM Map<T>(IEnumerable<T> entities, Paging paging)
            where T : class, IBaseModel
        {
            return new PartialListVM()
            {
                Values = Map(entities, ref paging),
                Paging = paging
            };
        }

        public virtual TDestination Map<TSource, TDestination>(TSource entity)
        {
            return _mapper.Map<TDestination>(entity);
        }

        public virtual IDoubleListForItemVM MapToDoubleListForItem<T, ItemT>(IQueryable<T> entities, ItemT item, Paging paging, Filtering filtering)
            where T : class, IBaseModel
            where ItemT : class, IBaseModel
        {
            var sorted = Sort(entities, filtering.SortBy);
            var mapped = Map(sorted, item);
            var selectedValues = mapped.Where(it => it.IsForItem).ToList();
            var notSelectedValues = mapped.Where(it => !it.IsForItem && it.Description.Contains(filtering.SearchString));
            var limitedList = SkipAndTake(notSelectedValues, ref paging).ToList();

            return new DoubleListForItemVM(limitedList, selectedValues, Map(item), paging, filtering);
        }

        public virtual IListForItemVM MapToListForItem<T, ItemT>(IQueryable<T> entities, ItemT item, Paging paging, Filtering filtering)
                                                    where T : class, IBaseModel
            where ItemT : class, IBaseModel
        {
            var sorted = Sort(entities, filtering.SortBy);
            var values = Map(sorted, item)
                .Where(it => it.IsForItem || it.Description.Contains(filtering.SearchString))
                .OrderByDescending(it => it.IsForItem);
            var limitedList = SkipAndTake(values, ref paging).ToList();

            return new ListForItemVM(limitedList, Map(item), paging, filtering);
        }

        public virtual IOrderedQueryable<T> Sort<T>(IQueryable<T> entities, SortByEnum sortBy)
            where T : class, IBaseModel
        {
            return entities.Order();
        }
    }
}