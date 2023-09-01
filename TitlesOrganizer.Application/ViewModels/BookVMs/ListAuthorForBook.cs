namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class ListAuthorForBook
    {
        public List<AuthorForBook> Authors { get; set; } = new List<AuthorForBook>();
        public int Count { get; set; }
        public int BookId { get; set; }
    }
}