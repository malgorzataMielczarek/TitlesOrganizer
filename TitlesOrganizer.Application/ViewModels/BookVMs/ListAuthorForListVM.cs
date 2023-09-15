using TitlesOrganizer.Application.ViewModels.Common;

namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class ListAuthorForListVM : ListItems<AuthorForListVM>
    {
        public ListAuthorForListVM()
        { }

        public ListAuthorForListVM(IQueryable<AuthorForListVM> list, int count, SortByEnum sortBy, int pageSize, int pageNo, string searchString) : base(list, count, sortBy, pageSize, pageNo, searchString)
        {
        }
    }
}