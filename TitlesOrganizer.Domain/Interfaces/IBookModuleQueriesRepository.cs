using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Domain.Interfaces
{
    public interface IBookModuleQueriesRepository
    {
        IQueryable<Creator> GetAllAuthors();

        IQueryable<Creator> GetAllAuthorsWithBooks();

        IQueryable<Book> GetAllBooks();

        IQueryable<Book> GetAllBooksWithAuthorsGenresAndSeries();

        IQueryable<BookSeries> GetAllBookSeries();

        IQueryable<BookSeries> GetAllBookSeriesWithBooks();

        IQueryable<LiteratureGenre> GetAllLiteratureGenres();

        IQueryable<LiteratureGenre> GetAllLiteratureGenresWithBooks();

        Creator? GetAuthor(int id);

        Creator? GetAuthorWithBooks(int id);

        Book? GetBook(int id);

        BookSeries? GetBookSeries(int id);

        BookSeries? GetBookSeriesWithBooks(int id);

        Book? GetBookWithCreatorsGenresAndSeries(int id);

        LiteratureGenre? GetLiteratureGenre(int id);

        LiteratureGenre? GetLiteratureGenreWithBooks(int id);
    }
}