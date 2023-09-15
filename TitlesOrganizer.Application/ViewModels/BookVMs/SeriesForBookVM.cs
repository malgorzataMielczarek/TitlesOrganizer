namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class SeriesForBookVM
    {
        public int Id { get; set; }
        public bool IsForBook { get; set; }
        public string Title { get; set; } = string.Empty;
        public string OtherBooks { get; set; } = string.Empty;
    }
}