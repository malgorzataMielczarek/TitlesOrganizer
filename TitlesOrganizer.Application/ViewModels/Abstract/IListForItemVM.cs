using System.ComponentModel.DataAnnotations;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models.Abstract;

namespace TitlesOrganizer.Application.ViewModels.Abstract
{
    public interface IListForItemVM<T, TItem> where T : class, IBaseModel where TItem : class, IBaseModel
    {
        [ScaffoldColumn(false)]
        IForListVM<TItem> Item { get; set; }

        List<IForItemVM<T, TItem>> Values { get; set; }

        [ScaffoldColumn(false)]
        Paging Paging { get; set; }

        [ScaffoldColumn(false)]
        Filtering Filtering { get; set; }
    }
}