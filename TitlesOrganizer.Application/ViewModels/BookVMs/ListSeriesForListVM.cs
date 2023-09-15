using TitlesOrganizer.Application.ViewModels.Common;

namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class ListSeriesForListVM : ListItems<SeriesForListVM>
    {
        public ListSeriesForListVM()
        { }

        public ListSeriesForListVM(IQueryable<SeriesForListVM> list, int count, SortByEnum sortBy, int pageSize, int pageNo, string searchString) : base(list, count, sortBy, pageSize, pageNo, searchString)
        {
        }
    }
}