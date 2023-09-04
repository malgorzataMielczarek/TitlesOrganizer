namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class ListAuthorForBookVM
    {
        public List<AuthorForBookVM> Authors { get; set; } = new List<AuthorForBookVM>();
        public int Count { get; set; }
        public int BookId { get; set; }
    }
}