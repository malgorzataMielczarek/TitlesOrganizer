using TitlesOrganizer.Application.Interfaces;
using TitlesOrganizer.Domain.Interfaces;

namespace TitlesOrganizer.Application.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;

        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }
    }
}