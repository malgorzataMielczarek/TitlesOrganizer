using TitlesOrganizer.Application.ViewModels.Helpers;

namespace TitlesOrganizer.Application.ViewModels.Abstract
{
    public interface IListVM
    {
        Filtering Filtering { get; set; }
        Paging Paging { get; set; }
        List<IForListVM> Values { get; set; }
    }
}