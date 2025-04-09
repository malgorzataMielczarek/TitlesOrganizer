using TitlesOrganizer.Domain.Interfaces.Base;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Domain.Interfaces
{
    public interface IBookSeriesCommandsRepository : ICommandsRepository<Series>
    {
        void UpdateBookSeriesBooksRelation(Series series);
    }
}