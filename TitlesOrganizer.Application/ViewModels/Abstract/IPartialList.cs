using TitlesOrganizer.Application.ViewModels.Helpers;

namespace TitlesOrganizer.Application.ViewModels.Abstract
{
    public interface IPartialList
    {
        Paging Paging { get; set; }
        List<IForListVM> Values { get; set; }
    }
}