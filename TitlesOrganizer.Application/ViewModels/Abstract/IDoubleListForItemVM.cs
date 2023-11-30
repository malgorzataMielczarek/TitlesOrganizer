namespace TitlesOrganizer.Application.ViewModels.Abstract
{
    public interface IDoubleListForItemVM : IListForItemVM
    {
        List<IForItemVM> SelectedValues { get; set; }
    }
}