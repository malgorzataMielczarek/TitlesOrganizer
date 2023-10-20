using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Domain.Interfaces
{
    public interface IBookQueriesRepository
    {
        IQueryable<Author> GetAllAuthors();

        IQueryable<Author> GetAllAuthorsWithBooks();

        IQueryable<Book> GetAllBooks();

        IQueryable<BookSeries> GetAllBookSeries();

        IQueryable<BookSeries> GetAllBookSeriesWithBooks();

        IQueryable<LiteratureGenre> GetAllLiteratureGenres();

        IQueryable<LiteratureGenre> GetAllLiteratureGenresWithBooks();

        Author GetAuthor(int id);

        Author GetAuthorWithBooks(int id);

        Book GetBook(int id);

        BookSeries GetBookSeries(int id);

        BookSeries GetBookSeriesWithBooks(int id);

        Book GetBookWithAllRelatedObjects(int id);

        LiteratureGenre GetLiteratureGenre(int id);

        LiteratureGenre GetLiteratureGenreWithBooks(int id);
    }
}