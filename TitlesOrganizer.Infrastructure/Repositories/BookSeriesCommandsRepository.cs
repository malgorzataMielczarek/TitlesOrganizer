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

        public void Delete(Series series)
        {
            _context.BookSeries.Remove(series);
            _context.SaveChanges();
        }

        public int Insert(Series series)
        {
            _context.BookSeries.Add(series);
            _context.SaveChanges();

            return series.Id;
        }

        public void Update(Series series)
        {
            _context.Attach(series);
            _context.Entry(series).Property(nameof(Series.Description)).IsModified = true;
            _context.Entry(series).Property(nameof(Series.Title)).IsModified = true;
            _context.Entry(series).Property(nameof(Series.OriginalTitle)).IsModified = true;
            _context.SaveChanges();
        }

        public void UpdateBookSeriesBooksRelation(Series series)
        {
            _context.Attach(series);
            _context.Entry(series).Collection(nameof(Series.Books)).IsModified = true;
            _context.SaveChanges();
        }
    }
}