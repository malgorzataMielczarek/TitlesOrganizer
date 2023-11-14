using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Domain.Interfaces
{
    public interface IBookModuleQueriesRepository
    {
        IQueryable<Author> GetAllAuthors();

        IQueryable<Author> GetAllAuthorsWithBooks();

        IQueryable<Book> GetAllBooks();

        IQueryable<Book> GetAllBooksWithAuthorsGenresAndSeries();

        IQueryable<BookSeries> GetAllBookSeries();

        IQueryable<BookSeries> GetAllBookSeriesWithBooks();

        IQueryable<LiteratureGenre> GetAllLiteratureGenres();

        IQueryable<LiteratureGenre> GetAllLiteratureGenresWithBooks();

        Author? GetAuthor(int id);

        Author? GetAuthorWithBooks(int id);

        Book? GetBook(int id);

        BookSeries? GetBookSeries(int id);

        BookSeries? GetBookSeriesWithBooks(int id);

        Book? GetBookWithAuthorsGenresAndSeries(int id);

        LiteratureGenre? GetLiteratureGenre(int id);

        LiteratureGenre? GetLiteratureGenreWithBooks(int id);
    }
}