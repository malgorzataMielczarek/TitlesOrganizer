using TitlesOrganizer.Application.ViewModels.BookVMs.DetailsVMs;
using TitlesOrganizer.Application.ViewModels.BookVMs.ForListVMs;
using TitlesOrganizer.Application.ViewModels.BookVMs.ReferencesVMs.ForAuthorVMs;
using TitlesOrganizer.Application.ViewModels.BookVMs.ReferencesVMs.ForBookVMs;
using TitlesOrganizer.Application.ViewModels.BookVMs.ReferencesVMs.ForGenreVMs;
using TitlesOrganizer.Application.ViewModels.BookVMs.ReferencesVMs.ForSeriesVMs;
using TitlesOrganizer.Application.ViewModels.BookVMs.UpdateVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;

namespace TitlesOrganizer.Application.Interfaces
{
    public interface IBookQueriesService
    {
        AuthorVM GetAuthor(int id, int booksPageSize, int booksPageNo);

        AuthorDetailsVM GetAuthorDetails(int id, int booksPageSize, int booksPageNo, int authorsPageSize, int authorsPageNo, int seriesPageSize, int seriesPageNo);

        ListAuthorForListVM GetAuthorsList(SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        ListAuthorForBookVM GetAuthorsListForBook(int bookId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        PartialList<AuthorForListVM> GetAuthorsPartialListForGenre(int genreId, int pageSize, int pageNo);

        BookVM GetBook(int id);

        BookDetailsVM GetBookDetails(int id);

        ListBookForListVM GetBooksList(SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        ListBookForAuthorVM GetBooksListForAuthor(int authorId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        ListBookForGenreVM GetBooksListForGenre(int genreId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        ListBookForSeriesVM GetBooksListForSeries(int seriesId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        PartialList<BookForListVM> GetBooksPartialListForAuthor(int authorId, int pageSize, int pageNo);

        PartialList<BookForListVM> GetBooksPartialListForGenre(int genreId, int pageSize, int pageNo);

        GenreVM GetGenre(int id, int pageSize, int pageNo);

        GenreDetailsVM GetGenreDetails(int id, int booksPageSize, int booksPageNo, int authorsPageSize, int authorsPageNo, int seriesPageSize, int seriesPageNo);

        ListGenreForListVM GetGenresList(SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        ListGenreForBookVM GetGenresListForBook(int bookId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        PartialList<GenreForListVM> GetGenresPartialListForAuthor(int authorId, int pageSize, int pageNo);

        SeriesVM GetSeries(int id, int pageSize, int pageNo);

        SeriesDetailsVM GetSeriesDetails(int id, int booksPageSize, int booksPageNo);

        ListSeriesForListVM GetSeriesList(SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        ListSeriesForBookVM GetSeriesListForBook(int bookId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        PartialList<SeriesForListVM> GetSeriesPartialListForAuthor(int authorId, int pageSize, int pageNo);

        PartialList<SeriesForListVM> GetSeriesPartialListForGenre(int genreId, int pageSize, int pageNo);
    }
}