using TitlesOrganizer.Application.ViewModels.Common;

namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class ListGenreForBookVM : ListItems<GenreForBookVM>
    {
        public int BookId { get; set; }

        public ListGenreForBookVM()
        { }

        public ListGenreForBookVM(IQueryable<GenreForBookVM> list, int count, SortByEnum sortBy, int pageSize, int pageNo, string searchString) : base(list, count, sortBy, pageSize, pageNo, searchString)
        {
        }
    }
}