namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class ListAuthorForListVM
    {
        public List<AuthorForListVM> Authors { get; set; } = new List<AuthorForListVM>();
        public int Count { get; set; }
    }
}