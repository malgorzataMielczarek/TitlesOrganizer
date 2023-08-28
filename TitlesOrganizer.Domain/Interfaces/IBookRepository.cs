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

        void DeleteBook(int bookId);

        void DeleteGenre(int genreId);

        IQueryable<Author> GetAllAuthors();

        IQueryable<Book> GetAllBooks();

        IQueryable<LiteratureGenre> GetAllGenres();

        Author? GetAuthorById(int authorId);

        Book? GetBookById(int bookId);

        IQueryable<Book>? GetBooksByAuthor(int authorId);

        IQueryable<Book>? GetBooksByGenre(int genreId);

        LiteratureGenre? GetGenreById(int genreId);

        void RemoveAuthorFromBook(int bookId, int authorId);

        void RemoveGenreFromBook(int bookId, int genreId);

        int UpdateAuthor(Author author);

        int UpdateBook(Book book);

        int UpdateGenre(LiteratureGenre genre);
    }
}