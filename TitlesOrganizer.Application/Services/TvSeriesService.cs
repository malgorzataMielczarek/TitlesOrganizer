using TitlesOrganizer.Application.Interfaces;
using TitlesOrganizer.Domain.Interfaces;

namespace TitlesOrganizer.Application.Services
{
    public class TvSeriesService : ITvSeriesService
    {
        private readonly ITvSeriesRepository _tvSeriesRepository;

        public TvSeriesService(ITvSeriesRepository tvSeriesRepository)
        {
            _tvSeriesRepository = tvSeriesRepository;
        }
    }
}