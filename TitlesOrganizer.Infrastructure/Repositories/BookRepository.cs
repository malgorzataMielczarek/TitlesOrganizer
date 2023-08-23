using TitlesOrganizer.Domain.Interfaces;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly Context _context;

        public BookRepository(Context context)
        {
            _context = context;
        }

        public int AddBook(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();

            return book.Id;
        }

        public void DeleteBook(int bookId)
        {
            Book? book = _context.Books.Find(bookId);
            if (book != null)
            {
                _context.Books.Remove(book);
                _context.SaveChanges();
            }
        }

        public Book? GetBookById(int bookId)
        {
            return _context.Books.FirstOrDefault(book => book.Id == bookId);
        }

        public IQueryable<Book> GetBooksByAuthor(int authorId)
        {
            return _context.Books.Where(b => b.Authors.Any(a => a.Id == authorId));
        }
    }
}