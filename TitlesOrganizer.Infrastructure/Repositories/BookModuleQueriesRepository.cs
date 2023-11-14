using Microsoft.EntityFrameworkCore;
using TitlesOrganizer.Domain.Interfaces;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Infrastructure.Repositories
{
    public class BookModuleQueriesRepository : IBookModuleQueriesRepository
    {
        private readonly Context _context;

        public BookModuleQueriesRepository(Context context)
        {
            _context = context;
        }

        public IQueryable<Author> GetAllAuthors() => _context.Authors;

        public IQueryable<Author> GetAllAuthorsWithBooks() => _context.Authors.Include(a => a.Books);

        public IQueryable<Book> GetAllBooks() => _context.Books;

        public IQueryable<BookSeries> GetAllBookSeries() => _context.BookSeries;

        public IQueryable<BookSeries> GetAllBookSeriesWithBooks() => _context.BookSeries.Include(s => s.Books);

        public IQueryable<Book> GetAllBooksWithAuthorsGenresAndSeries() => _context.Books
            .Include(b => b.Authors)
            .Include(b => b.Genres)
            .Include(b => b.Series);

        public IQueryable<LiteratureGenre> GetAllLiteratureGenres() => _context.LiteratureGenres;

        public IQueryable<LiteratureGenre> GetAllLiteratureGenresWithBooks() => _context.LiteratureGenres.Include(g => g.Books);

        public Author? GetAuthor(int id) => _context.Authors.FirstOrDefault(a => a.Id == id);

        public Author? GetAuthorWithBooks(int id) => _context.Authors.Include(a => a.Books).FirstOrDefault(a => a.Id == id);

        public Book? GetBook(int id) => _context.Books.FirstOrDefault(b => b.Id == id);

        public BookSeries? GetBookSeries(int id) => _context.BookSeries.FirstOrDefault(s => s.Id == id);

        public BookSeries? GetBookSeriesWithBooks(int id) => _context.BookSeries.Include(s => s.Books).FirstOrDefault(s => s.Id == id);

        public Book? GetBookWithAuthorsGenresAndSeries(int id) => _context.Books
            .Include(b => b.Authors)
            .Include(b => b.Genres)
            .Include(b => b.Series)
            .FirstOrDefault(b => b.Id == id);

        public LiteratureGenre? GetLiteratureGenre(int id) => _context.LiteratureGenres.FirstOrDefault(g => g.Id == id);

        public LiteratureGenre? GetLiteratureGenreWithBooks(int id) => _context.LiteratureGenres.Include(g => g.Books).FirstOrDefault(g => g.Id == id);
    }
}