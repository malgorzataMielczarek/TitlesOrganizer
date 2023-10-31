using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models.Abstract;

namespace TitlesOrganizer.Application.ViewModels.Base
{
    public class PartialList<T> : IPartialList<T> where T : class, IBaseModel
    {
        public List<IForListVM<T>> Values { get; set; } = new List<IForListVM<T>>();

        public Paging Paging { get; set; } = new Paging();
    }
}