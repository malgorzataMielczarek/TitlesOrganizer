namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class ListGenreForBookVM
    {
        public List<GenreForBookVM> Genres { get; set; } = new List<GenreForBookVM>();
        public int Count { get; set; }
        public int BookId { get; set; }
    }
}