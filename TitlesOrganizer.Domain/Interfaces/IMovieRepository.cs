using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Domain.Interfaces
{
    public interface IMovieRepository
    {
        int AddMovie(Movie movie);

        int AddCountry(int movieId, int countryId);

        int AddExistingDirector(int movieId, int directorId);

        int AddExistingGenre(int movieId, int genreId);

        int AddNewDirector(int movieId, Director director);

        int AddNewGenre(int movieId, VideoGenre genre);

        void DeleteMovie(int movieId);

        int UpdateMovie(Movie movie);
    }
}