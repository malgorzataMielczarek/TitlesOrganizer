using TitlesOrganizer.Application.ViewModels.Common;

namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class ListSeriesForBookVM : ListItems<SeriesForBookVM>
    {
        public ListSeriesForBookVM()
        {

        }
        public ListSeriesForBookVM(IQueryable<SeriesForBookVM> list, int count, SortByEnum sortBy, int pageSize, int pageNo, string searchString) : base(list, count, sortBy, pageSize, pageNo, searchString)
        {

        }

        public int BookId { get; set; }
    }
}