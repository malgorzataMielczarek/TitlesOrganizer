using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models.Abstract;

namespace TitlesOrganizer.Application.Mappings.Abstract
{
    public interface IMappings
    {
        IQueryable<T> Filter<T>(IQueryable<T> entities, string searchString)
            where T : BaseModel;

        List<IForItemVM> Map<T, ItemT>(IEnumerable<T> entities, ItemT item)
            where T : BaseModel
            where ItemT : BaseModel;

        IForItemVM Map<T, ItemT>(T entity, ItemT item)
            where T : BaseModel
            where ItemT : BaseModel;

        List<IForListVM> Map<T>(IEnumerable<T> entities)
            where T : BaseModel;

        IListVM Map<T>(IQueryable<T> entities, Paging paging, Filtering filtering)
            where T : BaseModel;

        List<IForListVM> Map<T>(IEnumerable<T> entities, ref Paging paging)
            where T : BaseModel;

        IForListVM Map<T>(T entity)
            where T : BaseModel;

        TDestination Map<TSource, TDestination>(TSource entity);

        IPartialListVM Map<T>(IEnumerable<T> entities, Paging paging) where T : BaseModel;

        IDoubleListForItemVM MapToDoubleListForItem<T, ItemT>(IQueryable<T> entities, ItemT item, Paging paging, Filtering filtering)
            where T : BaseModel
            where ItemT : BaseModel;

        IListForItemVM MapToListForItem<T, ItemT>(IQueryable<T> entities, ItemT item, Paging paging, Filtering filtering)
            where T : BaseModel
            where ItemT : BaseModel;

        IOrderedQueryable<T> Sort<T>(IQueryable<T> entities, SortByEnum sortBy)
            where T : BaseModel;
    }
}