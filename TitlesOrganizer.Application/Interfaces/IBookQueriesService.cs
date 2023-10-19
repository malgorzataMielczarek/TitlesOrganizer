using TitlesOrganizer.Application.ViewModels.BookVMs.CommandVMs;
using TitlesOrganizer.Application.ViewModels.BookVMs.QueryVMs.DetailsVMs;
using TitlesOrganizer.Application.ViewModels.BookVMs.QueryVMs.ForListVMs;
using TitlesOrganizer.Application.ViewModels.BookVMs.QueryVMs.ReferencesVMs.ForAuthorVMs;
using TitlesOrganizer.Application.ViewModels.BookVMs.QueryVMs.ReferencesVMs.ForBookVMs;
using TitlesOrganizer.Application.ViewModels.BookVMs.QueryVMs.ReferencesVMs.ForGenreVMs;
using TitlesOrganizer.Application.ViewModels.BookVMs.QueryVMs.ReferencesVMs.ForSeriesVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;

namespace TitlesOrganizer.Application.Interfaces
{
    public interface IBookQueriesService
    {
        AuthorDetailsVM GetDetailsOfAuthor(int id, int booksPageSize, int booksPageNo, int genresPageSize, int genresPageNo, int seriesPageSize, int seriesPageNo);

        BookDetailsVM GetDetailsOfBook(int id);

        GenreDetailsVM GetDetailsOfGenre(int id);

        ListAuthorForListVM GetDetailsOfGenrePartialListOfAuthors(int genreId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        ListBookForListVM GetDetailsOfGenrePartialListOfBooks(int genreId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        ListSeriesForListVM GetDetailsOfGenrePartialListOfSeries(int genreId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        SeriesDetailsVM GeDetailsOftSeries(int id);

        ListBookForListVM GetDetailsOfSeriesPartialListOfBooks(int seriesId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        ListBookForAuthorVM GetListForAuthorOfBooks(int authorId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        ListAuthorForBookVM GetListForBookOfAuthors(int bookId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        ListGenreForBookVM GetListForBookOfGenres(int bookId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        ListSeriesForBookVM GetListForBookOfSeries(int bookId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        ListBookForGenreVM GetListForGenreOfBooks(int genreId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        ListBookForSeriesVM GetListForSeriesOfBooks(int seriesId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        ListAuthorForListVM GetListOfAuthors(SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        ListBookForListVM GetListOfBooks(SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        ListGenreForListVM GetListOfGenres(SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        ListSeriesForListVM GetListOfSeries(SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        AuthorVM GetForUpdateAuthor(int id, int pageSize, int pageNo);

        BookVM GetForUpdateBook(int id);

        GenreVM GetForUpdateGenre(int id, int pageSize, int pageNo);

        SeriesVM GetForUpdateSeries(int id, int pageSize, int pageNo);
    }
}