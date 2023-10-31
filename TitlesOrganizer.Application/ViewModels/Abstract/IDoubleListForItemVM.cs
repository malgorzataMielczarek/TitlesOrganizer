using TitlesOrganizer.Domain.Models.Abstract;

namespace TitlesOrganizer.Application.ViewModels.Abstract
{
    public interface IDoubleListForItemVM<T, TItem> where T : class, IBaseModel where TItem : class, IBaseModel
    {
        List<IForItemVM<T, TItem>> SelectedValues { get; set; }
    }
}