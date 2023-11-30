using AutoMapper;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models.Abstract;

namespace TitlesOrganizer.Application.ViewModels.Abstract
{
    public interface IMappings
    {
        List<IForItemVM> Map<T, ItemT>(IQueryable<T> entities, ItemT item)
            where T : class, IBaseModel
            where ItemT : class, IBaseModel;

        IForItemVM Map<T, ItemT>(T entity, ItemT item)
            where T : class, IBaseModel
            where ItemT : class, IBaseModel;

        List<IForListVM> Map<T>(IEnumerable<T> entities)
            where T : class, IBaseModel;

        IPartialList Map<T>(IQueryable<T> entities, Paging paging)
            where T : class, IBaseModel;

        IListVM Map<T>(IQueryable<T> entities, Paging paging, Filtering filtering)
            where T : class, IBaseModel;

        List<IForListVM> Map<T>(IQueryable<T> entities, ref Paging paging)
            where T : class, IBaseModel;

        IForListVM Map<T>(T entity)
            where T : class, IBaseModel;

        TDestination Map<TSource, TDestination>(TSource entity, IMapper mapper);

        IDoubleListForItemVM MapToDoubleListForItem<T, ItemT>(IQueryable<T> entities, ItemT item, Paging paging, Filtering filtering)
            where T : class, IBaseModel
            where ItemT : class, IBaseModel;

        IListForItemVM MapToListForItem<T, ItemT>(IQueryable<T> entities, ItemT item, Paging paging, Filtering filtering)
            where T : class, IBaseModel
            where ItemT : class, IBaseModel;

        IOrderedQueryable<T> Sort<T>(IQueryable<T> entities, SortByEnum sortBy);
    }
}