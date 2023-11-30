namespace TitlesOrganizer.Application.ViewModels.Abstract
{
    public interface IForItemVM
    {
        string Description { get; set; }
        int Id { get; set; }
        bool IsForItem { get; set; }
    }
}