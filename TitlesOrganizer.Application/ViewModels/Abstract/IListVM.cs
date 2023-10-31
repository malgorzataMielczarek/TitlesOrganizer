using System.ComponentModel.DataAnnotations;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models.Abstract;

namespace TitlesOrganizer.Application.ViewModels.Abstract
{
    public interface IListVM<T> where T : class, IBaseModel
    {
        List<IForListVM<T>> Values { get; set; }

        [ScaffoldColumn(false)]
        Paging Paging { get; set; }

        [ScaffoldColumn(false)]
        Filtering Filtering { get; set; }
    }
}