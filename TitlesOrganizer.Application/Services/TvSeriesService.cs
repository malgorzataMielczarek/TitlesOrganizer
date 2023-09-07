using TitlesOrganizer.Application.Interfaces;
using TitlesOrganizer.Domain.Interfaces;

namespace TitlesOrganizer.Application.Services
{
    public class TvSeriesService : VideoService, ITvSeriesService
    {
        private readonly ITvSeriesRepository _tvSeriesRepository;

        public TvSeriesService(IVideoRepository videoRepository, ITvSeriesRepository tvSeriesRepository) : base(videoRepository)
        {
            _tvSeriesRepository = tvSeriesRepository;
        }
    }
}