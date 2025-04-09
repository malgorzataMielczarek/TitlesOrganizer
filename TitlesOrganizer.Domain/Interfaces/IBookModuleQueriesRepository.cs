using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Domain.Interfaces
{
    public interface IBookModuleQueriesRepository
    {
        IQueryable<Creator> GetAllAuthors();

        IQueryable<Creator> GetAllAuthorsWithBooks();

        IQueryable<Book> GetAllBooks();

        IQueryable<Book> GetAllBooksWithAuthorsGenresAndSeries();

        IQueryable<Series> GetAllBookSeries();

        IQueryable<Series> GetAllBookSeriesWithBooks();

        IQueryable<Genre> GetAllLiteratureGenres();

        IQueryable<Genre> GetAllLiteratureGenresWithBooks();

        Creator? GetAuthor(int id);

        Creator? GetAuthorWithBooks(int id);

        Book? GetBook(int id);

        Series? GetBookSeries(int id);

        Series? GetBookSeriesWithBooks(int id);

        Book? GetBookWithCreatorsGenresAndSeries(int id);

        Genre? GetLiteratureGenre(int id);

        Genre? GetLiteratureGenreWithBooks(int id);
    }
}