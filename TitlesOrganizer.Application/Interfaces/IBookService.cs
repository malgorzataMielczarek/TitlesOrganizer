using TitlesOrganizer.Application.ViewModels.Base;
using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.Interfaces
{
    public interface IBookService
    {
        void Delete(int id);

        BookVM Get(int id);

        BookDetailsVM GetDetails(int id);

        ListBookForListVM GetList(SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        ListBookForAuthorVM GetListForAuthor(int authorId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        ListBookForGenreVM GetListForGenre(int genreId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        ListBookForSeriesVM GetListForSeries(int seriesId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        PartialList<Book> GetPartialListForAuthor(int authorId, int pageSize, int pageNo);

        PartialList<Book> GetPartialListForGenre(int genreId, int pageSize, int pageNo);

        void SelectAuthors(int bookId, List<int> selectedIds);

        void SelectGenres(int bookId, List<int> selectedIds);

        void SelectSeries(int bookId, int? selectedId);

        int Upsert(BookVM book);
    }
}