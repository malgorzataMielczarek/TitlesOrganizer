namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class SeriesDetailsVM
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string OriginalTitle { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ListBookForListVM Books { get; set; } = new ListBookForListVM();
    }
}