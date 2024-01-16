using TitlesOrganizer.Domain.Interfaces;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Infrastructure.Repositories
{
    public class BookCommandsRepository : IBookCommandsRepository
    {
        private readonly Context _context;

        public BookCommandsRepository(Context context)
        {
            _context = context;
        }

        public void Delete(Book book)
        {
            _context.Books.Remove(book);
            _context.SaveChanges();
        }

        public int Insert(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();

            return book.Id;
        }

        public void Update(Book book)
        {
            _context.Attach(book);
            _context.Entry(book).Property(nameof(Book.Title)).IsModified = true;
            _context.Entry(book).Property(nameof(Book.OriginalTitle)).IsModified = true;
            _context.Entry(book).Property(nameof(Book.OriginalLanguageCode)).IsModified = true;
            _context.Entry(book).Property(nameof(Book.Year)).IsModified = true;
            _context.Entry(book).Property(nameof(Book.Edition)).IsModified = true;
            _context.Entry(book).Property(nameof(Book.Description)).IsModified = true;
            _context.Entry(book).Property(nameof(Book.NumberInSeries)).IsModified = true;
            _context.SaveChanges();
        }

        public void UpdateBookAuthorsRelation(Book book)
        {
            _context.Attach(book);
            _context.Entry(book).Collection(nameof(Book.Creators)).IsModified = true;
            _context.SaveChanges();
        }

        public void UpdateBookGenresRelation(Book book)
        {
            _context.Attach(book);
            _context.Entry(book).Collection(nameof(Book.Genres)).IsModified = true;
            _context.SaveChanges();
        }

        public void UpdateBookSeriesRelation(Book book)
        {
            _context.Attach(book);
            _context.Entry(book).Reference(nameof(Book.Series)).IsModified = true;
            _context.SaveChanges();
        }
    }
}