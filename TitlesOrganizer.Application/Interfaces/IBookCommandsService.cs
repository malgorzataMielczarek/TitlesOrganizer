using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;

namespace TitlesOrganizer.Application.Interfaces
{
    public interface IBookCommandsService
    {
        void Delete(int id);

        BookVM Get(int id);

        BookDetailsVM GetDetails(int id);

        ListBookForListVM GetList(SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        ListBookForAuthorVM GetListForAuthor(int authorId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        ListBookForGenreVM GetListForGenre(int genreId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        ListBookForSeriesVM GetListForSeries(int seriesId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        PartialList<BookForListVM> GetPartialListForAuthor(int authorId, int pageSize, int pageNo);

        PartialList<BookForListVM> GetPartialListForGenre(int genreId, int pageSize, int pageNo);

        void SelectForAuthor(int authorId, List<int> selectedIds);

        void SelectForGenre(int genreId, List<int> selectedIds);

        void SelectForSeries(int seriesId, List<int> selectedIds);

        int Upsert(BookVM book);
    }
}