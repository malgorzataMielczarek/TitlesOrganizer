using TitlesOrganizer.Application.ViewModels.Common;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;
using TitlesOrganizer.Application.ViewModels.Concrete.BookVMs;

namespace TitlesOrganizer.Application.Interfaces
{
    public interface IBookSeriesService
    {
        void Delete(int id);

        SeriesVM Get(int id, int booksPageSize, int booksPageNo);

        SeriesDetailsVM GetDetails(int id, int booksPageSize, int booksPageNo);

        ListSeriesForListVM GetList(SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        ListSeriesForBookVM GetListForBook(int bookId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        PartialListVM<BookSeries> GetPartialListForAuthor(int authorId, int pageSize, int pageNo);

        PartialListVM<BookSeries> GetPartialListForGenre(int genreId, int pageSize, int pageNo);

        void SelectBooks(int seriesId, int[] selectedIds);

        int Upsert(SeriesVM series);
    }
}