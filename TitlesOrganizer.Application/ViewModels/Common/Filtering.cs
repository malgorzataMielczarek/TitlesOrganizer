namespace TitlesOrganizer.Application.ViewModels.Common
{
    public class Filtering
    {
        public SortByEnum SortBy { get; set; }
        public string SearchString { get; set; } = string.Empty;
    }
}