using TitlesOrganizer.Application.ViewModels.Helpers;

namespace TitlesOrganizer.Application.ViewModels.Abstract
{
    public interface IPartialListVM
    {
        Paging Paging { get; set; }
        List<IForListVM> Values { get; set; }
    }
}