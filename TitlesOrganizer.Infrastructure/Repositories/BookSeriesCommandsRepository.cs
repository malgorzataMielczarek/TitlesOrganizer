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
            throw new NotImplementedException();
        }

        public int Insert(BookSeries series)
        {
            throw new NotImplementedException();
        }

        public void Update(BookSeries series)
        {
            throw new NotImplementedException();
        }

        public void UpdateBookSeriesBooksRelation(BookSeries series)
        {
            throw new NotImplementedException();
        }
    }
}