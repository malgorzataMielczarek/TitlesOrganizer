using TitlesOrganizer.Domain.Interfaces.Base;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Domain.Interfaces
{
    public interface IBookSeriesCommandsRepository : ICommandsRepository<BookSeries>
    {
        void UpdateBookSeriesBooksRelation(BookSeries series);
    }
}