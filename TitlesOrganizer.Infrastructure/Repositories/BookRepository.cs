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

        public int AddNewAuthor(int bookId, Author author)
        {
            _context.Authors.Add(author);
            var book = GetBookById(bookId);
            if (book != null)
            {
                book.Authors.Add(author);
            }

            _context.SaveChanges();

            return author.Id;
        }

        public int AddExistingAuthor(int bookId, int authorId)
        {
            var book = GetBookById(bookId);
            if (book != null)
            {
                var author = GetAuthorById(authorId);
                if (author != null)
                {
                    book.Authors.Add(author);

                    _context.SaveChanges();

                    return author.Id;
                }
            }

            return -1;
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
            return _context.Books.Find(bookId);
        }

        public IQueryable<Book> GetBooksByAuthor(int authorId)
        {
            return _context.Books.Where(b => b.Authors.Any(a => a.Id == authorId));
        }

        public Author? GetAuthorById(int authorId)
        {
            return _context.Authors.Find(authorId);
        }
    }
}