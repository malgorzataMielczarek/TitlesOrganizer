using Microsoft.EntityFrameworkCore;
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
            if (!_context.Books.Contains(book))
            {
                _context.Books.Add(book);
                _context.SaveChanges();
            }

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
                    if (!book.Authors.Contains(author))
                    {
                        book.Authors.Add(author);

                        _context.SaveChanges();
                    }

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
                    if (!book.Genres.Contains(genre))
                    {
                        book.Genres.Add(genre);

                        _context.SaveChanges();
                    }

                    return genre.Id;
                }
            }

            return -1;
        }

        public int AddExistingSeries(int bookId, int seriesId)
        {
            var book = GetBookById(bookId);
            if (book != null)
            {
                var series = GetSeriesById(seriesId);
                if (series != null)
                {
                    if (book.BookSeries != series)
                    {
                        book.BookSeries = series;

                        _context.SaveChanges();
                    }

                    return series.Id;
                }
            }

            return -1;
        }

        public int AddNewAuthor(int bookId, Author author)
        {
            var book = GetBookById(bookId);
            if (!_context.Authors.Contains(author))
            {
                _context.Authors.Add(author);
            }

            if (book != null)
            {
                book.Authors.Add(author);
                _context.SaveChanges();

                return author.Id;
            }

            return -1;
        }

        public int AddNewGenre(int bookId, LiteratureGenre genre)
        {
            if (!_context.LiteratureGenres.Contains(genre))
            {
                _context.LiteratureGenres.Add(genre);
            }

            var book = GetBookById(bookId);
            if (book != null)
            {
                book.Genres.Add(genre);
            }

            _context.SaveChanges();

            return genre.Id;
        }

        public int AddNewSeries(int bookId, BookSeries series)
        {
            if (!_context.BookSeries.Contains(series))
            {
                _context.BookSeries.Add(series);
            }

            var book = GetBookById(bookId);
            if (book != null)
            {
                book.BookSeries = series;
            }
            _context.SaveChanges();

            return series.Id;
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

        public void DeleteSeries(int seriesId)
        {
            var series = GetSeriesById(seriesId);
            if (series != null)
            {
                _context.BookSeries.Remove(series);
                _context.SaveChanges();
            }
        }

        public IQueryable<Author> GetAllAuthors() => _context.Authors;

        public IQueryable<Author> GetAllAuthorsWithBooks() => _context.Authors.Include(a => a.Books);

        public IQueryable<Book> GetAllBooks() => _context.Books;

        public IQueryable<Book> GetAllBooksWithRelated() => _context.Books.Include(b => b.OriginalLanguage).Include(b => b.Genres).Include(b => b.Authors).Include(b => b.BookSeries);

        public IQueryable<LiteratureGenre> GetAllGenres() => _context.LiteratureGenres;

        public IQueryable<LiteratureGenre> GetAllGenresWithBooks() => _context.LiteratureGenres.Include(g => g.Books);

        public IQueryable<BookSeries> GetAllSeries() => _context.BookSeries;

        public IQueryable<BookSeries> GetAllSeriesWithBooks() => _context.BookSeries.Include(s => s.Books);

        public Author? GetAuthorById(int authorId) => _context.Authors.Include(a => a.Books).FirstOrDefault(a => a.Id == authorId);

        public IQueryable<Author>? GetAuthorsOfSeries(int seriesId) => _context.BookSeries.Include(s => s.Books).FirstOrDefault(s => s.Id == seriesId)?.Books.SelectMany(b => b.Authors).AsQueryable();

        public Book? GetBookById(int bookId) => _context.Books.Include(b => b.OriginalLanguage).Include(b => b.Genres).Include(b => b.Authors).Include(b => b.BookSeries).FirstOrDefault(b => b.Id == bookId);

        public IQueryable<Book>? GetBooksByAuthor(int authorId) => GetAuthorById(authorId)?.Books.AsQueryable();

        public IQueryable<Book>? GetBooksByGenre(int genreId) => GetGenreById(genreId)?.Books?.AsQueryable();

        public IQueryable<Book>? GetBooksInSeries(int seriesId) => _context.Books.Include(b => b.BookSeries).Where(b => b.BookSeriesId == seriesId);

        public LiteratureGenre? GetGenreById(int genreId) => _context.LiteratureGenres.Include(g => g.Books).FirstOrDefault(g => g.Id == genreId);

        public IQueryable<LiteratureGenre>? GetGenresOfSeries(int seriesId) => GetBooksInSeries(seriesId)?.SelectMany(b => b.Genres).AsQueryable();

        public IQueryable<BookSeries>? GetSeriesByAuthor(int authorId) => GetBooksByAuthor(authorId)?.SkipWhile(b => b.BookSeries == null).Select
            (b => b.BookSeries!).AsQueryable();

        public IQueryable<BookSeries>? GetSeriesByGenre(int genreId) => GetBooksByGenre(genreId)?.SkipWhile(b => b.BookSeries == null).Select(b => b.BookSeries!).AsQueryable();

        public BookSeries? GetSeriesById(int seriesId) => _context.BookSeries.Include(s => s.Books).FirstOrDefault(s => s.Id == seriesId);

        public void RemoveAuthor(int bookId, int authorId)
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

        public void RemoveGenre(int bookId, int genreId)
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

        public int UpdateSeries(BookSeries series)
        {
            _context.BookSeries.Update(series);
            if (_context.SaveChanges() == 1)
            {
                return series.Id;
            }

            return -1;
        }
    }
}