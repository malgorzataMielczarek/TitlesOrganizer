using TitlesOrganizer.Application.ViewModels.Common;

namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class ListAuthorForBookVM : ListItems<AuthorForBookVM>
    {
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public List<AuthorForBookVM> SelectedAuthors { get; set; }

        public ListAuthorForBookVM()
        {
            BookTitle = string.Empty;
            SelectedAuthors = new List<AuthorForBookVM>();
        }

        public ListAuthorForBookVM(IQueryable<AuthorForBookVM> list, int count, SortByEnum sortBy, int pageSize, int pageNo, string searchString) : base(list, count, sortBy, pageSize, pageNo, searchString)
        {
            BookTitle = string.Empty;
            SelectedAuthors = new List<AuthorForBookVM>();
        }
    }
}