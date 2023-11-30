using TitlesOrganizer.Application.ViewModels.Helpers;

namespace TitlesOrganizer.Application.ViewModels.Abstract
{
    public interface IListForItemVM
    {
        Filtering Filtering { get; set; }
        IForListVM Item { get; set; }
        Paging Paging { get; set; }
        List<IForItemVM> Values { get; set; }
    }
}