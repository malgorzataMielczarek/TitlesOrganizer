using TitlesOrganizer.Application.ViewModels.BookVMs.CommendVMs.UpsertModelVMs;
using TitlesOrganizer.Application.ViewModels.Common;

namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class ListGenreVM : ListItems<GenreVM>
    {
        public ListGenreVM()
        { }

        public ListGenreVM(IQueryable<GenreVM> list, int count, SortByEnum sortBy, int pageSize, int pageNo, string searchString) : base(list, count, sortBy, pageSize, pageNo, searchString)
        {
        }
    }
}