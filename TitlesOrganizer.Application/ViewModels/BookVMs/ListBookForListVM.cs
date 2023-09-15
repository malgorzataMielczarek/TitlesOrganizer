using TitlesOrganizer.Application.ViewModels.Common;

namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class ListBookForListVM : ListItems<BookForListVM>
    {
        public ListBookForListVM()
        { }

        public ListBookForListVM(IQueryable<BookForListVM> list, int count, SortByEnum sortBy, int pageSize, int pageNo, string searchString) : base(list, count, sortBy, pageSize, pageNo, searchString)
        {
        }
    }
}