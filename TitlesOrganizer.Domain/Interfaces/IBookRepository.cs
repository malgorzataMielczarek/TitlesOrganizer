using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Domain.Interfaces
{
    public interface IBookRepository
    {
        int AddBook(Book book);

        int AddExistingAuthor(int bookId, int authorId);

        int AddExistingGenre(int bookId, int genreId);

        int AddNewAuthor(int bookId, Author author);

        int AddNewGenre(int bookId, LiteratureGenre genre);

        int AddSeries(BookSeries series);

        void DeleteBook(int bookId);

        void DeleteGenre(int genreId);

        void DeleteSeries(int seriesId);

        IQueryable<Author> GetAllAuthors();
        IQueryable<Author> GetAllAuthorsWithBooks();
        IQueryable<Book> GetAllBooks();
        IQueryable<Book> GetAllBooksWithRelated();
        IQueryable<LiteratureGenre> GetAllGenres();
        IQueryable<LiteratureGenre> GetAllGenresWithBooks();
        IQueryable<BookSeries> GetAllSeries();
        IQueryable<BookSeries> GetAllSeriesWithBooks();
        Author? GetAuthorById(int authorId);

        IQueryable<Author>? GetAuthorsOfSeries(int seriesId);

        Book? GetBookById(int bookId);

        IQueryable<Book>? GetBooksByAuthor(int authorId);

        IQueryable<Book>? GetBooksByGenre(int genreId);

        IQueryable<Book>? GetBooksInSeries(int seriesId);

        LiteratureGenre? GetGenreById(int genreId);

        IQueryable<LiteratureGenre>? GetGenresOfSeries(int seriesId);

        IQueryable<BookSeries>? GetSeriesByAuthor(int authorId);

        IQueryable<BookSeries>? GetSeriesByGenre(int genreId);

        BookSeries? GetSeriesById(int seriesId);

        void RemoveAuthor(int bookId, int authorId);

        void RemoveGenre(int bookId, int genreId);

        int UpdateAuthor(Author author);

        int UpdateBook(Book book);

        int UpdateGenre(LiteratureGenre genre);

        int UpdateSeries(BookSeries series);
    }
}