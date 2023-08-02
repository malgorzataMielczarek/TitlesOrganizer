using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Domain.Interfaces
{
    public interface IBookRepository
    {
        int AddBook(Book book);

        void DeleteBook(int bookId);

        Book? GetBookById(int bookId);

        IQueryable<Book> GetBooksByAuthor(int authorId);
    }
}