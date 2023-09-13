namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class BookDetailsVM
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? InSeries { get; set; }
        public string? SeriesTitle { get; set; }
        public int? SeriesId { get; set; }
        public Dictionary<int, string> Authors { get; set; } = new Dictionary<int, string>();
        public string Description { get; set; } = string.Empty;
        public string OriginalTitle { get; set; } = string.Empty;
        public string OriginalLanguage { get; set; } = string.Empty;
        public string Year { get; set; } = string.Empty;
        public string Edition { get; set; } = string.Empty;
        public Dictionary<int, string> Genres { get; set; } = new Dictionary<int, string>();
    }
}