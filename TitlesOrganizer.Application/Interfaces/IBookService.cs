using TitlesOrganizer.Application.ViewModels.BookVMs;

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

        void DeleteBook(int id);

        ListAuthorForBookVM GetAllAuthorsForBookList(int bookId);

        ListAuthorForListVM GetAllAuthorsForList();

        ListBookForListVM GetAllBooksForList();

        List<GenreVM> GetAllGenres();

        ListGenreForBookVM GetAllGenresForBookList(int bookId);

        AuthorDetailsVM GetAuthorDetails(int id);

        BookDetailsVM GetBookDetails(int id);

        GenreDetailsVM GetGenreDetails(int id);

        GenreDetailsVM GetSeriesDetails(int id);

        void UpdateBook(BookVM book);
    }
}