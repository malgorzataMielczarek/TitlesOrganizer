using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models.Abstract;

namespace TitlesOrganizer.Application.ViewModels.Base
{
    public class BaseListVM<T> : IListVM<T> where T : class, IBaseModel
    {
        public virtual List<IForListVM<T>> Values { get; set; } = new List<IForListVM<T>>();
        public Paging Paging { get; set; } = new Paging();
        public Filtering Filtering { get; set; } = new Filtering();
    }
}