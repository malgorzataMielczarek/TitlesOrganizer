using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Domain.Interfaces
{
    public interface IBookRepository
    {
        int AddBook(Book book);

        int AddToExistingSeries(int bookId, int seriesId);

        int AddToNewSeries(int bookId, BookSeries series);

        int AddExistingAuthor(int bookId, int authorId);

        int AddExistingGenre(int bookId, int genreId);

        int AddNewAuthor(int bookId, Author author);

        int AddNewGenre(int bookId, LiteratureGenre genre);

        int AddOriginalLanguage(int bookId, int languageId);

        void DeleteBook(int bookId);

        Author? GetAuthorById(int authorId);

        Book? GetBookById(int bookId);

        IQueryable<Book> GetBooksByAuthor(int authorId);
    }
}