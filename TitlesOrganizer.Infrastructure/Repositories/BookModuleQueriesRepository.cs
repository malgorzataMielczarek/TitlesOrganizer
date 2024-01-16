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

        public IQueryable<Creator> GetAllAuthors() => _context.Creators
            .Where(c => c.Profession.HasFlag(Domain.Models.Enums.Profession.Author));

        public IQueryable<Creator> GetAllAuthorsWithBooks() => _context.Creators
            .Include(a => a.Books)
            .Where(c => c.Profession.HasFlag(Domain.Models.Enums.Profession.Author));

        public IQueryable<Book> GetAllBooks() => _context.Books;

        public IQueryable<BookSeries> GetAllBookSeries() => _context.BookSeries;

        public IQueryable<BookSeries> GetAllBookSeriesWithBooks() => _context.BookSeries.Include(s => s.Books);

        public IQueryable<Book> GetAllBooksWithAuthorsGenresAndSeries() => _context.Books
            .Include(b => b.Creators.Where(c => c.Profession.HasFlag(Domain.Models.Enums.Profession.Author)))
            .Include(b => b.Genres)
            .Include(b => b.Series);

        public IQueryable<LiteratureGenre> GetAllLiteratureGenres() => _context.LiteratureGenres;

        public IQueryable<LiteratureGenre> GetAllLiteratureGenresWithBooks() => _context.LiteratureGenres.Include(g => g.Books);

        public Creator? GetAuthor(int id) => _context.Creators
            .FirstOrDefault(c => c.Id == id &&
            c.Profession.HasFlag(Domain.Models.Enums.Profession.Author));

        public Creator? GetAuthorWithBooks(int id) => _context.Creators
            .Include(c => c.Books)
            .FirstOrDefault(c => c.Id == id &&
            c.Profession.HasFlag(Domain.Models.Enums.Profession.Author));

        public Book? GetBook(int id) => _context.Books.FirstOrDefault(b => b.Id == id);

        public BookSeries? GetBookSeries(int id) => _context.BookSeries.FirstOrDefault(s => s.Id == id);

        public BookSeries? GetBookSeriesWithBooks(int id) => _context.BookSeries.Include(s => s.Books).FirstOrDefault(s => s.Id == id);

        public Book? GetBookWithCreatorsGenresAndSeries(int id) => _context.Books
            .Include(b => b.Creators)
            .Include(b => b.Genres)
            .Include(b => b.Series)
            .FirstOrDefault(b => b.Id == id);

        public LiteratureGenre? GetLiteratureGenre(int id) => _context.LiteratureGenres.FirstOrDefault(g => g.Id == id);

        public LiteratureGenre? GetLiteratureGenreWithBooks(int id) => _context.LiteratureGenres.Include(g => g.Books).FirstOrDefault(g => g.Id == id);
    }
}