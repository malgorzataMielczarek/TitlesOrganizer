using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Domain.Models.Abstract;

namespace TitlesOrganizer.Application.ViewModels.Base
{
    public class BaseForItemVM<T, TItem> : IForItemVM<T, TItem> where T : class, IBaseModel where TItem : class, IBaseModel
    {
        public virtual bool IsForItem { get; set; } = false;
        public int Id { get; set; }
        public virtual string Description { get; set; } = string.Empty;
    }
}