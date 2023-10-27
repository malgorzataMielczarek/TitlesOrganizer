using TitlesOrganizer.Domain.Interfaces;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Infrastructure.Repositories
{
    public class BookQueriesRepository : IBookQueriesRepository
    {
        private readonly Context _context;

        public BookQueriesRepository(Context context)
        {
            _context = context;
        }

        public IQueryable<Author> GetAllAuthors()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Author> GetAllAuthorsWithBooks()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Book> GetAllBooks()
        {
            throw new NotImplementedException();
        }

        public IQueryable<BookSeries> GetAllBookSeries()
        {
            throw new NotImplementedException();
        }

        public IQueryable<BookSeries> GetAllBookSeriesWithBooks()
        {
            throw new NotImplementedException();
        }

        public IQueryable<LiteratureGenre> GetAllLiteratureGenres()
        {
            throw new NotImplementedException();
        }

        public IQueryable<LiteratureGenre> GetAllLiteratureGenresWithBooks()
        {
            throw new NotImplementedException();
        }

        public Author? GetAuthor(int id)
        {
            throw new NotImplementedException();
        }

        public Author? GetAuthorWithBooks(int id)
        {
            throw new NotImplementedException();
        }

        public Book? GetBook(int id)
        {
            throw new NotImplementedException();
        }

        public BookSeries? GetBookSeries(int id)
        {
            throw new NotImplementedException();
        }

        public BookSeries? GetBookSeriesWithBooks(int id)
        {
            throw new NotImplementedException();
        }

        public Book? GetBookWithAllRelatedObjects(int id)
        {
            throw new NotImplementedException();
        }

        public LiteratureGenre? GetLiteratureGenre(int id)
        {
            throw new NotImplementedException();
        }

        public LiteratureGenre? GetLiteratureGenreWithBooks(int id)
        {
            throw new NotImplementedException();
        }
    }
}