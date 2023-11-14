using TitlesOrganizer.Domain.Interfaces;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Infrastructure.Repositories
{
    public class BookSeriesCommandsRepository : IBookSeriesCommandsRepository
    {
        private readonly Context _context;

        public BookSeriesCommandsRepository(Context context)
        {
            _context = context;
        }

        public void Delete(BookSeries series)
        {
            _context.BookSeries.Remove(series);
            _context.SaveChanges();
        }

        public int Insert(BookSeries series)
        {
            _context.BookSeries.Add(series);
            _context.SaveChanges();

            return series.Id;
        }

        public void Update(BookSeries series)
        {
            _context.Attach(series);
            _context.Entry(series).Property(nameof(BookSeries.Description)).IsModified = true;
            _context.Entry(series).Property(nameof(BookSeries.Title)).IsModified = true;
            _context.Entry(series).Property(nameof(BookSeries.OriginalTitle)).IsModified = true;
            _context.SaveChanges();
        }

        public void UpdateBookSeriesBooksRelation(BookSeries series)
        {
            _context.Attach(series);
            _context.Entry(series).Collection(nameof(BookSeries.Books)).IsModified = true;
            _context.SaveChanges();
        }
    }
}