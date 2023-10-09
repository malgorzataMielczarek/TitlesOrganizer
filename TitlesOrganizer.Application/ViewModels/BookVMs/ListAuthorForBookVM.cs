using TitlesOrganizer.Application.ViewModels.Common;

namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class ListAuthorForBookVM : ListItems<AuthorForBookVM>
    {
        public int BookId { get; set; }
        public List<AuthorForBookVM> SelectedAuthors { get; set; } = new List<AuthorForBookVM>();

        public ListAuthorForBookVM()
        { }

        public ListAuthorForBookVM(IQueryable<AuthorForBookVM> list, int count, SortByEnum sortBy, int pageSize, int pageNo, string searchString) : base(list, count, sortBy, pageSize, pageNo, searchString)
        {
        }
    }
}