using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Domain.Models.Abstract;

namespace TitlesOrganizer.Application.ViewModels.Base
{
    public class BaseForListVM<T> : IForListVM<T> where T : class, IBaseModel
    {
        public int Id { get; set; }
        public virtual string Description { get; set; } = string.Empty;
    }
}