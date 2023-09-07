using TitlesOrganizer.Application.Interfaces;
using TitlesOrganizer.Domain.Interfaces;

namespace TitlesOrganizer.Application.Services
{
    public class MovieService : VideoService, IMovieService
    {
        private readonly IMovieRepository _movieRepository;

        public MovieService(IVideoRepository videoRepository, IMovieRepository movieRepository) : base(videoRepository)
        {
            _movieRepository = movieRepository;
        }
    }
}