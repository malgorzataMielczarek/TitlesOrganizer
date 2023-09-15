using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Application.ViewModels.Common;

namespace TitlesOrganizer.Application.Interfaces
{
    public interface IBookService
    {
        int AddAuthor(NewAuthorVM author);

        void AddAuthorsForBook(int bookId, List<int> authorsIds);

        int AddBook(BookVM book);

        int AddGenre(GenreVM genre);

        int AddGenre(int bookId, GenreVM genre);

        void AddGenresForBook(int bookId, List<int> genresIds);

        int AddNewSeries(NewSeriesVM newSeries);

        void AddSeriesForBook(int bookId, int seriesId);

        void DeleteBook(int id);

        ListAuthorForBookVM GetAllAuthorsForBookList(int bookId, ViewModels.Common.SortByEnum sortBy, int pageSize, int pageNo, string searchString);

        ListAuthorForListVM GetAllAuthorsForList(ViewModels.Common.SortByEnum sortBy, int pageSize, int pageNo, string searchString);

        ListBookForListVM GetAllBooksForList(ViewModels.Common.SortByEnum sortBy, int pageSize, int pageNo, string searchString);

        ListGenreVM GetAllGenres(ViewModels.Common.SortByEnum sortBy, int pageSize, int pageNo, string searchString);

        ListGenreForBookVM GetAllGenresForBookList(int bookId, ViewModels.Common.SortByEnum sortBy, int pageSize, int pageNo, string searchString);

        ListSeriesForBookVM GetAllSeriesForBookList(int bookId, SortByEnum sortBy, int pageSize, int pageNo, string searchString);

        ListSeriesForListVM GetAllSeriesForList(SortByEnum sortBy, int pageSize, int pageNo, string searchString);

        AuthorDetailsVM GetAuthorDetails(int id, ViewModels.Common.SortByEnum sortBy, int pageSize, int pageNo, string searchString);

        BookDetailsVM GetBookDetails(int id);

        GenreDetailsVM GetGenreDetails(int id, ViewModels.Common.SortByEnum sortBy, int pageSize, int pageNo, string searchString);

        SeriesDetailsVM GetSeriesDetails(int id, ViewModels.Common.SortByEnum sortBy, int pageSize, int pageNo, string searchString);

        void UpdateBook(BookVM book);
    }
}