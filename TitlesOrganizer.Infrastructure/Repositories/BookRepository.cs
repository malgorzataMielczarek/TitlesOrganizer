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

        public int AddExistingGenre(int bookId, int genreId)
        {
            var book = GetBookById(bookId);
            if (book != null)
            {
                var genre = GetGenreById(genreId);
                if (genre != null)
                {
                    book.Genres.Add(genre);

                    _context.SaveChanges();

                    return genre.Id;
                }
            }

            return -1;
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

        public int AddNewGenre(int bookId, LiteratureGenre genre)
        {
            _context.LiteratureGenres.Add(genre);
            var book = GetBookById(bookId);
            if (book != null)
            {
                book.Genres.Add(genre);
            }

            _context.SaveChanges();

            return genre.Id;
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

        public void DeleteGenre(int genreId)
        {
            var genre = GetGenreById(genreId);

            if (genre != null)
            {
                _context.LiteratureGenres.Remove(genre);

                _context.SaveChanges();
            }
        }

        public IQueryable<Author> GetAllAuthors() => _context.Authors.AsQueryable();

        public IQueryable<Book> GetAllBooks() => _context.Books.AsQueryable();

        public IQueryable<LiteratureGenre> GetAllGenres() => _context.LiteratureGenres.AsQueryable();

        public Author? GetAuthorById(int authorId) => _context.Authors.Find(authorId);

        public Book? GetBookById(int bookId) => _context.Books.Find(bookId);

        public IQueryable<Book>? GetBooksByAuthor(int authorId) => GetAuthorById(authorId)?.Books.AsQueryable();

        public IQueryable<Book>? GetBooksByGenre(int genreId) => GetGenreById(genreId)?.Books?.AsQueryable();

        public LiteratureGenre? GetGenreById(int genreId) => _context.LiteratureGenres.Find(genreId);

        public void RemoveAuthorFromBook(int bookId, int authorId)
        {
            var book = GetBookById(bookId);
            if (book != null)
            {
                var author = book.Authors.FirstOrDefault(a => a.Id == authorId);
                if (author != null)
                {
                    book.Authors.Remove(author);

                    if (!author.Books.Any(b => b != book))
                    {
                        _context.Authors.Remove(author);
                    }

                    _context.SaveChanges();
                }
            }
        }

        public void RemoveGenreFromBook(int bookId, int genreId)
        {
            var book = GetBookById(bookId);
            if (book != null)
            {
                var genre = book.Genres.FirstOrDefault(a => a.Id == genreId);
                if (genre != null)
                {
                    book.Genres.Remove(genre);

                    _context.SaveChanges();
                }
            }
        }

        public int UpdateAuthor(Author author)
        {
            _context.Authors.Update(author);

            if (_context.SaveChanges() == 1)
            {
                return author.Id;
            }

            return -1;
        }

        public int UpdateBook(Book book)
        {
            _context.Books.Update(book);

            if (_context.SaveChanges() == 1)
            {
                return book.Id;
            }

            return -1;
        }

        public int UpdateGenre(LiteratureGenre genre)
        {
            _context.LiteratureGenres.Update(genre);

            if (_context.SaveChanges() == 1)
            {
                return genre.Id;
            }

            return -1;
        }
    }
}